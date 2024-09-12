using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Office : MonoBehaviour
{
    public static Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("officeQuest", false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void StartQuest(){
        //play animation
        anim.SetBool("officeQuest", true);
        //when(animation is done){
        //play exposition over PA
        //}

        QuestSystem.questActive = false;
    }
}
