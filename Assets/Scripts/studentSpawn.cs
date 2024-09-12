using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSpawn : MonoBehaviour
{
    GameObject person;
    int numPeople;
    GameObject people;
    GameObject x;
    
    public StudentSpawn(GameObject p, int num, GameObject people) {
        person = p;
        
        numPeople = num;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public string toString(){
        string txt = "";
        for(var i = 0; i < numPeople; i++){
            txt = txt + ", ";
        }
        return txt;
    }
}
