using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class playerController : MonoBehaviour
{

    public GameObject playerBullet;
    public bool playerIsImmortal = false; // Here you can cheat ;)
    public int playerLives = 3; // read by GUI script
    public bool isGameOver = false; // read by GUI script
    public int nextSceneIndex; // Index of the scene to load after game over
    public float gameOverDelay = 5.0f; // Delay before switching to the next scene after game over

    // Tuning
    private float pushUpForce = 6.0f; // force applied when fly button is tapped
    private float playerBulletXOffset = 0.5f;
    private float playerBulletYOffset = 0f;
    private float timeBetweenShots = 0.2f;  // 0.2 = 5 shots per second
    private float timestamp;

    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            Fire1Pressed();
        }
        if (Input.GetButton("Fire2"))
        {
            Fire2Pressed();
        }
    }

    void Fire1Pressed()
    {
        //Fly up
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, pushUpForce));
    }

    void Fire2Pressed()
    {
        if (Time.time >= timestamp)
        {
            // Shoot bullet
            Instantiate(playerBullet, transform.position + new Vector3(playerBulletXOffset, playerBulletYOffset, 0), Quaternion.identity);
            timestamp = Time.time + timeBetweenShots;
        }
    }

    // NB. Player and its bullet go in two dedicated layers where collisions must be disabled (Edit->project settings->physics 2D)
    // Collision with Trigger is for the bullets and enemies
    void OnTriggerEnter2D(Collider2D thisObject)
    {
        playerDidCollide();
    }

    // Normal Collision (no trigger) is for floor and ceiling, so we have physical constraints
    void OnCollisionEnter2D(Collision2D thisObject)
    {
        playerDidCollide();
    }

    void playerDidCollide()
    {
        if (playerLives > 0 && !playerIsImmortal)
        {
            playerLives--; // lose 1 life
        }
        else if (playerLives <= 0 && !playerIsImmortal)
        { // no lives remaining
            gameOver();
        }
    }

    void gameOver()
    {
        isGameOver = true; // This is picked by the Update function in GUI.cs
        Invoke("LoadNextScene", gameOverDelay); // Load next scene after a delay
        Time.timeScale = 0.0F; // Stop the game
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }

    
}
