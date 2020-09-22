using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public static Enemy instance = null;
    public float health;
    //public float damage;

    //public Transform pointAttack;

    //public float attackRange;

    //public LayerMask whatIsPlayer;

    //public float timeBtwAttack;
    //public float startTimeBtwAttack;

    //public const string zomb1StartAttacking = "Zomb1_StartAttacking";

    private void Start()
    {
        EnemyControl enemyControl = GetComponent<EnemyControl>();
    }

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Update()
    {

        OnCollider
        //if (timeBtwAttack <= 0)
        //{
            //Collider2D[] toDamagePlayer = Physics2D.OverlapCircleAll(pointAttack.position, attackRange, whatIsPlayer);
            //for (int i = 0; i < toDamagePlayer.Length; i++)
            //{
            //    toDamagePlayer[i].enemyControl.ChangeAnimationState(zomb1StartAttacking); 
            //    //timeBtwAttack = startTimeBtwAttack;
            //}


        //}
        //else
        //{
        //    timeBtwAttack -= Time.deltaTime;
        //}

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(pointAttack.position, attackRange);
    //}

    //public void TakeDamage(float damage)
    //{
    //    health -= damage;

    //    if (health <= 0)
    //    {
    //        Destruction();
    //    }
    //}

    //void Attack()
    //{
    //    Player.singletone.TakeDamage(damage);
    //}


    //void Destruction()
    //{
    //    Destroy(gameObject);
    //}
}
