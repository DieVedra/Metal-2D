using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;


    [SerializeField] private ParticleSystem _plasmagunEffectLight;

    private Animator _animation;
    private Rigidbody2D _rb;
    
    [SerializeField] private CinemachineVirtualCamera[] switchView;
    public float PositionPlayerX;
    public float stopCount;

    public Text speedTXT;


    [SerializeField]
    float speed;
    float speedCount;
    [SerializeField]
    int jumpForce;
    int moveInput;
    int moveInputInJump;

    public bool facingRight = true;

    bool isWalking = false;
    bool isWalkingFixed = false;

    bool isJumping = false;

    bool isJumpOnWalk = false;
    bool isJumpOnWalkAndIsJumpedFixed = false;

    private bool _isGround;
    private bool _isEnemy;
    [SerializeField] private Transform _feetPos;
    public float CheckRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsEnemy;


    [SerializeField] private GameObject _gunBolterUI;
    [SerializeField] private GameObject _gunPlasmUI;

    [SerializeField] private GameObject _ammoBolterUI;
    [SerializeField] private GameObject _ammoPlasmUI;

    public bool BulletShotUp = false;

    public bool PlasmAnimCount = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _animation = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        speedCount = speed;
    }

    private void Update()
    {

        //speedTXT.text = "speed: "  + speed.ToString() + "\n" + "isGround - " + isGround + "\n"
        //                + "\n" + "isWalking - " + isWalking + "\n"
        //                + "\n" + "isJumping - " + isJumping + "\n"
        //                + "\n" + "isJumpOnWalk - " + isJumpOnWalk + "\n";

        _isGround = Physics2D.OverlapCircle(_feetPos.position, CheckRadius, _whatIsGround);
        _isEnemy = Physics2D.OverlapCircle(_feetPos.position, CheckRadius, _whatIsEnemy);

        if (_isGround || _isEnemy)
        {
            _animation.SetBool("isGround", true);
        }
        else
        {
            _animation.SetBool("isGround", false);
        }


        PositionPlayerX = transform.position.x;

        if (isJumping && (_isGround || _isEnemy))
        {
            isJumping = false;

            speed = speedCount;

            isJumpOnWalk = false;

            if (moveInputInJump != 0)
            {
                moveInput = moveInputInJump;
            }
        }




        if (isWalking && (_isGround || _isEnemy))
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
            RbVelocityMove();
        }

        if (isJumpOnWalkAndIsJumpedFixed)
        {
            RbVelocityMove();
        }


        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }

        if (!isWalking && !PlasmAnimCount)
        {
            _animation.SetBool("isRunning", false);
            _animation.SetBool("PlasmaActive", false);
        }
        else if (isWalking && !PlasmAnimCount)
        {
            _animation.SetBool("isRunning", true);
            _animation.SetBool("PlasmaActive", false);
        }
        else if (!isWalking && PlasmAnimCount)
        {
            _animation.SetBool("isRunning", false);
            _animation.SetBool("PlasmaActive", true);
        }
        else if (isWalking && PlasmAnimCount)
        {
            _animation.SetBool("isRunning", true);
            _animation.SetBool("PlasmaActive", true);
        }

    }

    private void RbVelocityMove()
    {
        _rb.velocity = new Vector2(moveInput * speed, _rb.velocity.y);
    }

    private void Flip()
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
        if (!PlasmAnimCount)
        {
            _animation.SetBool("LookUp", true);
        }
        else
        {
            _animation.SetBool("LookUp", true);
            _animation.SetBool("PlasmaActive", true);
        }
        
        BulletShotUp = true;
    }

    public void OnLookSeeBottonUp()
    {
        _animation.SetBool("LookUp", false);
        BulletShotUp = false;
    }
    public void OnJumpButtonDown()
    {
        if (_isGround || _isEnemy)
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
        if (_gunBolterUI.activeSelf)
        {
            _gunBolterUI.SetActive(false);
            _gunPlasmUI.SetActive(true);
            PlasmAnimCount = true;
            PlasmagunEffect();
            _ammoBolterUI.SetActive(false);
            _ammoPlasmUI.SetActive(true);
        }
        else
        {
            _gunBolterUI.SetActive(true);
            _gunPlasmUI.SetActive(false);
            PlasmAnimCount = false;
            PlasmagunEffect();
            _ammoBolterUI.SetActive(true);
            _ammoPlasmUI.SetActive(false);
        }
    }

    private IEnumerator JumpCorutine()
    {
        _rb.velocity = Vector2.up * jumpForce;

        speed -= 1.5f;

        yield return new WaitForSeconds(0.04f);

        isJumping = true;

        if (isWalking)
        {
            isJumpOnWalk = true;
        }

        if (!PlasmAnimCount)
        {
            _animation.SetTrigger("StartJumping");
        }
        else
        {
            _animation.SetBool("PlasmaActive", true);
            _animation.SetTrigger("StartJumping");
        }

    }
    private void PlasmagunEffect()
    {
        if (PlasmAnimCount && Player.singletone.ammoPlasma > 0)
        {
            _plasmagunEffectLight.gameObject.SetActive(true);
        }
        else
        {
            _plasmagunEffectLight.gameObject.SetActive(false);
        }

    }
    public void StopPlasmagunEffect()
    {
        _plasmagunEffectLight.Stop();
    }
}
