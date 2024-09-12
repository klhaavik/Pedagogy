using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public Canvas ui;
    public Canvas title;
    public Canvas instruct;
    public Button playBtn;
    public GameObject monster;
    public GameObject player;
    public GameObject lighting;
    float timer = 0;
    bool playTimer = false;
    bool didScreen = false;

    /*On 2nd screen there should be:
     *  Survive
     *  Find a key to escape
     *  Shift to run
     *  E to open doors
     *  WASD to move
     */

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        monster = GameObject.Find("Monster");
        player = GameObject.Find("Player");
        //lighting.SetActive(false);
        if (!didScreen)
        {
            FadeScript.fade = false;
            ui.enabled = false;
            title.enabled = true;
        } else
        {
            FadeScript.fade = true;
            ui.enabled = true;
            title.enabled = false;
        }
        /*
        if (playTimer)
        {
            timer += Time.deltaTime;
        }
        if (timer > 7f)
        {
            instruct.enabled = false;
            player.GetComponent<AudioSource>().Play(); //beginning screeches
            monster.GetComponent<AudioSource>().Play(); //monster breathing
            Monster.caught = true;
            FadeScript.fade = true;
            ui.enabled = true;
        }
        print("timer:" + timer);*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (didScreen && !InstructionScreen.playTimer)
        {
            ui.enabled = true;
            title.enabled = false;
            FadeScript.fade = true;
            Monster.caught = true;
            /*player.GetComponent<AudioSource>().Play(); //beginning screeches
            monster.GetComponent<AudioSource>().Play(); //monster breathing
            
        }*/
    }

    public void OnPressPlay()
    {
        //SceneManager.LoadScene("SampleScene");
        Cursor.visible = false;
        //ui.enabled = true;
        title.enabled = false;
        //FadeScript.fade = true;
        //player.GetComponent<AudioSource>().Play(); //beginning screeches
        //monster.GetComponent<AudioSource>().Play(); //monster breathing
        //Monster.caught = true;
        playBtn.GetComponent<Button>().enabled = false;
        //lighting.SetActive(true);
        if (!didScreen)
        {
            instruct.enabled = true;
            InstructionScreen.playTimer = true;
            didScreen = true;
            //print("playTimer: " + playTimer);
        } else
        {
            ui.enabled = true;
            FadeScript.fade = true;
            player.GetComponent<AudioSource>().Play(); //beginning screeches
            monster.GetComponent<AudioSource>().Play(); //monster breathing
            Monster.caught = true;
        }
    }

    public void Restart(){
        SceneManager.LoadScene("SampleScene");
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}
