using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class updateDepth : MonoBehaviour
{
    public TMP_Text depth;
    public GameObject fallingMarker; 
    private bool falling = false ;
    private bool started = false;
    private int currentDepth = 0;
    public Autostereogram shader; 
    void OnCollisionEnter(Collision collision)
    {
        falling = false;
        Debug.Log("collision");
        shader.main.color = new Color(1, 0.8f, 0.8f, 1);
        StartCoroutine("Splash");
    }

    private IEnumerator Splash()
    {
            yield return new WaitForSeconds(0.1f); // wait two minutes
            shader.main.color = Color.white;
    }

    void Update()
    {
        if (falling)
        {
            currentDepth = ((int)-fallingMarker.transform.position.y);
            depth.text = "Current Depth : " + currentDepth;
        } else if( started )  {
            depth.text = "Game over : " + currentDepth;
        }
        //Restart game
        //TODO: move to actual game controler class
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fallingMarker.transform.position = new Vector3(5, 0, 0);
            Time.timeScale = 1;
            falling = true; 
        }

        //Start game
        //TODO: move to actual game controler class
        if (Input.anyKey && !started)
        {
            falling = true;
            started = true;
            Time.timeScale = 1;
        }

    }
}
