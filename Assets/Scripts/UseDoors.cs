using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDoors : MonoBehaviour
{
    GameObject target;
    public GameObject empty;
    public GameObject player;
    public bool changeRotation;
    public static bool openable;
    public static bool overwrite = false;
    // normal doors are -1.75, glass ones are -3.25
    public float hingePlacement;
    float rotation = 60;
    float timer = 0;
    bool opening = false;
    // Start is called before the first frame update
    void Start()
    {
        if (name.Contains("Glass"))
        {
            openable = false;
            //print("this: " + name + " result: " + openable);
        } else {
            openable = true;
        }
        if (changeRotation)
        {
            rotation *= -1;
        }
        target = Instantiate(empty, transform.position, Quaternion.identity);
        //target.transform.localScale = new Vector3(1, 1, 1);
        target.transform.parent = transform;
        //target.transform.position = transform.position + new Vector3(-1.75f, 0, 0);
        target.transform.localPosition = new Vector3(hingePlacement, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //print("error: " + name + " result: " + openable);
        if (name.Contains("Glass") && !overwrite)
        {
            openable = false;
            //print("fixpls: " + name + " result: " + openable);
        }
        else
        {
            openable = true;
        }
        if (Vector3.Distance(transform.position, player.transform.position) < 20 && Input.GetKey(KeyCode.R) && openable)
        {
            opening = true;
        }
        if (opening)
        {
            timer += Time.deltaTime;
            transform.RotateAround(target.transform.position, Vector3.up, rotation * Time.deltaTime);
        }
        if (timer > 1.55f)
        {
            timer = 0;
            opening = false;
            rotation *= -1;
        }
    }
}
