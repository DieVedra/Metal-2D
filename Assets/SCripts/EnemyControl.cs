using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private Animator _animationZomb1;
    private Transform _target;
    private bool _facingRight;

    [SerializeField] private float health;

    //============================================

    [SerializeField] private float _startRunDistance;
    [SerializeField] private float _attackDistanceToPlayer;
    [SerializeField] private float _distanceToStay;

    ////======================================================

    [SerializeField] private float _damage;

    //========================================================

    private bool _walk;
    private bool _stay;
    private bool _attack;
    private bool _run;
    private bool _runStoping;

    //=========================================================

    [SerializeField] private float _speed;
    [SerializeField] private float _speedRun;
    private float _speedCount;

    //=========================================================

    private string _correntState;

    private const string zomb1Idle = "Zomb1_Idle";
    private const string zomb1Walking = "Zomb1_Walking";
    private const string zomb1StartingRun = "Zomb1_StartingRun";
    private const string zomb1StartAttacking = "Zomb1_StartAttacking";
    private const string zomb1Dead = "Zomb1_Dead";
    private const string zomb1StoppingRun = "Zomb1_StoppingRun";
    private const string zomb1EndAttacking = "Zomb1_EndAttacking";

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _animationZomb1 = GetComponent<Animator>();

        _speedCount = _speed;
    }

    private void FixedUpdate()
     {
        if (_walk && _runStoping)
        {
            StopRunEnding();
        }
        else if(_walk)
        {
            Walk();
        }

        if (_stay && _runStoping)
        {
            StopRunEnding();
        }
        else if(_stay)
        {
            Stay();
            
        }

        if (_attack)
        {
            Attack();
        }

        if (_run)
        {
            Run();
        }

    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(transform.position, target.position));

        if ((Vector2.Distance(transform.position, _target.position) > _distanceToStay) && health > 0)
        {
            _stay = true;
            _walk = false;

        }
        else if ((Vector2.Distance(transform.position, _target.position) < _distanceToStay &&
                Vector2.Distance(transform.position, _target.position) > _startRunDistance) && health > 0)
        {
            _stay = false;
            _walk = true;

        }
        else
        {
            _stay = false;
            _walk = false;
        }

        if ((Vector2.Distance(transform.position, _target.position) < _startRunDistance &&
            Vector2.Distance(transform.position, _target.position) > _attackDistanceToPlayer) &&
                             health > 0)
        {
            _runStoping = true;
            _run = true;
        }
        else _run = false;

        if (Vector2.Distance(transform.position, _target.position) < _attackDistanceToPlayer && health > 0)
        {
            _attack = true;
            _runStoping = false;
        }
        else _attack = false;


        if ((!_facingRight && transform.position.x < _target.position.x) ||
           (_facingRight && transform.position.x > _target.position.x))
        {
            Flip();
        }
    }

    private void Stay()
    {
        _speed = 0f;

        ChangeAnimationState(zomb1Idle);
    }

    private void Walk()
    {
        _speed = _speedCount;

        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        ChangeAnimationState(zomb1Walking);
    }

    private void Run()
    {
        ChangeAnimationState(zomb1StartingRun);

        if (_speed < _speedRun)
        {
            _speed += 0.1f;
        }


        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        if (_attack == true)
        {
            return;
        }
    }

    private void StopRunEnding()
    {
        ChangeAnimationState(zomb1StoppingRun);

        if (_speed > _speedCount)
        {
            _speed -= 0.1f;
        }

        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }
    private void RunStopingBoolFalse()
    {
        _runStoping = false;
    }

    private void Attack()
    {
        _speed = 0f;

        ChangeAnimationState(zomb1StartAttacking);

        if (Player.singletone.healthPlayer <= 0)
        {
            ChangeAnimationState(zomb1EndAttacking);
            Invoke("Stay", 0.3f);
            _attack = false;
            return;
        }
    }

    private void SendDamage()
    {
        Player.singletone.TakeDamage(_damage);
    }


    private void Flip()
    {
        
        _facingRight = !_facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void ChangeAnimationState(string newState)
    {
        if (_correntState == newState)
        {
            return;
        }

        _animationZomb1.Play(newState);

        _correntState = newState;
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
                health -= damage;

            if (health <= 0)
            {
                //ChangeAnimationState(zomb1Dead);
                _animationZomb1.SetTrigger("DeadTrig");

                Invoke("Destruction", 3f);
                //Debug.Log(1);
            }
        }
    }

    private void Destruction()
    {
        Destroy(gameObject);
    }
}
