using Mirror;
using UnityEngine;
using UnityEngine.VFX;

public class GraczSterowanie : NetworkBehaviour
{
    private Rigidbody rb;
    public UIManager uiManager;
    public GameManager gameManager;
    [SyncVar] public string username;
    [SyncVar] public float speedY;
    public float sila = 10f;
    public float silaSkoku = 50f;
    public int jumpAmount = 4;
    [HideInInspector]
    public int maxJumpAmount;
    [HideInInspector]
    public bool inJump = false;
    [HideInInspector]
    public int lastJumped = 0;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(transform.Find("Cam").gameObject);
            Material mat = new(GetComponent<Renderer>().material)
            {
                name = username,
                color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))
            };

            GetComponent<Renderer>().material = mat;
        }
        else
        {
            uiManager = FindObjectOfType<UIManager>(true);
            username = uiManager.GetUsername();
            gameManager = FindObjectOfType<GameManager>(true);
            jumpAmount += 2;
            maxJumpAmount = jumpAmount;
            rb = GetComponent<Rigidbody>();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }
        speedY = rb.velocity.y;

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
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!inJump) lastJumped = 0;
                rb.AddForce(new Vector3(0, silaSkoku, 0));
                    jumpAmount -= 1;
                lastJumped += 1;
                inJump = true;
            }
        }

        if (rb.velocity == Vector3.zero)
        {
            jumpAmount = maxJumpAmount;
            //if (lastJumped >= 5)
            //    inJump = false;
        }



        if (gameObject.transform.position.y < -10)
        {
            rb.position = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

    [ClientRpc]
    public void JumpOff(Vector3 position, float radius, bool IsThisPlayer)
    {
        Instantiate(gameManager.bubble, position, Quaternion.identity).GetComponent<VisualEffect>().SetFloat("radius", radius);
        if (!IsThisPlayer)
        {
            rb.AddExplosionForce(200, position, radius);
        }
        inJump = false;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        Application.Quit();
    }
}



