using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public static Bullet instance = null;
    public float speed;
    public Rigidbody2D rb;

    public LayerMask whatIsSolid;

    public float distance;

    public float damageBull;

    public float lifeTime;

    public ExplosionBull exp;

    

    private void Start()
    {
        

            //rb.velocity = transform.right * speed;
            Invoke("DestroyOnLifetime", lifeTime);
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<EnemyControl>().TakeDamage(damageBull);
                exp.spawnExp();
            }
            Destroy(gameObject);
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void DestroyOnLifetime()
    {
        Destroy(gameObject);
    }
}