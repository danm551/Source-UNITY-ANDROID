using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages overall behavior of player projectile.
public class PlayerProjectileManager : MonoBehaviour {
    Rigidbody2D rb;
    public Vector2 speed;
    public AudioSource audioSource;
    public AudioClip playerLaser;

    void Awake()
    {
        audioSource.clip = playerLaser;
        audioSource.Play();
    }

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
    }

    void Update()
    {
        Travel();
    }

    void Travel()
    {
        rb.velocity = speed; 
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject); 
        }
    }
}
