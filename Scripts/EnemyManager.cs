using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the overall behavior of the enemy
public class EnemyManager : MonoBehaviour {
    public GameObject enemyProjectile;
    public AudioSource audioSource;
    public AudioClip enemyExplosion;
    public static float enemySpeed = 4f;
    public static float enemyShootDelay = 2f;
    static int deathCount = 1, killsUntilNextLevel = 9;
    Transform enemyFirePos;
    Vector2 enemyPosition, currentVector;
    float enemyBoundary = 0.6f, shootTimer;
    bool isCurrentVectorNull = true, alive = true;

    void Start() {
        shootTimer = Time.time + enemyShootDelay; //sets shooting delay
        enemyFirePos = transform.Find("enemyFirePos");
    }

    /// <summary>
    ///     Provides momevent and shoot function only if enemy is alive
    /// </summary>
    void Update() {
        if (alive)
        {
            Movement();
            Shoot();
        }
    }

    /// <summary>
    ///     Initializes a new Vector2 with a random y value
    ///     Vector is applied on enemy initialization and upon hitting the top or bottom of play area
    /// </summary>
    void Movement()
    {
        enemyPosition = gameObject.transform.position;

        if (isCurrentVectorNull)
        {
            currentVector = new Vector2(-1, Random.Range(-2f, 2f));
            isCurrentVectorNull = false;
        }
        else if (enemyPosition.y > Camera.main.orthographicSize - enemyBoundary)
        {
            currentVector = new Vector2(-1, -2f);
        }
        else if (enemyPosition.y < -Camera.main.orthographicSize + enemyBoundary)
        {
            currentVector = new Vector2(-1, 2f);
        }

        transform.Translate(currentVector * enemySpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (shootTimer < Time.time)
        {
            Instantiate(enemyProjectile, enemyFirePos.position, Quaternion.identity);
            shootTimer = Time.time + enemyShootDelay;
        }
    }

    /// <summary>
    ///     Collision procedure:
    ///         Plays death animation
    ///         Increases score
    ///         Calls for level up procedure after ten deaths
    ///         Plays death sound
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player Projectile"))
        {
            alive = false;

            gameObject.transform.localScale = new Vector2(2, 2);
            Animator animator = gameObject.GetComponent<Animator>();
            animator.Play("enemy_explosion");

            GameManagement.score += 10;
            GameManagement.scoreText.text = "Score: " + GameManagement.score;

            if(deathCount == 10)
            {
                GameManagement.levelUp = true;
                deathCount = 0;
                killsUntilNextLevel = 10;
            }

            GameManagement.nextLevelText.text = "Kills : " + killsUntilNextLevel--;

            GameManagement.PlayEnemyDeathSound();
            gameObject.SetActive(false);
            Destroy(gameObject, enemyExplosion.length);

            deathCount++;
        }
    }
}
