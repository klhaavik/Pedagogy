using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLightCullingV2 : MonoBehaviour
{
    Light l;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Light>())
        {
            l = GetComponent<Light>();
            l.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<MeshRenderer>().enabled)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnBecameInvisible()
    {
        if (l)
        {
            l.enabled = false;
        }
    }
    private void OnBecameVisible()
    {
        if (l)
        {
            l.enabled = true;
        }
    }
}
