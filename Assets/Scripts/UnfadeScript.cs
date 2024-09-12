using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnfadeScript : MonoBehaviour
{
    float plus = 0.05f;
    public static RawImage rawImage;
    public static bool unfade = false;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unfade)
        {
            rawImage.color += new Color(0, 0, 0, plus);
        }
        if (rawImage.color.a > 0.6f && unfade)
        {
            AppearScript.appear = true;
        }
    }
}
