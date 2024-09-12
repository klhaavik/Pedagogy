using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    float minus = 0.01f;
    RawImage raw;
    public static RawImage staticRaw;
    public static bool fade = false;
    // Start is called before the first frame update
    void Start()
    {
        raw = GetComponent<RawImage>();
        staticRaw = raw;
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            raw.color -= new Color(0, 0, 0, minus);
        }
    }
}
