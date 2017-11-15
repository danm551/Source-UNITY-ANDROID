using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages overall behavior of enemy projectile.
public class EnemyProjectileManager : MonoBehaviour
{
    Rigidbody2D rb;
    public static Vector2 enemyProjectileSpeed = new Vector2(-10.00f, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = enemyProjectileSpeed;
    }

    void Update()
    {
        Travel();
    }

    void Travel()
    {
        rb.velocity = enemyProjectileSpeed;
    }
}