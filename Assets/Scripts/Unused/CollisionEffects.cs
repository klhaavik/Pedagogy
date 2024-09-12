using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionEffects : MonoBehaviour
{
    //this for hitting coins and dying to enemies
    //this script goes on the player
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Coin"))
        {
            score++;
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
