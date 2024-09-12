using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class StartZoom : MonoBehaviour
{
    bool right = true;
    bool left = true;
    Camera cam;
    CinemachineVirtualCamera vc;

    //distance from camera starts at 2 and will return to 4 when done.
    // Start is called before the first frame update
    void Start()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!right)
        {
            //display press d to move right, icon
            //displayicon.setactive
        } else if (!left)
        {
            //display press a to move left, icon
            //displayicon.setactive
        } else if (right && left)
        {
            CinemachineComponentBase componentBase = vc.GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase as CinemachineFramingTransposer).m_CameraDistance = 4;
            //GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent(CinemachineCore.Stage.Body).
        }
    }
}
