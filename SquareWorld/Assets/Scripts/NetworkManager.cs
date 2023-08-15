using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : NetworkBehaviour
{
    public Mirror.NetworkManager networkManager;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Debug.Log("Server is online");
    }

    public void OnApplicationQuit()
    {
        if (networkManager != null)
        {
            if(isServerOnly)
            {
                networkManager.StopServer();
            }else if (isServer&&isClient)
            {
                networkManager.StopHost();
            }
            else if (isClientOnly)
            {
                networkManager.StopClient();
            }
        }
    }
}
