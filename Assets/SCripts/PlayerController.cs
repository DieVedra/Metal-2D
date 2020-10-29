using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour      
{
    //1. Ходьба isWalking 
    //2. Остановка !isWalking 
    //3. Прыжок isJumping isGround
    //4. Прыжок во время ходьбы с плавной остановкой в воздухе с приземлением без отпускания клавиши ходьбы isJumping && isWalking
    //5. Прыжок во время ходьбы с резкой остановкой в воздухе с приземлением и с отпусканием клавиши ходьбы isJumping && !isWalking
    public static PlayerController instance = null; 

    Animator animation;
    Rigidbody2D rb;
    [HideInInspector]
    public CinemachineVirtualCamera[] switchView;
    public float positionPlayerX;
    public float stopCount;

    public Text speedTXT;


    [SerializeField]
    float speed;
    float speedCount;
    [SerializeField]
    int jumpForce;
    int moveInput;
    int moveInputInJump;

    bool facingRight = true;

    bool isWalking = false;
    bool isWalkingFixed = false;

    bool isJumping = false;

    bool isJumpOnWalk = false;
    bool isJumpOnWalkAndIsJumpedFixed = false;

    bool isGround;
    public Transform feetPos;
    public Transform posJumpInMove;
    public float checkRadius;
    public LayerMask whatIsGround;

    public GameObject gunBolterUI;
    public GameObject gunPlasmUI;

    [HideInInspector]
    public bool bulletShotUp = false;

    //public bool pcOrSensorControl;

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
        speedCount = speed;
    }

    private void Update()
    {

        speedTXT.text = "speed: "  + speed.ToString() + "\n" + "isGround - " + isGround + "\n"
                        + "\n" + "isWalking - " + isWalking + "\n"
                        + "\n" + "isJumping - " + isJumping + "\n"
                        + "\n" + "isJumpOnWalk - " + isJumpOnWalk + "\n";




        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGround)
        {
            animation.SetBool("isGround",true);
        }
        else
        {
            animation.SetBool("isGround", false);
        }


        positionPlayerX = transform.position.x;

        if (isJumping && isGround)
        {
            isJumping = false;

            speed = speedCount;

            isJumpOnWalk = false;

            if (moveInputInJump != 0)
            {
                moveInput = moveInputInJump;
            }
        }




        if (isWalking && isGround)
        {
            isWalkingFixed = true;
        }
        else
        {
            isWalkingFixed = false;
        }

        if (isJumpOnWalk && isJumping)
        {
            isJumpOnWalkAndIsJumpedFixed = true;
        }
        else
        {
            isJumpOnWalkAndIsJumpedFixed = false;
        }




    }
    private void FixedUpdate()
    {

        if (isWalkingFixed)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        
        if(isJumpOnWalkAndIsJumpedFixed)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }



        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }

        if (!isWalking && !plasmAnimCount)
        {
            animation.SetBool("isRunning", false);
            animation.SetBool("PlasmaActive", false);
        }
        else if(isWalking && !plasmAnimCount)
        {
            animation.SetBool("isRunning", true);
            animation.SetBool("PlasmaActive", false);
        }
        else if(!isWalking && plasmAnimCount)
        {
            animation.SetBool("isRunning", false);
            animation.SetBool("PlasmaActive", true);
        }
        else if(isWalking && plasmAnimCount)
        {
            animation.SetBool("isRunning", true);
            animation.SetBool("PlasmaActive", true);
        }

    }

    void Flip()
    {
        if (isJumping)
        {
            return;
        }
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


    public void OnLookSeeButtonDown()
    {
        if (!plasmAnimCount)
        {
            animation.SetBool("LookUp", true);
        }
        else
        {
            animation.SetBool("LookUp", true);
            animation.SetBool("PlasmaActive", true);
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

        if (isGround)
        {
            StartCoroutine(JumpCorutine());
        }
    }


    public void MoveOnButtonDown(int getAxis)
    {
        if (isJumping)
        {
            moveInputInJump = getAxis;
        }
        else
        {

            moveInput = getAxis;
        }

            isWalking = true;
    }

    public void MoveOnButtonUp()
    {
        moveInputInJump = 0;
        isWalking = false;
    }

    public void SwitchGunButton()
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

    private IEnumerator JumpCorutine()
    {
        rb.velocity = Vector2.up * jumpForce;

        speed -= 1.5f;

        yield return new WaitForSeconds(0.04f);

        isJumping = true;

        if (isWalking)
        {
            isJumpOnWalk = true;
        }


        if (!plasmAnimCount)
        {
            animation.SetTrigger("StartJumping");
        }
        else
        {
            animation.SetBool("PlasmaActive", true);
            animation.SetTrigger("StartJumping");
        }

    }
}
