using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseLadder : MonoBehaviour
{
    //place where ladder should go
    Transform desiredPos;
    public GameObject guide;
    public static bool onLadder;
    bool ladAvail = false;
    public GameObject player;
    float velocity = 5;
    // Start is called before the first frame update
    void Start()
    {
        desiredPos = guide.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ladAvail)
        {

        }
    }

    public void OnUseItem(GameObject obj)
    {
        print("terabtye " + Vector3.Distance(obj.transform.position, desiredPos.position));
        if (Vector3.Distance(obj.transform.position, desiredPos.position) < 30)
        {
            ladAvail = true;
            //bring it out of inventory
            //GetComponent<MeshRenderer>().enabled = true;
            //GetComponent<BoxCollider>().enabled = true;
            transform.position = desiredPos.position;
            transform.rotation = desiredPos.rotation;
            //obj.transform.localScale = desiredPos.localScale;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == player)
        {
            player.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject == player)
        {
            player.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
