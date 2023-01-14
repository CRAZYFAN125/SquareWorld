using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CamControl : MonoBehaviour
{
    private GameObject gracz;

    [ClientCallback]
    // Update is called once per frame
    void Update()
    {
        if (gracz==null)
        {
            var x = FindObjectOfType<GraczSterowanie>();
            if (x!=null)
            {
                gracz = x.gameObject;
            }
            return;
        }
        gameObject.transform.position = gameObject.transform.position;
    }
}
