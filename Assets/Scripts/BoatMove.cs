using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public GameObject player, playerParent;
    public static bool moving = false;
    float timer = 0;
    float bob = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timer += Time.deltaTime;
            GetComponent<Rigidbody>().velocity = new Vector3(0, bob, 1f);
            if (timer > 0.05f)
            {
                bob *= -1f;
                timer = 0;
            }
            //player.GetComponent<Rigidbody>().velocity += new Vector3(0, 0, 1);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!moving && collision.collider.gameObject == player)
        {
            moving = true;
            playerParent.transform.parent = gameObject.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {

    }
}
