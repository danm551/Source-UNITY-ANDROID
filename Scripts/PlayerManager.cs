using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the overall behavior of the player ship.
public class PlayerManager : MonoBehaviour
{
    public GameObject projectile;
    public Text debugMove, debugShoot;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip playerExplosion;
    public static bool playerAlive = true;
    public float shotDelay = 2f;
    Transform firePosTransform;
    Animator playerFirePosAnimator;
    float screenRestriction = (Screen.height / 2), speed = .5f, shipBoundary = .6f, shotTime = 0;
    bool deltaX = false, deltaY = false;
    int moveTouch;


    void Start()
    {
        firePosTransform = transform.Find("Player Fire Position");
        playerFirePosAnimator = GameObject.Find("Player Fire Position").GetComponent<Animator>();
    }

    void Update()
    {
        if (playerAlive)
        {
            Movement();
        }
    }

    /// <summary>
    ///     Handles player movement
    ///     Includes platform specific directives for handling touchscreen and mouse/keyboard interfaces
    /// </summary>
    void Movement()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float screenWidth = Camera.main.orthographicSize * screenRatio;

        Vector3 shipPosition = GameObject.Find("Player").transform.position;

#if UNITY_ANDROID
        //debugMove.text = "Here in android";
        for (int i = 0; i < Input.touchCount; i++)
        {
            if ((Input.GetTouch(i).phase == TouchPhase.Began) && (Input.GetTouch(i).position.x <= screenRestriction))
            {
                moveTouch = Input.GetTouch(i).fingerId;
            }
            else if((Input.GetTouch(i).phase == TouchPhase.Began) && (Input.GetTouch(i).position.x > screenRestriction))
            {
                if (shotTime == 0)
                {
                    shotTime = Time.time;
                    Shoot();
                }
                else if ((Time.time - shotTime) >= shotDelay)
                {
                    shotTime = Time.time;
                    Shoot();
                }
            }

            if ((Input.GetTouch(i).phase == TouchPhase.Moved) && (Input.GetTouch(i).position.x <= screenRestriction) && Input.GetTouch(i).fingerId == moveTouch)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(i).deltaPosition;

                if ((shipPosition.x < (screenWidth - shipBoundary)) && (shipPosition.x > (-screenWidth + shipBoundary)))
                {
                    deltaX = true;
                }
                else if (shipPosition.x > (screenWidth - shipBoundary) && touchDeltaPosition.x < 0)
                {
                    deltaX = true;
                }
                else if (shipPosition.x < (-screenWidth + shipBoundary) && touchDeltaPosition.x > 0)
                {
                    deltaX = true;
                }
                else
                {
                    deltaX = false;
                }

                if ((shipPosition.y < (Camera.main.orthographicSize - shipBoundary)) && (shipPosition.y > (-Camera.main.orthographicSize + shipBoundary)))
                {
                    deltaY = true;
                }
                else if (shipPosition.y >= (Camera.main.orthographicSize - shipBoundary) && touchDeltaPosition.y < 0)
                {
                    deltaY = true;
                }
                else if (shipPosition.y <= (-Camera.main.orthographicSize + shipBoundary) && touchDeltaPosition.y > 0)
                {
                    deltaY = true;
                }
                else
                {
                    deltaY = false;
                }

                if (deltaX && deltaY)
                {
                    transform.Translate(touchDeltaPosition.x * speed * Time.deltaTime, touchDeltaPosition.y * speed * Time.deltaTime, 0);
                }
            }
        }
#endif

#if UNITY_EDITOR_WIN
        speed = 8f;

        if (Input.GetKey(KeyCode.D) && shipPosition.x < (screenWidth - shipBoundary))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A) && shipPosition.x > (-screenWidth + shipBoundary))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W) && shipPosition.y < (Camera.main.orthographicSize - shipBoundary))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S) && shipPosition.y > (-Camera.main.orthographicSize + shipBoundary))
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }

        if ((Input.GetMouseButtonDown(0))){
            if (shotTime == 0)
            {
                shotTime = Time.time;
                Shoot();
            }
            else if ((Time.time - shotTime) >= shotDelay)
            {
                shotTime = Time.time;
                Shoot();
            }
        }
#endif
    }


    void Shoot()
    {
        playerFirePosAnimator.Play("player_muzzle_flash");
        animator.Play("shoot");
        Instantiate(projectile, firePosTransform.position, Quaternion.identity);
    }

    /// <summary>
    ///     Calls for death procedure  when collision with enemy or projectile is detected
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy Projectile") || coll.gameObject.CompareTag("Enemy"))
        {
            playerAlive = false;

            StartCoroutine(ApplyDeath());
        }
    }

    /// <summary>
    ///     Handles player death procedure:
    ///         Plays sound, animation
    ///         Waits...
    ///         Deatroys player ship
    /// </summary>
    /// <returns></returns>
    IEnumerator ApplyDeath()
    {
        audioSource.clip = playerExplosion;
        audioSource.Play();

        Animator animator = gameObject.GetComponent<Animator>();
        animator.transform.localScale = new Vector2(1.5f, 1.5f);// increases animation size
        animator.Play("player_explosion");

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        Destroy(gameObject, playerExplosion.length); //destroy is on delay to allow sound to play
    }
}
