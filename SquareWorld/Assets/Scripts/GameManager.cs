using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [System.Serializable]
    public class SpawnParticles
    {
        public string name;
        public GameObject Prefab;
    }

    public static GameManager Instance { private set; get; }
    public Mirror.NetworkManager networkManager;
    List<GameObject> players = new();
    public Text More;
    public float JumpOutMulti = 2.5f;
    public GameObject bubble;
    public SpawnParticles[] spawnParticles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }
    private void Start()
    {
        if (networkManager == null)
        {
            networkManager = FindObjectOfType<Mirror.NetworkManager>();
        }
        MapGenerator.OnMapReady += LockPlayers;
    }
    public void LockPlayers()
    {
        players.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));

    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (MapGenerator.ready)
        {
            NetworkManager x = GetComponent<NetworkManager>();
            x.OnStopClient();
        }
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //if (connectionToServer.isReady)
            //{
            //    networkManager.StopClient();
            //}
            Application.Quit();
        }
        if (!MapGenerator.ready) return;



        if (isServer)
        {
            float max = 0;
            string nameUpper = string.Empty;
            foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (item.transform.position.y > max)
                {
                    max = item.transform.position.y;
                    nameUpper = item.GetComponent<GraczSterowanie>().username;
                }
            }
            string text = $"{nameUpper} - {max}m";

            Highscore(text);
            #region Buble Creation
            foreach (var item in FindObjectsOfType<GraczSterowanie>())
            {
                if (item.speedY == 0)
                    if (item.inJump)
                    {
                        if (item.lastJumped > 0)
                        {
                            SpawnBuble(item, item.transform.position, item.lastJumped);
                            item.inJump = false;
                        }
                    }
            }
            #endregion

        }



    }

    [ClientRpc]
    public void Highscore(string text)
    {

        More.text = text;

    }

    [ServerCallback]
    public void SpawnBuble(GraczSterowanie gracz, Vector3 position, int jumpsRemening)
    {
        try
        {
            foreach (var item in FindObjectsOfType<GraczSterowanie>())
            {
                item.JumpOff(position, jumpsRemening * JumpOutMulti, item == gracz);
            }
            print($"{position}, {jumpsRemening * JumpOutMulti}");
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning($"{ex.Message}\n\n{ex.InnerException}\n\n{ex.Source}\n\n{ex}");
        }
    }

    [ServerCallback]
    public void MovePlayer(GameObject playerToMove, Vector3 location, Particle useParticle = Particle.none)
    {
        foreach (var item in players)
        {
            item.GetComponent<GraczSterowanie>().Move(players.ToArray(), playerToMove, location, useParticle);
        }
    }
    private void OnApplicationQuit()
    {
        if (isServer && isClient)
            networkManager.StopHost();
        else if (isServerOnly)
            networkManager.StopServer();
        else if (isClientOnly)
            networkManager.StopClient();
    }

    public bool TryGetParticleByName(string particleName, out GameObject particle)
    {
        particle = null;
        foreach (var item in spawnParticles)
        {
            if (item.name == particleName)
            {
                particle = item.Prefab;
                return true;
            }
        }
        Debug.LogWarning("Particle " + particleName + " not found!");
        return false;
    }
}

public enum Particle
{
    none,
    death,
    teleport
}