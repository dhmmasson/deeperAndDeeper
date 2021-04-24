using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed;
    //Use physic update for movement ?

 
    void FixedUpdate()
    {
        float rotation = - Input.GetAxis("Horizontal") * rotationSpeed;
        
        rotation *= Time.deltaTime;
        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
    }
}
