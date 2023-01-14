using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : NetworkBehaviour
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        Debug.Log("Server is online");
    }
}
