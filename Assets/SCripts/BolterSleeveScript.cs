using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolterSleeveScript : MonoBehaviour
{
    public Rigidbody2D rb;

    //public LayerMask whatIsGround;

    public float speedRotation;

    public float speedRotationRange;

    public int RBForsesCalledVal;


    public float forceX;

    public float forceY;

    public float lifeTime;

    void Start()
    {
        Invoke("DestroyOnLifetime", lifeTime);
    }
    private void FixedUpdate()
    {
            RBForses();
    }
    void RBForses()
    {
        if (RBForsesCalledVal >= 0)
        {
            RBForsesCalledVal--;

            if (PlayerController.instance.facingRight)
            {
                RBVelocity(1);
                RBTorque();
            }
            else
            {
                RBVelocity(-1);
                RBTorque();
            }
        }
    }
    void RBVelocity(int modify)
    {
        rb.velocity = new Vector2(forceX * modify, forceY * modify);
    }
    void RBTorque()
    {
        rb.AddTorque(Random.Range(speedRotation - speedRotationRange, speedRotation + speedRotationRange),
                     ForceMode2D.Force);
    }
    void DestroyOnLifetime()
    {
        Destroy(gameObject);
    }
}
