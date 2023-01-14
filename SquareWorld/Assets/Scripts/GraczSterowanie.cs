using Mirror;
using UnityEngine;

public class GraczSterowanie : NetworkBehaviour
{
    public Rigidbody rb;
    public float sila = 10f;
    public float silaSkoku = 50f;
    public int jumpAmount = 4;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(transform.Find("Cam").gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }


        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector3(0, 0, sila));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector3(0, 0, -sila));
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-sila, 0, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(sila, 0, 0));
        }
        if (jumpAmount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector3(0, silaSkoku, 0));
                jumpAmount -= 1;
            }
        }

        if (rb.velocity == Vector3.zero)
            jumpAmount = 5;


        if (gameObject.transform.position.y < -10)
        {
            rb.position = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }
}



