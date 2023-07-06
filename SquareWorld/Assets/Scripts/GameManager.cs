using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GameManager : NetworkBehaviour
{
    public Mirror.NetworkManager networkManager;
    List<GameObject> players = new();
    public Text More;
    public float JumpOutMulti = 2.5f;
    public GameObject bubble;

    private void Start()
    {
        if (networkManager == null)
        {
            networkManager = FindObjectOfType<Mirror.NetworkManager>();
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (MapGenerator.ready)
        {
            NetworkManager x = GetComponent<NetworkManager>();
            x.OnStopClient();
        }
        else
        {
            players.Clear();
            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
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


        }

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

    [ClientRpc]
    public void Highscore(string text)
    {

        More.text = text;

    }

    [ServerCallback]
    public void SpawnBuble(GraczSterowanie gracz, Vector3 position, int jumpsRemening)
    {
        
        foreach (var item in FindObjectsOfType<GraczSterowanie>())
        {
                item.JumpOff(position, jumpsRemening * JumpOutMulti, item == gracz);
        }
        print($"{position}, {jumpsRemening * JumpOutMulti}");
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
}
