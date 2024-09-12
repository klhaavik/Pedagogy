using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetJump : MonoBehaviour
{
    Movement3D movement;
    // Start is called before the first frame update
    void Start(){
        movement = GameObject.Find("Player").GetComponent<Movement3D>();
    }
    void OnCollisionEnter(Collision col){
        print("col");
        movement.ResetJump();
    }
}
