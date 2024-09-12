using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    GameObject target;
    public GameObject empty;
    public GameObject player;
    public bool changeRotation;
    // normal doors are -1.75, glass ones are -3.25
    public float hingePlacement;
    float rotation = 60;
    float timer = 0;
    bool opening = false;
    // Start is called before the first frame update
    void Start()
    {
        if (changeRotation)
        {
            rotation *= -1;
        }
        target = Instantiate(empty, transform.position, Quaternion.identity);
        //target.transform.localScale = new Vector3(1, 1, 1);
        target.transform.parent = transform;
        //target.transform.position = transform.position + new Vector3(-1.75f, 0, 0);
        target.transform.localPosition = new Vector3(hingePlacement,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 20 && Input.GetKey(KeyCode.E))
        {
            opening = true;
        }
        if (opening)
        {
            timer += Time.deltaTime;
            transform.RotateAround(target.transform.position, Vector3.up, rotation * Time.deltaTime);
        }
        if (timer>1.5f)
        {
            timer = 0;
            opening = false;
            rotation *= -1;
        }
    }
}
