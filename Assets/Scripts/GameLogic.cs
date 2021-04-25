using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class GameLogic : MonoBehaviour
{
    [Header("Menu")]
    public GameObject Menu;

    [Header("HUD - Depth indicator")]
    public GameObject fallingMarker;
    public TMP_Text Depth;
    [Header("Shader")]
    public Autostereogram shader;
    [Header("Obstacle Spwaning")]
    public GameObject obstacle;
    public float interval;
    public float timeScale = 0.99f;

    private bool falling = false ;
    private bool started = false;
    private bool paused = false; 
    private int currentDepth = 0;
    private float savedTimeScale = 0;

    private bool menuEntered = false; 

    private void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        //Game Logic
        if( started && !paused )
        {
            GameInProgressUpdate();
        } else
        {
            MenuUpdate();
        }
    }

    void GameInProgressUpdate()
    {
        if (falling)
        {
            currentDepth = ((int)-fallingMarker.transform.position.y);
            Depth.text = "Current Depth : " + currentDepth;
        }
        else
        {
            Depth.text = "Game over : " + currentDepth;
        }
        //Reset Game 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            menuEntered = true;
        }
    }
    void MenuUpdate()
    {
        if (Input.anyKey)
        {
            if (!Input.GetKey(KeyCode.Mouse0) && !menuEntered)
            {
                if (paused)
                {
                    ContinueGame();
                } else
                {
                    StartGame();
                }
            }
        } else
        {
            menuEntered = false;
        }
    }
    //----------- Game State Logic -----------//
    void StartGame()
    {
        started = true;
        fallingMarker.transform.position = new Vector3(5, 0, 0);
        Time.timeScale = 1;
        Menu.SetActive(false);
    }
    void PauseGame()
    {
        savedTimeScale = timeScale; 
        Time.timeScale = 0;
        Menu.SetActive(true);
        paused = true; 
    }
    void ContinueGame() {
        Time.timeScale = savedTimeScale;
        Menu.SetActive(false);
        paused = false;
    }
    //----------- Obstacle Spawning -----------//
    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // wait two minutes
            spawn();
            Time.timeScale += 0.01f;
        }
    }
    private void spawn()
    {
        Instantiate(obstacle, new Vector3(0, 50, 0), Quaternion.Euler(0, Random.Range(-90, 90), 0), this.transform);
    }
    //----------- Collision Handling -----------//
    void OnCollisionEnter(Collision collision)
    {
        falling = false;
        shader.main.color = new Color(1, 0.8f, 0.8f, 1);
        StartCoroutine("Splash");
    }
    private IEnumerator Splash()
    {
        yield return new WaitForSeconds(0.1f); // wait two minutes
        shader.main.color = Color.white;
    }
}
