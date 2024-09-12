using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterUpdated : MonoBehaviour
{
    GameObject hallway = null;
    float cooldown = 0;
    float jumpscareTimer = 0;
    bool scareActive = false;
    int scare;
    bool jumpActive = false;
    bool followActive = false;
    public static bool active;
    //NOTICE FOR LATER: figure out if monster is a duplicate or if monster used here is real
    public GameObject monster;
    float speed = 10;
    Vector3 monstPos = new Vector3();
    Vector3 startPos = new Vector3();
    float followTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*check if player is walking down a hallway
		 * if true, one of two things can happen:
		 *	harmless jumpscare, almost like walking into monster
		 *	monster comes into vision at end of hallway and moves towards player and if player turns around, die.
		 *		must confront and realize its not real
		 *	combinations of both? monster comes at end of hallway, all chill. then turn around, monster comes from other direction.
		 *		people might panic.
		 * some audio, do later most likely
		 * youre walking and something happens to the left of you. pass a room and someything happens in the room.
		 * if you go look, (hard to put into words but maybe easy code) two possibilites:
		 *	something pops up in window, most likely not, instead other thing
		 *		in certain windows, there are faces that are visible in corners of vision, if looked at, they go away
		 *	go in to investigate, door closes
		*/

        cooldown += Time.deltaTime;
        //scare is active
        if (scareActive)
        {
            //when cooldown is over, initiate the chosen scare
            if (cooldown > 5)
            {
                startPos = transform.position;
                //may add combo scare later
                if (scare == 0)
                {
                    BeginJumpscare();
                } else
                {
                    FollowScare();
                }
                cooldown = 0;
                scareActive = false;
            }
        }

        //jumpscare assisting code here:
        if (jumpActive)
        {
            jumpscareTimer += Time.deltaTime;
        }
        if (jumpscareTimer > 1f)
        {
            //sends monster away, either do this by sending it to brazil or destroying it. will decide later
            monster.transform.position = new Vector3(1000, 1000, 1000);
            jumpscareTimer = 0;
            jumpActive = false;
            scareActive = false;
            cooldown = 0;
        }

        //followscare assisting code here:
        if (followActive)
        {
            //moves the monster towards the player, experiment with speed
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, speed * Time.deltaTime);
            //fix this later, its supposed to keep monster from floating
            //monster.transform.position = new Vector3(transform.position.x, );
            //fix this later too, rotation is all wack
            monster.transform.LookAt(transform);
            //this is the turning around code
            if (Vector3.Distance(transform.position, monstPos) > Vector3.Distance(startPos, monstPos))
            {
                //print(followTimer);
                followTimer += Time.deltaTime;
            } else
            {
                followTimer = 0;
            }
            if (followTimer > 3)
            {
                followTimer = 0;
                followActive = false;
                BeginJumpscare();
                //you die!
            }
        }
    }

    void BeginJumpscare()
    {
        if (!jumpActive)
        {
            //puts monster on player with sufficient offset
            //for this offset i would suggest putting the offset based on the forward of the player so the monster is in front of the player no matter where they are standing
            //maybe play scary sound
            monster.transform.position = transform.position + JumpscarePosition();
            monster.transform.LookAt(transform);
            jumpscareTimer = 0;
            //activates the timer for this
            jumpActive = true;
        }
    }

    void FollowScare()
    {
        if (!followActive)
        {
            //moves monster to the end of the hall
            monster.transform.position = CalculateMonstPos();
            followActive = true;
        }
    }

    Vector3 JumpscarePosition()
    {
        Vector3 newPos = Camera.main.transform.forward * 5;
        return newPos;
    }

    //calculates the position of the monster at the end of the hall, used by followscare only at the moment
    Vector3 CalculateMonstPos()
    {
        //plan for this: use transform.forward and hallway transform to calculate new position
        Vector3 newPos = new Vector3();
        //this multiplier will be used to determine which end of the hallway the monster goes
        int multiplier = 0;
        //checking if hallway is longer on x or z (which is y because blender object uses y)
        //its so weird because z scaling is y, but z position is still z
        if (hallway.transform.localScale.x > hallway.transform.localScale.y)
        {
            if (Camera.main.transform.forward.x > 0)
            {
                multiplier = 1;
            }
            else
            {
                multiplier = -1;
            }
            //adds the length of the hall to send it to the end of the hall, idk why it needs/1.1f
            newPos = hallway.transform.position + (new Vector3(hallway.transform.localScale.x/1.1f, 0, 0) * multiplier);
        } else
        {
            if (Camera.main.transform.forward.z > 0)
            {
                multiplier = 1;
            } else
            {
                multiplier = -1;
            }
            newPos = hallway.transform.position + (new Vector3(0, 0, hallway.transform.localScale.y/1.1f) * multiplier);
        }
        //places monster above the floor so it is not stuck in or below the floor
        newPos += new Vector3(0, 1, 0);
        monstPos = newPos;
        return newPos;
    }

    void OnCollisionEnter(Collision collision)
    {
        //are you in a hallway and are you available to scare? did it this way for testing but still works
        bool hallCollison = collision.collider.gameObject.name.Contains("Hall");
        if (hallCollison && !scareActive)
        {
            //assigns hallway correctly, chooses a scare, and begins it
            hallway = collision.collider.gameObject;
            scare = Random.Range(0, 2);
            scareActive = true;
            cooldown = 0;
        }
        //if you confront the monster, you will discover that it is not real
        if (collision.collider.gameObject == monster && followActive)
        {
            //monster disspears, maybe add animation for this
            monster.transform.position = new Vector3(1000, 1000, 1000);
            followActive = false;
            scareActive = false;
            cooldown = 0;
        }
    }
}
