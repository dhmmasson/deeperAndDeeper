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

    // Update is called once per frame
    void FixedUpdate()
    {
        float dHeight = transform.position.y - previousHeight; 
        foreach (GameObject obstacle in obstacles)
        {           
            obstacle.transform.Translate(new Vector3(0, dHeight, 0)); 
        }
        previousHeight = transform.position.y;
    }
}