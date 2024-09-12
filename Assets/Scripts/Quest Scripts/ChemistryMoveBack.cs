using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistryMoveBack : MonoBehaviour
{
    public static bool putBack;
    bool didPutBack = false;
    Vector3 positionTo;
    public static bool used = false;
    //might use this later
    public static bool turnTrue = true;
    public static bool tpBack = false;
    // Start is called before the first frame update
    void Start()
    {
        positionTo = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Chemistry.objectHeld==gameObject && putBack)
        {
            print("rfgerfgerg");
            transform.position = Vector3.MoveTowards(transform.position, positionTo, 1f);
            if (turnTrue)
            {
                didPutBack = true;
                used = true;
            }
            turnTrue = true;
        }
        if (Chemistry.objectHeld == gameObject && transform.position == positionTo && didPutBack)
        {
            //this works so dont touch it! it resets the chemistry script for the next beaker.
            putBack = false;
            didPutBack = false;
            Chemistry.timer = 0;
            Chemistry.doTime = false;
            Chemistry.objectHeld = null;
            Chemistry.past90 = false;
            print("this thing is cool " + gameObject.name);
        }
        /*
        if (tpBack)
        {
            print("whaht" + positionTo);
            //idk why this happens but this works so dont touch it!
            transform.position = Vector3.MoveTowards(transform.position, positionTo, 1000000f);
            Chemistry.objectHeld = null;
            tpBack = false;
        }
        */
        if (transform.parent==null && !putBack && !Chemistry.pouring && !Chemistry.doTime)
        {
            transform.position = positionTo;
        }
    }
}
