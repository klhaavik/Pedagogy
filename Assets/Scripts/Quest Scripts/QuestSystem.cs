using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    
    public static string currentRoom;
    static int questSelector;
    RaycastHit hit;
    int questLayerMask = 1 << 10;
    int checkMonsterLayerMask = 1 << 11;
    public static Text interactTxt;
    GameObject cam;
    GameObject monster;
    BoxCollider checkMonster;
    
    public static bool questActive = false;
    public static bool questStarted = false;
    public static int questsCompleted = 0;
    static List<int> selectedIndexes;

    string currentObj;
    static Text objText;
    //hey kai im going to use this for other scripts hence why it is static. Im going to add more things here too
    public static string[] objectives = {"Get to class on time", "Go to the office","Carpentry: Fix ladder", "Chemistry: Brew Potion", "Office: Fix PA System", "Music: Conduct Choir"};
    
    float t = 0.0f;
    bool monsterTimer = false;
    int threshold = 10;
    int interval = 10;
    float jumpscareTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //for testing
        currentRoom = "Carpentry";
        //questActive = true;
        
        objText = GameObject.Find("Objective").GetComponent<Text>();
        interactTxt = GameObject.Find("InteractText").GetComponent<Text>();
        cam = GameObject.Find("Main Camera");
        monster = GameObject.Find("Monster");
        checkMonster = GameObject.Find("checkMonsterTrigger").GetComponent<BoxCollider>();

        selectedIndexes = new List<int>();
        //objText.text = objectives[0];
        selectedIndexes.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if(!questActive){
            return;
        }

        //player dies and respawns
        if(!Player.movementEnabled){
            t += Time.deltaTime;
            //Debug.Log("failed");
            if(t >= 3.0f){
                Player.movementEnabled = true;
                t = 0.0f;
                Player.Die();
                Debug.Log("respawned");
            }
        }
        
        //displays interact text only if looking at quest object
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 30.0f, questLayerMask)&&!questStarted){
            interactTxt.enabled = true;
            if(Input.GetKeyDown(KeyCode.E)){
                StartQuest(currentRoom);
                questStarted = true;
            }
        }else{
            interactTxt.enabled = false;
        }

        //pause quest
        if (questStarted)
        {
            if(Input.GetKeyDown(KeyCode.E)){
                PauseQuest(currentRoom);
            }
            t += Time.deltaTime;
        }

        //failed quest and receive jumpscare
        if(monsterTimer&&(t>jumpscareTime)){
            monster.transform.position = transform.position + cam.transform.forward * 15;
            monsterTimer = false;
            Player.movementEnabled = false;
            t = 0.0f;
            questStarted = false;
        }

        //check for player response
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, checkMonsterLayerMask)&&monsterTimer){
            monsterTimer = false;
            checkMonster.enabled = false;
            //Debug.Log("avoided");
            jumpscareTime *= 0.8f;
            if(jumpscareTime<1.0f){
                jumpscareTime = 1.0f;
            }
        }

        //random chance of monster breathing which player has to check
        if(t < interval)
        {
            return;
        }
        int chance = Random.Range(1,10);
        
        if (true)
        {
            //play monster breathing sound
            //Debug.Log("monster breathed");
            monsterTimer = true;
            checkMonster.enabled = true;
            checkMonster.transform.position = transform.position - cam.transform.forward * 20;
        }
        threshold -= 1;
        t = 0.0f;
    }

    /*public static void ShowQuestActive(string room){
        bool inList = false;
        for (int i=0; i<rooms.Length; i++)
        {
            if (rooms[i]==room)
            {
                inList = true;
            }
            if(!inList){
                return;
            }
            
        }

        if(!questActive){
            //quests[questsCompleted].SetActive(true);
            print(room + " is activated");
            questActive = true;
        }
    }*/

    public void StartQuest(string room)
    {
        switch (room)
        {
            case "Office":
                Office.StartQuest();
                break;
            case "DiningHall":
                break;
            case "Chemistry":
                Chemistry.StartQuest();
                break;
            case "Carpentry":
                Carpentry.StartQuest();
                break;
            default:
                break;
        }
    }

    public void PauseQuest(string room){
        
        switch (room)
        {
            case "DiningHall":
                break;
            case "Carpentry":
                Carpentry.PauseQuest();
                break;
            case "Chemistry":
                Chemistry.PauseQuest();
                break;
            case "Office":
                //Office.PauseQuest();
                break;
            default:
                break;
        }
    }

    public static void CompleteQuest(){
        questActive = false;
        questStarted = false;
        bool pickingQuest = true;
        while(pickingQuest){
            pickingQuest = false;
            questSelector = Random.Range(1, objectives.Length-1);
            for(var i = 0; i < selectedIndexes.Count; i++){
                if(questSelector==i){
                    pickingQuest = true;
                    break;
                }
            }
        }
        objText.text = objectives[questSelector];
        selectedIndexes.Add(questSelector);
        string[] temp = objectives[questSelector].Split(':');
        currentRoom = temp[0];
    }
}
