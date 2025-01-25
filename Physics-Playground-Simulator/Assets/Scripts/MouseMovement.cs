using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


/// <summary>
/// Script to enable mouse movement
/// </summary>
public class MouseMovement : MonoBehaviour

{   
    // default mouse sensitivity 
    public float mouseSensitivity = 300f;
    
    // tracks the vertical rotation 
    // we seperate this from transform so we can enable rotation clamping (prevents player from flipping)
    float xRotation = 0f;
    // horizontal 
    float yRotation = 0f; 

    // prevent camera from rotating beyond 90 and -90 degrees
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {   
        //removes cursor
         Cursor.lockState = CursorLockMode.Locked; 

    }
    
    void Update()
    {

    // getting mouse inputs 
    // multiple by sensitivity 
    // time delta makes movement frame-rate independent 
    // otherwise sensitivity will be changing depending on the system performance
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    // invert vertical rotation so up mouse movement looks up
    // subtracting makes up, up. 
    xRotation -= mouseY;
    
    // restrict the vertical rotation 
    xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

    // mouseX allows continuous horizontal rotation. 
    yRotation += mouseX;

    // convert rotation values to euler angles (smoother)
    // only rotate about x and y. keep z = 0
    // 
    transform.localRotation = Quaternion.Euler (xRotation, yRotation, 0f);

    }
}

    

