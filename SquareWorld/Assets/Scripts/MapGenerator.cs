using Mirror;
using System.Collections.Generic;
//using System;
using UnityEngine;

public class MapGenerator : NetworkBehaviour
{
    public bool ServerOnStart = false;
    public int przeszkody = 5;
    public float radiusBlock = 3;
    public GameObject Przeszkoda;
    public GameObject Kolce;
    public List<Vector3> points;
    public GameObject Panel;
    public static bool ready = false;

    public delegate void MapReady();
    public static event MapReady OnMapReady;



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
            var kolce = new List<Vector3>();
            var bloki = new List<Vector3>();

            points = new();
            for (int i = 0; i < przeszkody; i++)
            {
                res:
                float x = Random.Range(-20, 20);
                float y;
                if (i != 0)
                    y = Random.Range(.5f, 1 + i / 3);
                else
                    y = Random.Range(.5f, 1);
                float z = Random.Range(-20, 20);
                if (Vector3.Distance(new Vector3(0, 0, 0), new Vector3(x, y, z)) < radiusBlock)
                    goto res;
                points.Add(new Vector3(x, y, z));


                if (Random.Range(0, 3) == 2)
                    kolce.Add(new Vector3(x, y, z));
                else
                    bloki.Add(new Vector3(x, y, z));
            }
            SetPointsRPC(bloki.ToArray(),kolce.ToArray());

            ready = true;
            OnMapReady();
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
    public void SetPointsRPC(Vector3[] points, Vector3[] kolce)
    {
        foreach (var item in points)
        {
            Instantiate(Przeszkoda, item, Quaternion.identity);
        }

        foreach (var item in kolce)
        {
            Instantiate(Kolce, item, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(0, 0, 0), radiusBlock);
    }
}
