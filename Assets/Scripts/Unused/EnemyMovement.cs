using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //y position does not change, cats cannot fly
    //x position moves toward the pigeon's x position
    GameObject pigeon;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pigeon = GameObject.Find("Pigeon");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, pigeon.transform.position) < 10f)
        {
            rb.velocity = new Vector2(pigeon.transform.position.x - transform.position.x, 0);
        }
    }
}
