using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearScript : MonoBehaviour
{
    public static bool appear = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().enabled = appear;
        } else if (GetComponent<Text>())
        {
            GetComponent<Text>().enabled = appear;
        }
    }
}
