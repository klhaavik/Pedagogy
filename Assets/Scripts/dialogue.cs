using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public List<GameObject> triggers = new List<GameObject>();
    public List<List<string>> dialogueOptions = new List<List<string>>();
    public static int index = 0;
    public static bool giveDialogue = false;
    float timer = 0;
    public Text text;
    int messi;
    // Start is called before the first frame update
    void Start()
    {
        triggers.Add(GameObject.Find("FrontEntrance"));
        dialogueOptions.Add(
            new List<string> {
                "<bit of atmospheric sound>",
                "<atmosphere fades> Hello students this is your principal speaking.",
                "Welcome back to another day at our great establishment.",
                "Today is an A day.",
                "Those in English Honors must report to room <room#> on the third floor",
                "You better hurry students. Time is running out.",
                "As always, go tigers!"
            }
        );

        triggers.Add(null);
        dialogueOptions.Add(
            new List<string> {
                "AAAAA blood! (see my comments in dialogue.cs)",
                "I gotta get out of here! (also see my comments)",
                "The nearest exit is near the cafeteria down the stairs" //change this!
                //this is of course not the final real dialogue, but there are some considerations for this later
                //we should not make the dialogue here too explanatory because it takes away from the player's experience
                //i suggest here we make the game spooky in this moment with ominous sound and effects
                //this way the player comes to the conclusion themselves rather than the game just stating it to them
            }
        );
        triggers.Add(GameObject.Find("DiningHallEntrance"));
        dialogueOptions.Add(
            new List<string> {
                "Oh no the door is locked!", //might want to express this through the actual door being locked, not dialogue
                "What's that mysterious figure running down the hall towards me?", //change this eventually
                "Quick, hide!"
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (giveDialogue)
        {
            if (messi < dialogueOptions[index].Count)
            {
                timer += Time.deltaTime;
                int intTimer = (int)timer;
                messi = intTimer / 5;
                text.text = dialogueOptions[index][messi];
            } else
            {
                if (index==0)
                {
                    QuestSystem.questActive = true;
                }
                index++;
                text.text = " ";
                giveDialogue = false;
            }
        } else
        {
            messi = 0;
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject==triggers[index])
        {
            giveDialogue = true;
        }
    }
}
