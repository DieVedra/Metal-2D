using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public static Bullet instance = null;
    public float speed;
    public Rigidbody2D rb;

    public float damage_Bull = 1f;

    public float lifeTime;

    

    private void Start()
    {
            rb.velocity = transform.right * speed;
            Invoke("DestroyOnLifetime", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage_Bull);
        }
        Destroy(gameObject);
    }

    void DestroyOnLifetime()
    {
        Destroy(gameObject);
    }
}