using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public static EnemyControl instance = null;

    Animator animationZomb1;
    Rigidbody2D rb;

    int directionOfMovement;

    public float positionEnemyX;

    [SerializeField]
    bool facingRight;

    public float startRunFar;
    void Start()
    {
        animationZomb1 = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    [SerializeField]
    int speed;

    

    private void FixedUpdate()
     {
        rb.velocity = new Vector2(directionOfMovement * speed, rb.velocity.y);

        if (!facingRight && directionOfMovement > 0)
        {
            Flip();
        }
        else if (facingRight && directionOfMovement < 0)
        {
            Flip();
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        positionEnemyX = transform.position.x;

        if (PlayerController.instance.positionPlayerX < positionEnemyX)
        {
            directionOfMovement = -1;
            
        }
        else if (PlayerController.instance.positionPlayerX > positionEnemyX)
        {
            directionOfMovement = 1;
        }
        else
        {
            directionOfMovement = 0;
            
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

        //if (directionOfMovement < 0)
        //{
        //    transform.eulerAngles = new Vector3(0, 180, 0);
        //}
        //else if (directionOfMovement > 0)
        //{
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //}
    }
}
