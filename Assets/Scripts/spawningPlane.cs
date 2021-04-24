using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawningPlane : MonoBehaviour
{
    public GameObject obstacle;
    public float interval;
    public float timeScale = 0.99f; 
    void Start()
    {
        spawn();
        
        StartCoroutine("SpawnObstacle");
    }

    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // wait two minutes
            spawn();
            interval *= timeScale; 
        }
       
    }
    private void spawn()
    {
        Instantiate(obstacle, new Vector3(0, 50, 0), Quaternion.Euler(0, Random.Range(-90, 90), 0), this.transform);
    }
}
