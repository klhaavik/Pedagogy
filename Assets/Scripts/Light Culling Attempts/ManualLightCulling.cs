using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLightCulling : MonoBehaviour
{
    public GameObject player;
    Light l;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (GetComponent<Light>())
        {
            l = GetComponent<Light>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(name + ": " + Vector3.Distance(player.transform.position, transform.position));
        //print()
        if (Vector3.Distance(player.transform.position, transform.position) > 250f && l)
        {
            l.enabled = false;
        } else if (l)
        {
            l.enabled = true;
        }
    }
}
