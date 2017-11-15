using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the overall flow of the game.
public class GameManagement : MonoBehaviour {
    public static Text scoreText, levelBanner, levelLabel, nextLevelText;
    public static AudioSource audioSource;
    public static AudioClip enemyExplosion;
    public static int score = 0, level = 1;
    public static bool levelUp;
    GameObject playAgainPanel, pausePanel;
    float spawnRate = 0.01f, enemySpeedRate = 0.02f, enemyShootDelayRate = 0.01f, enemyProjectileSpeedRate = 0.02f;

    void Start() {
        Input.backButtonLeavesApp = false; 

        playAgainPanel = GameObject.Find("Panel Play Again");
        playAgainPanel.SetActive(false);
        pausePanel = GameObject.Find("Panel Pause");
        pausePanel.SetActive(false);

        scoreText = GameObject.Find("Text Score").GetComponent<Text>(); //used by EnemyManager
        levelBanner = GameObject.Find("Text Banner").GetComponent<Text>(); //text that fades in-and-out upon leveling
        levelLabel = GameObject.Find("Text Level").GetComponent<Text>(); //persistant level indicator
        nextLevelText = GameObject.Find("Text Next Level").GetComponent<Text>(); //kills tracker

        audioSource = GetComponent<AudioSource>();
        enemyExplosion = Resources.Load<AudioClip>("Audio/enemy_explosion");

        StartCoroutine(FadeText()); //displays first level banner
    }

    /// <summary>
    ///     Checks for escape key event
    ///     Checks player's alive state
    ///     Calls for LevelUp
    /// </summary>
    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }

        if (!PlayerManager.playerAlive)
        {
            playAgainPanel.SetActive(true);
        }

        LevelUp();
    }

    /// <summary>
    ///     Resets values to default to prepare for subsequent plays
    /// </summary>
    public void ResetValues()
    {
        SpawnManager.spawnDelay = 2;
        EnemyManager.enemySpeed = 4;
        EnemyManager.enemyShootDelay = 2;
        EnemyProjectileManager.enemyProjectileSpeed.x = -10.00f;
        level = 1;
        score = 0;
    }

    /// <summary>
    ///     Restores timeScale to 1 if it was set to 0 during pause state
    ///     Calls for default settings
    ///     Loads scene
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene(string scene)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        ResetValues();

        PlayerManager.playerAlive = true;
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    ///     Initializes level up procedure when EnemyManager has flagged enough enemey kills
    /// </summary>
    public void LevelUp()
    {
        if (levelUp)
        {
            level++;
            levelLabel.text = "Level : " + level;
            StartCoroutine(FadeText());
            if (SpawnManager.spawnDelay > .75f)
            {
                SpawnManager.spawnDelay -= spawnRate;
            }
            if(EnemyManager.enemySpeed < 10)
            {
                EnemyManager.enemySpeed += enemySpeedRate;
            }
            if (EnemyManager.enemyShootDelay > 1)
            {
                EnemyManager.enemyShootDelay -= enemyShootDelayRate;
            }
            if (EnemyProjectileManager.enemyProjectileSpeed.x > -30)
            {
                EnemyProjectileManager.enemyProjectileSpeed.x -= enemyProjectileSpeedRate;
            }

            levelUp = false;
        }
    }

    /// <summary>
    ///     Fades lelvel banner in and out
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeText()
    {
        levelBanner.CrossFadeAlpha(1.0f, 1f, false);

        levelBanner.text = "Level " + level;

        yield return new WaitForSeconds(2);

        levelBanner.CrossFadeAlpha(0.0f, 1f, false);
    }

    public static void PlayEnemyDeathSound()
    {
        audioSource.clip = enemyExplosion;
        audioSource.Play();
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
