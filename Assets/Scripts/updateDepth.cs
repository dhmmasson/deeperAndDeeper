using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class updateDepth : MonoBehaviour
{
    public TMP_Text depth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        depth.text = "Current Depth : " + (-transform.position.y); 
    }
}
