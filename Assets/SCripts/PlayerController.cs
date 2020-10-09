using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    Animator animation;
    Rigidbody2D rb;
    [HideInInspector]
    public CinemachineVirtualCamera[] switchView;
    //CinemachineTransposer cineTransposer;

    //public Transform positionPlayer;
    public float positionPlayerX;


    [SerializeField]
    int speed;
    [SerializeField]
    int jumpForce;
    float moveInput;

    bool facingRight = true;

    bool isGround;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public GameObject gunBolterUI;
    public GameObject gunPlasmUI;

    [HideInInspector]
    public bool bulletShotUp = false;

    public bool pcOrSensorControl;

    public bool plasmAnimCount = false;

    private void Awake()
    {
        // Setting up the references.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //positionPlayer = GetComponent<Transform>();
        //cameraFollowOffset = GetComponent<CinemachineVirtualCamera>();
        //cineTransposer = cameraFollowOffset.GetCinemachineComponent<CinemachineTransposer>();

    }

    private void Update()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //positionPlayer = transform.position;
        positionPlayerX = transform.position.x;


}
    private void FixedUpdate()
    {


        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);


        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        if (moveInput == 0 && plasmAnimCount == false)
        {
            animation.SetBool("isRunning", false);
            animation.SetBool("PlasmaActive", false);
        }
        else if(moveInput != 0 && plasmAnimCount == false)
        {
            animation.SetBool("isRunning", true);
            animation.SetBool("PlasmaActive", false);
        }
        else if(moveInput == 0 && plasmAnimCount == true)
        {
            animation.SetBool("isRunning", false);
            animation.SetBool("PlasmaActive", true);
        }
        else if(moveInput !=0 && plasmAnimCount == true)
        {
            animation.SetBool("isRunning", true);
            animation.SetBool("PlasmaActive", true);
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

        if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (!facingRight)
        {
            switchView[0].gameObject.SetActive(false);
            switchView[1].gameObject.SetActive(true);
        }
        else
        {
            switchView[0].gameObject.SetActive(true);
            switchView[1].gameObject.SetActive(false);
        }
    
    }


    public void OnLookSeeBottonDown()
    {
        if (!plasmAnimCount)
        {
            animation.SetBool("LookUp", true);
            moveInput = 0;
        }
        else
        {
            animation.SetBool("LookUp", true);
            animation.SetBool("PlasmaActive", true);
            moveInput = 0;
        }
        

        bulletShotUp = true;
    }

    public void OnLookSeeBottonUp()
    {

        animation.SetBool("LookUp", false);
        bulletShotUp = false;
    }


    public void OnJumpButtonDown()
    {
        if (isGround == true)
        {
            if (!plasmAnimCount)
            {
                animation.SetTrigger("StartJumping");
            }
            else
            {
                animation.SetBool("PlasmaActive", true);
                animation.SetTrigger("StartJumping");
            }
             
            
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    public void MoveOnBottonDown(int getAxis)
    {
        if (isGround == true)
        {
            moveInput = getAxis;
        }
        //Debug.Log(isGround);
        //Debug.Log(moveInput);
    }

    public void MoveOnBottonUp()
    {
        moveInput = 0;
    }

    public void SwitchGunBotton()
    {
        if (gunBolterUI.activeSelf)
        {
            gunBolterUI.SetActive(false);
            gunPlasmUI.SetActive(true);
            plasmAnimCount = true;
        }
        else
        {
            gunBolterUI.SetActive(true);
            gunPlasmUI.SetActive(false);
            plasmAnimCount = false;
        }


    }
}
