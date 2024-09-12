using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalScroll : MonoBehaviour
{
    Renderer rend;
    float moveSpeed = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * moveSpeed;
        rend.material.SetTextureOffset("_BaseColorMap", new Vector2(offset, offset/1.5f));
        rend.material.SetTextureOffset("_DetailMap", new Vector2(offset/2f, -offset));
    }
}
