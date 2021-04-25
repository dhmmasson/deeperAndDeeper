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
    public GameObject GameUi;
    public TMP_Text DeepestUi;
    [Header("Shader")]
    public Autostereogram shader;
    [Header("Obstacle Spawning")]
    public GameObject obstacle;
    private Transform spawnPoint;

    public GameObject[] FlavorObject;

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
    private int deepest = 0; 


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
            Depth.text = "Current Depth : " + currentDepth +"m" ;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.P))
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
        
        savedTimeScale = 1;        
        ContinueGame();
    }
    void PauseGame()
    {
        savedTimeScale = timeScale; 
        Time.timeScale = 0;
        Menu.SetActive(true);
        GameUi.SetActive(false);        
        paused = true; 
    }
    void ContinueGame() {
        //Time.timeScale = savedTimeScale;
        StartCoroutine(rampTime(savedTimeScale));
        Menu.SetActive(false);
        GameUi.SetActive(true);         
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
        //for( int i = 0; i < Random.Range(2,5); i++ )
        //{            
        //    Instantiate(FlavorObject[Random.Range(0, FlavorObject.Length)], new Vector3(Random.Range(-10,10), Random.Range(10, 40), Random.Range(-10, 10)), Quaternion.Euler(0, Random.Range(-90, 90), 0), spawnPoint);
        //}
    }
    //----------- Collision Handling -----------//
    void OnCollisionEnter(Collision collision)
    {
        falling = false;
        shader.autoStereogram.color = new Color(0.64f, 0.36f, 0.364f, 1);
        if (currentDepth > deepest) deepest = currentDepth;
        DeepestUi.SetText("You went " + deepest + "m deep!\nCan you go deeper and deeper?");
        StartCoroutine("Splash");
        
    }
    private IEnumerator Splash()
    {
        yield return new WaitForSeconds(0.1f); // wait two minutes
        shader.autoStereogram.color = Color.black ;
        Time.timeScale = 1;
        StartGame();
    }
}
