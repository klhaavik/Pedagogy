using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionScreen : MonoBehaviour
{
    public static bool playTimer = false;
    public static float timer = 0;
    public GameObject player;
    public GameObject monster;
    public Canvas ui;
    public Canvas title;
    public GameObject black;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.C)) && !playTimer && !Monster.died && !title.enabled)
        {
            black.GetComponent<RawImage>().enabled = false;
            GetComponent<Canvas>().enabled = true;
        } else if (!playTimer)
        {
            black.GetComponent<RawImage>().enabled = true;
            GetComponent<Canvas>().enabled = false;
        }
        if (playTimer)
        {
            timer += Time.deltaTime;
            if (timer > 7f)
            {
                black.GetComponent<RawImage>().enabled = true;
                GetComponent<Canvas>().enabled = false;
                player.GetComponent<AudioSource>().Play(); //beginning screeches
                monster.GetComponent<AudioSource>().Play(); //monster breathing
                Monster.caught = true;
                FadeScript.fade = true;
                ui.enabled = true;
                playTimer = false;
            }
        }
        //print("timer:" + timer);
    }
}
