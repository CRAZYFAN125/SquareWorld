using Mirror;
//using System;
using UnityEngine;

public class MapGenerator : NetworkBehaviour
{
    public bool ServerOnStart = false;
    public int przeszkody = 5;
    public GameObject Przeszkoda;
    public Vector3[] points;
    public GameObject Panel;

    private void Start()
    {
        if (isServerOnly)
        {
            if (ServerOnStart)
            {
                Mirror.NetworkManager.singleton.StartServer();
            }
            //else
            //{
            //    Console.WriteLine("Insert command...");
            //    }
        }
        Application.targetFrameRate = 30;
        //Panel.SetActive(true);
    }


    public void GenerateMap()
    {
        if (isServer)
        {
            points = new Vector3[przeszkody];
            for (int i = 0; i < przeszkody; i++)
            {
                float x = Random.Range(-20, 20);
                float y = 1;
                if (i != 0)
                    y = Random.Range(.5f, 1 + i / 3);
                else
                    y = Random.Range(.5f, 1);
                float z = Random.Range(-20, 20);
                points[i] = new Vector3(x, y, z);
                SetPointsRPC(points);
            }
        }
        //if (isClient)
        //{
        //    GetPointsFromServer();
        //}
    }

    //[Command]
    //private void GetPointsFromServer()
    //{
    //    SetPointsRPC(points);
    //}

    [ClientRpc]
    public void SetPointsRPC(Vector3[] points)
    {
        foreach (var item in points)
        {
            Instantiate(Przeszkoda, item, Quaternion.identity);
        }
    }
}
