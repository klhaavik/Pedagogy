using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float startingScrollSpeed = 1f;
    public float maxScrollSpeed = 2f;
    float scrollDelta = 0.001f;
    GameObject camPilot;
    public float scrollSpeed;

    void Start(){
        camPilot = GameObject.Find("CameraPilot");
        scrollSpeed = startingScrollSpeed;
    }

    void Update()
    {
        Vector2 pos = camPilot.transform.position; 
        pos.x += scrollSpeed * Time.deltaTime;
        camPilot.transform.position = pos;

        //if(scrollSpeed < maxScrollSpeed) scrollSpeed += scrollDelta;
    }
}
