using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    float cameraDstNormal = 4f;
    float cameraDstFly = 6.5f;
    float currentDst;
    float smoothTime = 1.0f;
    float camDst; 
    CinemachineFramingTransposer cam;
    Movement3D player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Movement3D>();
        cam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        print(cam);
        //currentDst = cam.m_CameraDistance;
    }

    // Update is called once per frame
    void Update()
    {
        float newDst;
        if(player.HasBag()) {
            newDst = Mathf.SmoothDamp(cameraDstNormal, cameraDstFly, ref currentDst, smoothTime);
        }else{
            newDst = Mathf.SmoothDamp(cameraDstFly, cameraDstNormal, ref currentDst, smoothTime);
        }
        //cam.m_CameraDistance = newDst;
    }

}
