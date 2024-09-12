using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    KaiMovement movement;
    bool triggered = false;

    void Start(){
        movement = GameObject.Find("Player").GetComponent<KaiMovement>();
    }

    void OnTriggerEnter(Collider col){
        if(!triggered){
            movement.UpdateInitialValues();
            triggered = true;
        }   
    }
}
