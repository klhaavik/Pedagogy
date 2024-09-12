using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    TutorialManager tm;
    public TutorialTrigger nextTrigger;
    public bool first;
    public bool last;
    bool triggerEnabled = false;

    void Start(){
        tm = GameObject.Find("Canvas").GetComponent<TutorialManager>();
        if(first) triggerEnabled = true;
    }

    void OnTriggerEnter(Collider col){
        if(triggerEnabled){
            print("Triggered");
            tm.UpdateTutorial();
            triggerEnabled = false;
            if(!last) nextTrigger.SetEnabled(true);
        }
    }

    public void SetEnabled(bool state){
        triggerEnabled = state;
        print("enabled");
    }
}
