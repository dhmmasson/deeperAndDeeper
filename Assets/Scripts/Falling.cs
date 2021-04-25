using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    GameObject[] obstacles;
    float previousHeight; 
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("falling");
        previousHeight = transform.position.y; 
    }

    //Move all obstacles tagged as falling. Use a dummy falling object to get the deplacement
    void FixedUpdate()
    {
        float dHeight = transform.position.y - previousHeight;
        obstacles = GameObject.FindGameObjectsWithTag("falling");
        foreach (GameObject obstacle in obstacles)
        {           
            obstacle.transform.Translate(new Vector3(0, dHeight, 0));
            if (obstacle.transform.position.y < 0)
            {
                GameObject.Destroy(obstacle); ;
            }
        }       
        previousHeight = transform.position.y;
    }
}
