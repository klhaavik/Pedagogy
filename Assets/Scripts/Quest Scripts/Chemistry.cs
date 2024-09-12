using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UI;

public class Chemistry : MonoBehaviour
{
    //if someone complains about the amount of variables just no
    float chemical1 = 0;
    float chemical2 = 0;
    public static GameObject objectHeld = null;
    public List<GameObject> beakers = new List<GameObject>();
    public GameObject hand;
    Ray ray;
    RaycastHit hit;
    public List<ParticleSystem> ps = new List<ParticleSystem>();
    List<GameObject> formula = new List<GameObject>();
    List<GameObject> used = new List<GameObject>();
    public ParticleSystem inUse;
    public GameObject stew;
    public static bool pouring = false;
    public static bool past90 = false;
    public static float timer = 0;
    public static bool doTime = false;
    float timer2 = 0;
    bool doPlay = true;
    public Material stewColor;
    public GameObject liquid;
    bool mistake = false;
    public static bool active = false;
    public Text display;
    float textTimer = 100;
    string textToDisplay = "";
    // Start is called before the first frame update
    void Start()
    {
        stewColor = liquid.GetComponent<MeshRenderer>().material;
        stewColor.color = new Color(0.5f, 0.5f, 0.5f);
        /*foreach (ParticleSystem p in ps)
        {
            p.GetComponent<Material>().color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // instruction text maybe????? help. i dont remember why i wrote this
        // if someone complains about this code go to brazil no go to east timor
        // this nightmare makes the liquid stop being ass, might redo this 
        if (timer2 < 100)
        {
            timer2 += Time.deltaTime;
        }
        /*if (timer2 > 1 && timer2 < 100)
        {
            foreach (ParticleSystem p in ps)
            {
                p.Pause();
            }
            timer2 = 10000;
            //dont touch this it works
        }*/
        textTimer += Time.deltaTime;
        // Potassium cyanide: K + C + N = KCN
        // Beakers full of K H O N C
        //seeedsfraeaerhyj
        if (!active)
        {
            return;
        } else
        {
            print("i am active");
        }

        //a ray to where the player is looking
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        print("looks at " + hit.collider.gameObject);
        //shows what is inside the beaker, only displays if the object is in the beakers list
        if (objectHeld == null && beakers.Contains(hit.collider.gameObject.transform.parent.gameObject))
        {
            display.text = "Beaker: " + hit.collider.gameObject.transform.parent.gameObject.name;
        } else
        {
            if (textTimer < 4)
            {
                display.text = textToDisplay;
            } else
            {
                display.text = "";
            }
            //to do later: end quest here and shit 5rgre is passion pit depressed
        }
        //picks up beaker
        if (objectHeld == null && Input.GetKeyDown(KeyCode.E) && beakers.Contains(hit.collider.gameObject.transform.parent.gameObject))
        {
            if (!used.Contains(objectHeld))
            {
                used.Add(objectHeld);
            }
            ChemistryMoveBack.putBack = false;
            inUse = ps[beakers.IndexOf(hit.collider.gameObject.transform.transform.parent.gameObject)];
            //picks up object if you hover cursor over it
            objectHeld = hit.collider.gameObject.transform.parent.gameObject;
            ps[beakers.IndexOf(objectHeld)].Pause();
            //ps = objectHeld.transform.GetChild(objectHeld.transform.childCount - 1).GetComponent<ParticleSystem>();
            Vector3 localPosition = objectHeld.transform.position;
            Quaternion localRotation = objectHeld.transform.rotation;
            Vector3 localScale = objectHeld.transform.localScale;
            objectHeld.transform.position = hand.transform.position;
            objectHeld.transform.parent = hand.transform;
            //objectHeld.transform.position = localPosition;
            //objectHeld.transform.rotation = localRotation;
            //objectHeld.transform.localScale = localScale;
            //reminder set this up fucking correctly next time .. idk howww
            objectHeld.transform.localScale = new Vector3(objectHeld.transform.localScale.x, objectHeld.transform.localScale.x, objectHeld.transform.localScale.x);
        }
        //this is how you pick up the fucking stew when you finish :)
        if (objectHeld == null && hit.collider.gameObject.transform.parent.gameObject == stew && Input.GetKeyDown(KeyCode.E))
        {
            print(beakers[0] + "," + beakers[3] + "," + beakers[4]);
            //testing purposes to see what is in the formula
            foreach (GameObject g in formula)
            {
                print(g.name);
            }
            //testing if formula is bugged
            //this will also display if you did it right or wrong or whatever
            //if has k n and c, quest complete, but first checks if you fucked up
            //havent decided how im going to punish the player for messing up ...
            //if you have h or o, you lose
            if (formula.Contains(beakers[1]) || formula.Contains(beakers[2]))
            {
                textTimer = 0;
                textToDisplay = "You messed up! Press q to restart";
                print("You messed up! Press q to restart!");
                mistake = true;
                //messed up the formula
            }
            else if (formula.Contains(beakers[0]) && formula.Contains(beakers[3]) && formula.Contains(beakers[4]))
            {
                //woo you did it correctly! BUT DOESNT WORK SOME REAOSN actually it works most of the time close enough.
                // i still dont know why the text doesnt display
                //if my calculations are correct, the stew will be a certain color if this is done correctly
                //if not the color will be wack
                textTimer = 0;
                textToDisplay = "Invisibility potion complete!";
                //whateverScript.hasInvisPosition = true;
                print("Invis potion complete");
                //active = false;
                //return;
                //return statement here breaks everything
            }
            else
            {
                //maybe play error sound or some shit. display error on screen?
                textTimer = 0;
                textToDisplay = "The formula is not ready yet!";
                print("The formula is not ready yet!");
            }
        }
        //resets after you messed up
        if (mistake && Input.GetKeyDown(KeyCode.Q))
        {
            objectHeld = null;
            pouring = false;
            ChemistryMoveBack.used = false;
            //transform.position = whatever the fuck; (startposition) ill be out of this place before you tell me where the night is
            stewColor.color = new Color(0.5f, 0.5f, 0.5f);
            doTime = false;
            timer2 = 0;
            timer = 0;
            foreach (ParticleSystem p in ps)
            {
                p.Play();
            }
            // Its a mistake by Men At Work is a beautiful song
            mistake = false;
        }
        if (objectHeld!=null)
        {
            if (Input.GetKeyDown(KeyCode.E) && hit.collider.gameObject.transform.parent.gameObject==stew && !pouring)
            {
                if (used.Contains(objectHeld))
                {
                    //maybe give a notice that the thing is empty, or I can send empty beakers to brazil
                    return;
                }
                //puts object in the right position and plays particles
                objectHeld.transform.parent = null;
                objectHeld.transform.localScale = new Vector3(1, 1, 1);
                objectHeld.transform.rotation = new Quaternion(0, 90, 0, 0);
                objectHeld.transform.position = stew.transform.position + new Vector3(0, 8, 7);
                timer = 0;
                formula.Add(objectHeld);
                doTime = true;
                pouring = true;
            }
            if (Input.GetKeyDown(KeyCode.Q) && !pouring)
            {
                ps[beakers.IndexOf(objectHeld)].Play();
                objectHeld.transform.parent = null;
                objectHeld.transform.localScale = new Vector3(1, 1, 1);
                objectHeld.transform.rotation = new Quaternion(0, 0, 0, 0);
                objectHeld = null;
            }
        }
        if (pouring)
        {
            //objectHeld.transform.RotateAround(transform.right, 5f);
            //pours the object
            //first goes a full 90. then when rotation lowers to 60, it stops. this will stop the thing at 120 degree and pour fucking shit out.
            //changes color of the stew based on what is being poured into the thing
            //maybe add bubbles to show that player fucked up
            float xRotat = objectHeld.transform.rotation.eulerAngles.x;
            if (!past90 || xRotat > 60)
            {
                objectHeld.transform.Rotate(2f, 0, 0);
            }
            //i know the fucking variable is called past90 and it goes past 60 but i dont give a fuck
            if (xRotat > 60)
            {
                past90 = true;
            }
            if (xRotat > 70)
            {
                inUse.Play();
            }
            //eventually put other stupdifucking shit about the stew changing color or something. mac demarco tour tickets
            //both of these are done!
        }
        if (doTime)
        {
            timer += Time.deltaTime;
        }
        // this god awful catastrophe changes the stew color i know i could use a switch case but oh well
        if (timer > 5 && timer < 8)
        {
            if (objectHeld == beakers[0])
            {
                stewColor.color -= new Color(0, 0, -0.01f);
            }
            else if (objectHeld == beakers[1])
            {
                stewColor.color -= new Color(0, 0, 0.01f);
            }
            else if (objectHeld == beakers[2])
            {
                stewColor.color -= new Color(0, 0.01f, 0);
            }
            else if (objectHeld == beakers[3])
            {
                stewColor.color -= new Color(0, -0.01f, 0);
            }
            else if (objectHeld == beakers[4])
            {
                stewColor.color -= new Color(0.01f, 0, 0);
            }
        }
        if (timer > 10)
        {
            pouring = false;
            float xRotat = objectHeld.transform.rotation.eulerAngles.x;
            //this is an easy way to prevent a dumbass bug. dont touch it!
            if (xRotat > 1)
            {
                objectHeld.transform.Rotate(-2f, 0, 0);
            }
            else
            {
                //i had to make a seperate script for this fucking shit because unity is dumb
                //nvm im an idiot but im too lazy to fix it so i dont give a fuck.
                ChemistryMoveBack.putBack = true;
            }
        }
        if (timer > 12)
        {
            inUse.Stop();
        }
    }

    public static void StartQuest()
    {
        active = true;
        Cursor.visible = true;
    }
    public static void PauseQuest()
    {
        active = false;
        Cursor.visible = false;
    }

    void CompleteQuest(){
        active = false;
        QuestSystem.questActive = false;
        QuestSystem.questStarted = false;
        Player.checkpoint = GameObject.Find("chemistryTrigger").transform.position;
    }
}
