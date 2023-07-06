using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesOnCubeScript : NetworkBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameManager.Instance;
    }

    
}
