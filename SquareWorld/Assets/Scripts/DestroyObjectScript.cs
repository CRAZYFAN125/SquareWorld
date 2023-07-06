using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectScript : MonoBehaviour
{
    [SerializeField]
    private float lifeTime=10;


    void FixedUpdate()
    {
        if (lifeTime < 0)
        {
            Destroy(gameObject);
            return;
        }
        lifeTime -= Time.fixedDeltaTime;
    }
}
