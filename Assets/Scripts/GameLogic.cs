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
    [Header("Obstacle Spawning")]
    public GameObject obstacle;
    private Transform spawnPoint; 
    public float interval;
    public float timeScale = 0.99f;
    [Header("Info")]
    public float currentTimeScale = 0; 
    private bool falling = false ;
    private bool started = false;
    private bool paused = false; 
    private int currentDepth = 0;
    private float savedTimeScale = 0;

    private bool menuEntered = false; 

    private void Start()
    {
        Time.timeScale = 0;
        spawnPoint = GameObject.FindGameObjectsWithTag("Respawn")[0].transform; 
        StartCoroutine("SpawnObstacle"); 
    }

    void Update()
    {
        currentTimeScale = Time.timeScale; 
        //Game Logic
        if ( started && !paused )
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
            //Check keypress but not if you just entered menu
            if ( (Input.GetKey(KeyCode.Escape)
               || Input.GetKey(KeyCode.LeftArrow)
               || Input.GetKey(KeyCode.RightArrow)
               || Input.GetKey(KeyCode.Space)) && !menuEntered)
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
            //No key pressed for a frame 
            menuEntered = false;
        }
    }
    //----------- Game State Logic -----------//
    void StartGame()
    {
        ClearGame();
        started = true;
        falling = true; 
        fallingMarker.transform.position = new Vector3(5, 0, 0);
        StartCoroutine(rampTime(1));
        //Time.timeScale = 1;
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
        //Time.timeScale = savedTimeScale;
        StartCoroutine(rampTime(savedTimeScale));
        Menu.SetActive(false);
        paused = false;
    }

    void ClearGame()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("falling");
        foreach (GameObject obstacle in obstacles)
        {
            GameObject.Destroy(obstacle); ;  
        }
    }
    //----------- Ramp up game speed -----------//
    private IEnumerator rampTime( float target)
    {
        while (Time.timeScale < target)
        {
            Time.timeScale += 0.1f;
            yield return new WaitForSeconds(0.2f); // wait two minutes                        
        }
    }
    //----------- Obstacle Spawning -----------//
    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // wait two minutes
            spawn();
            Time.timeScale += 0.015f;
        }
    }
    private void spawn()
    {
        Instantiate(obstacle, new Vector3(0, 50, 0), Quaternion.Euler(0, Random.Range(-90, 90), 0), spawnPoint);
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
        shader.main.color = Color.black ;
    }
}
