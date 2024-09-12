using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCheck : MonoBehaviour
{
    public float reloadDelay;
    public float reloadDuration;
    HeartWipe heartWipeController;
    Movement3D movement;

    void Start(){
        heartWipeController = GameObject.Find("Heart").GetComponent<HeartWipe>();
        movement = GetComponent<Movement3D>();
    }

    /*void OnCollisionEnter(Collision col){
        if(col.gameObject.layer == 9){
            StartCoroutine(Reset(2, 1.5f));
        }else if(col.gameObject.layer == 6){
            print("death");
            StartCoroutine(Reset(0, 1.5f));
        }
    }*/

    void OnTriggerEnter(Collider col){

    }

    /*private IEnumerator Reset(float reloadDelay, float reloadDuration){
        movement.SetEnableMovement(false);
        yield return new WaitForSeconds(reloadDelay);

        heartWipeController.FadeOut();
        yield return new WaitForSeconds(reloadDuration);

        movement.TriggerReset();
        heartWipeController.FadeIn();
        yield return new WaitForSeconds(0.25f);

        movement.SetEnableMovement(true);
    }*/
}
