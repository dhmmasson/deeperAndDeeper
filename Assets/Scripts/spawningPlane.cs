using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawningPlane : MonoBehaviour
{
    public GameObject obstacle;
    public float interval;
    public float timeScale = 0.99f;
    public float timeS;
    void Start()
    {
        Time.timeScale = 0; 
        spawn();        
        StartCoroutine("SpawnObstacle");
    }

    private void Update()
    {
        //Start game
        //TODO: move to actual game controler class
        if (Input.anyKey)
        {
            Time.timeScale = 1;
        }

    }

    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // wait two minutes
            spawn();
            Time.timeScale += 0.01f;
            timeS = Time.timeScale;            
        }
       
    }
    private void spawn()
    {
        Instantiate(obstacle, new Vector3(0, 50, 0), Quaternion.Euler(0, Random.Range(-90, 90), 0), this.transform);
    }
}
