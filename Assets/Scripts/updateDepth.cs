using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class updateDepth : MonoBehaviour
{
    public TMP_Text depth;
    public GameObject fallingMarker; 
    private bool falling = true ;
    private int currentDepth = 0; 
    void OnCollisionEnter(Collision collision)
    {
        falling = false;
        Debug.Log("collision"); 
    }
    // Update is called once per frame
    void Update()
    {
        if (falling)
        {
            currentDepth = ((int)-fallingMarker.transform.position.y);
            depth.text = "Current Depth : " + currentDepth;
        }
        else
        {
            depth.text = "Game over : " + currentDepth;
        }
    }
}
