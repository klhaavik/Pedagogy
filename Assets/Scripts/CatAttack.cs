using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttack : MonoBehaviour
{
    int jumped = 0;
    GameObject pigeon;
    float timer = 0;
    bool playTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        pigeon = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, pigeon.transform.position) < 3f && GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position,pigeon.transform.position,0.1f);
        }
        else if (Vector3.Distance(gameObject.transform.position, pigeon.transform.position) < 6f && jumped < 5)
        {
            GetComponent<Rigidbody>().AddForce(15 * (pigeon.transform.position - transform.position + new Vector3(0,3f,0)));
            jumped++;
        }
        if (jumped>=5)
        {
            playTimer = true;
        }
        if (playTimer)
        {
            timer += Time.deltaTime;
        }
        if (timer > 3f)
        {
            jumped = 0;
            timer = 0;
            playTimer = false;
        }
    }
}
