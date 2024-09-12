using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LockHacker : MonoBehaviour
{
    public static string word = "adieu";
    //i dont remember why this char thing is here but i think it is important
    char[] letters = new char[word.Length];
    //list of gameobjects that will display status of letters
    //randomly placed to throw off players
    // 0 is d, 1 is u, 2 is i, 3 is a, 4 is e
    public List<Text> letterDisp = new List<Text>();
    //if youre wondering why its not working even when active, it might be disabled in the actual game
    public static bool active = false;
    string letterPressed = "";
    string guess = "";
    Event e;
    bool keyDown = false;
    public InputField inputBox;
    int guesses = 0;
    public Canvas displayer;
    public Canvas playerui;
    public Text noticeBox;
    float timer = 0;
    bool playTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        //initializes possible letters as an array
        letters = word.ToCharArray();
        //testing to see if worked
        foreach (char c in letters)
        {
            print("Abkhazian006: " + c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //why is this purple
        //plan: make a lock hacking quest where players hack using key logging
        //there is a list of keys that are logged; players have to combine those to form a word
        //that word is the password
        //also make a thing that sets camera to look at this thing. israel new prime minister

        if (!active)
        {
            //commented because it causes an error with title screen
            //displayer.enabled = false;
            //playerui.enabled = true;
            return;
        } else
        {
            displayer.enabled = true;
            playerui.enabled = false;
        }
        if (playTimer)
        {
            timer += Time.deltaTime;
            noticeBox.text = "The password was 'adieu'. You guessed it!";
        }
        if (timer > 5)
        {
            noticeBox.text = "";
            playTimer = false;
            displayer.enabled = false;
            playerui.enabled = true;
            active = false;
        }
        //have some cool displaying thing here maybe: like wordle type shit checklist for this montgrst:
        //-dont need to display options cause they already exist. jaoa felix chelsea 
        //-might want to change their fucking color to green to indicarwe right position
        //-use my cool cat font????
        //-i dont see anything else it should be easy to make and complete for users

        guess = inputBox.text.ToLower();
        print(guess.Length);

        //check if win when player enters the thing
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (guess == word)
            {
                playTimer = true;
            }
            if (guess.Length == 5)
            {
                inputBox.text = "";
                noticeBox.text = "";
                //changing text contents of boxes
                char[] charLetters = new char[guess.Length];
                charLetters = guess.ToCharArray();
                for (int i = 0; i < letterDisp.Count; i++)
                {
                    letterDisp[i].text = charLetters[i].ToString();
                }
                //checking if guess has letters in the word, might change to text color
                foreach (char c in charLetters)
                {
                    if (Array.Exists(letters, element => element == c))
                    {
                        letterDisp[Array.IndexOf(charLetters, c)].color = Color.yellow;
                        if (Array.IndexOf(letters, c) == Array.IndexOf(charLetters, c))
                        {
                            letterDisp[Array.IndexOf(letters, c)].GetComponent<Text>().color = Color.green;
                        }
                    }
                }
                //the word is guessed
                if (guess == word)
                {
                    //whateverScript.unlocked = true;
                    playTimer = true;
                }
                //reassigning text boxes. im sorry for doing it like this
                guesses++;
                int x = 0;
                for (int i = 0; i < letterDisp.Count; i++)
                {
                    string name = "Text" + x + "0 (" + guesses + ")";
                    letterDisp[i] = GameObject.Find(name).GetComponent<Text>();
                    x++;
                }
            } else
            {
                noticeBox.text = "The password is 5 letters long!";
            }
        }
    }
    public static void StartQuest()
    {
        active = true;
    }
}
