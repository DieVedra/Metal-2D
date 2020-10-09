using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [HideInInspector]
    public Animator animationZomb1;


    Transform target;


    //[HideInInspector]
    //public int directionOfMovement;

    bool facingRight;

    public float health;


    //============================================

    public float startRunDistance; 

    public float attackDistanceToPlayer;  

     public float distanceToStay; 

    //public GameObject PointDetectPlayer;

    ////======================================================

    //public Transform pointAttack;

    //public float attackRange;

    //public LayerMask whatIsPlayer;

    //========================================================

    //public float timeRecharge;

    //public float startTimeRecharge;

    //public float health;

    public float damage;

    //========================================================

    bool walk;
    bool stay;
    bool attack;
    bool run;
    bool runStoping;

    //=========================================================

    [SerializeField]
    float speed;
    [SerializeField]
    float speedRun;
    float speedCount;

    //=========================================================

    string correntState;
    
    public const string zomb1Idle = "Zomb1_Idle";
    public const string zomb1Walking = "Zomb1_Walking";
    public const string zomb1StartingRun = "Zomb1_StartingRun";
    public const string zomb1StartAttacking = "Zomb1_StartAttacking";
    public const string zomb1Dead = "Zomb1_Dead";
    public const string zomb1StoppingRun = "Zomb1_StoppingRun";
    public const string zomb1EndAttacking = "Zomb1_EndAttacking";

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animationZomb1 = GetComponent<Animator>();

        speedCount = speed;
    }



    private void FixedUpdate()
     {
        if (walk && runStoping)
        {
            StopRunEnding();
        }
        else if(walk)
        {
            Walk();
        }

        if (stay && runStoping)
        {
            StopRunEnding();
        }
        else if(stay)
        {
            Stay();
            
        }

        if (attack)
        {
            Attack();
        }

        if (run)
        {
            Run();
        }

    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(transform.position, target.position));

        if ((Vector2.Distance(transform.position, target.position) > distanceToStay) && health > 0)
        {
            stay = true;
            walk = false;

        }
        else if ((Vector2.Distance(transform.position, target.position) < distanceToStay &&
                Vector2.Distance(transform.position, target.position) > startRunDistance) && health > 0)
        {
            stay = false;
            walk = true;

        }else
        //else if (
        //           (!(Vector2.Distance(transform.position, target.position) < distanceToStay &&
        //           Vector2.Distance(transform.position, target.position) > startRunDistance) &&
        //           !(Vector2.Distance(transform.position, target.position) > distanceToStay)) && health > 0
        //       )
        {
            stay = false;
            walk = false;

        }



        if ((Vector2.Distance(transform.position, target.position) < startRunDistance &&
            Vector2.Distance(transform.position, target.position) > attackDistanceToPlayer) &&
                             health > 0)
        {

            runStoping = true;
            run = true;


        }
        else run = false;



        if (Vector2.Distance(transform.position, target.position) < attackDistanceToPlayer && health > 0)
        {
            attack = true;
            runStoping = false;
        }
        else attack = false;


        if ((!facingRight && transform.position.x < target.position.x) ||
           (facingRight && transform.position.x > target.position.x))
        {
            Flip();
        }
    }

    void Stay()
    {
        speed = 0;

        ChangeAnimationState(zomb1Idle);
    }

    void Walk()
    {
        speed = speedCount;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        ChangeAnimationState(zomb1Walking);
    }

    void Run()
    {
        ChangeAnimationState(zomb1StartingRun);

        if (speed < speedRun)
        {
            speed += 0.1f;
        }


        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (attack == true)
        {
            return;
        }
    }

    void StopRunEnding()
    {
        ChangeAnimationState(zomb1StoppingRun);

        if (speed > speedCount)
        {
            speed -= 0.1f;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    void RunStopingBoolFalse()
    {
        runStoping = false;
    }

    void Attack()
    {
        speed = 0;

        ChangeAnimationState(zomb1StartAttacking);

        if (Player.singletone.healthPlayer <= 0)
        {
            ChangeAnimationState(zomb1EndAttacking);
            Invoke("Stay", 0.3f);
            attack = false;
            return;
        }
    }

    public void SendDamage()
    {
        Player.singletone.TakeDamage(damage);
    }


    void Flip()
    {
        
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void ChangeAnimationState(string newState)
    {
        if (correntState == newState)
        {
            return;
        }

        animationZomb1.Play(newState);

        correntState = newState;
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
                health -= damage;

            if (health == 0)
            {
                //ChangeAnimationState(zomb1Dead);
                animationZomb1.SetTrigger("DeadTrig");

                Invoke("Destruction", 3f);
                //Debug.Log(1);
            }
        }
        else return;
    }

    void Destruction()
    {

        Destroy(gameObject);
    }
}
