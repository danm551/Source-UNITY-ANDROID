using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the spawning of enemies.
public class SpawnManager : MonoBehaviour
{
    public GameObject enemyShip;
    public static float spawnDelay = 2;
    float randomY, timer = 0f;
    Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = GameObject.Find("Enemy Spawn Point").transform.position; //location of enemy spawn object
        timer = Time.time + spawnDelay; //sets spawn delay
    }

    /// <summary>
    ///     Determines if it's time to spawn enemy
    ///         Determines a random spawn position in relation to spawnPosition
    ///         Spawns enemy
    ///         Sets new timer
    /// </summary>
    void Update()
    {
        if (timer < Time.time)
        {
            randomY = Random.Range(-4.4f, 4.4f); //Y orthographic size of (5 - .6) for enemy boundary
            spawnPosition.y = randomY;
            Instantiate(enemyShip, spawnPosition, Quaternion.identity);
            timer = Time.time + spawnDelay;
        }
    }
}
