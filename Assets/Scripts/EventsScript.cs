using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsScript : MonoBehaviour
{
    //objects that will trigger an event
    /*this list should contain the following:
     * the front entrance/commons
     * your classroom where youre supposed to go at the start
     * the office
     * list may change based on what quest/path you choose to go down
    */
    public List<GameObject> triggers = new List<GameObject>();
    public static int index = -1;
    public static bool eventHappening = false;
    //public Audio announcement;
    //list containing all dialogue audio options
    //public List<Audio> dialogue;
    public static float timer = 0;
    public Text objText;
    public static Text staticText;
    public static RaycastHit hit;
    public GameObject blood;
    public static GameObject staticBlood;
    public GameObject audioPlayer;
    public Canvas screenHolder;
    // Start is called before the first frame update
    void Start()
    {
        staticText = objText;
        staticBlood = blood;
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 20f);
        if (eventHappening)
        {
            timer += Time.deltaTime;
            //the commented parts are concepts for stuff that needs to be implemented later.
            /*
             * i wouldnt reccomend switch case because you need several different conditions
            switch (index)
            {
                case 0:

                    break;
            }
            */
            if (index == 0 /*&& timer > announcement.length*/)
            {
                //doesnt have to be 2008, just the classroom youre going to
                objText.text = QuestSystem.objectives[0];
            }
            if (index == 1 && timer > 8f)
            {
                objText.text = QuestSystem.objectives[1];
                //audio guiding the player to office here
                timer = 0;
            }
            if (index == 2 /*&& PASystem.isFixed might not need this*/)
            {
                //clue.Play(); plays clue dialogue that exposes some of current situation
                objText.text = "Audio should play";
            }
            if (index == 2 && timer > 4f /*placeholder for audio's length to wait until its over*/)
            {
                //player has to make a choice on what to do next
                //screen goes to black and player must choose a path on screen
                objText.text = " ";
                UnfadeScript.rawImage.color = new Color(0, 0, 0, 0);
                UnfadeScript.unfade = true;
                screenHolder.enabled = true;
                //screenHolder.gameObject.SetActive(false);
                Cursor.visible = true;
                timer = 0;
                index++;
            }
            if (index == 3)
            {
                //placeholder for being inside the choice ui, may want some stuff here
            }
            if (index == 4)
            {
                //hallway jumpscares should be active at this point
                //there should be audio guiding to the corresponding location
                screenHolder.enabled = false;
                //screenHolder.gameObject.SetActive(true);
                //In case you want to fade ...
                //FadeScript.staticRaw.color = new Color(0, 0, 0, 1);
                //FadeScript.fade = true;
            }
            //when you complete the carpentry quest (keeping it as pressing e for debugging purposes)
            if (index == 4 && Input.GetKeyDown(KeyCode.E))
            {
                //ladder gets placed into inventory
                //when player is in right place, they are able to place the ladder in a spot to reach a cabinet
                //they will find the baton on the cabinet to do some choir conducting
            }
            print("Index: " + index);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //determining if object collided is an event object
        //i commented it because i found a more optimal way but this still could be useful
        if (triggers.Contains(collision.collider.gameObject))
        {
            /*
             * this may be useful later but not right now
            int objIndex = 0;
            for (int i=0; i < triggers.Count; i++)
            {
                if (collision.collider.gameObject == triggers[i])
                {
                    objIndex = i;
                    break;
                }
            }
            if (objIndex >= index)
            {
                //youre good!
            } else
            {
                return;
            }
            //specific one-time activation for events goes here: (if that makes sense)
            if (collision.collider.gameObject==triggers[index])
            {
                if (index == 0)
                {
                    //announcement will play, something like:
                    //announcement.Play();
                }
                if (index == 1)
                {
                    //maybe have some dialogue here
                    //sends you to the office
                    //dialogue[0].Play();
                }
                timer = 0;
                eventHappening = true;
            }
            */
        }
    }
    //this void is called in RoomTriggers script
    public static void BeginEvent(string name)
    {
        //assigning rooms local value
        int currentIndex = 0;
        switch (name)
        {
            case "bad":
                return;
            case "Front":
                currentIndex = 0;
                break;
            case "2005":
                currentIndex = 1;
                break;
            case "Office":
                currentIndex = 2;
                break;
            default:
                return;
        }
        //veryfying that its the right trigger for the part of the story youre on
        if (index+1==currentIndex)
        {
            //when you walk into the building
            if (currentIndex == 0)
            {
                //announcement will play, something like:
                //announcement.Play();
            }
            //when you walk into the class
            if (currentIndex == 1)
            {
                staticText.text = " ";
                //maybe have some dialogue here, like "why is the class so empty? i need to go to the office."
                //dialogue[0].Play();
            }
            //when you enter the office
            if (currentIndex == 2)
            {
                //allows player to access the PASystem to fix it. actually dont need this because player can only do that if they are in the room
            }
            //when you exit the office
            if (currentIndex == 3)
            {
                
            }
            timer = 0;
            eventHappening = true;
            index++;
        } else
        {
            return;
        }
    }
}
