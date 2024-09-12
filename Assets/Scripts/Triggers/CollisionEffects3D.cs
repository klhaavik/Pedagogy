using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionEffects3D : MonoBehaviour
{
    //this for hitting coins and dying to enemies
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        GameObject g = col.gameObject;
        if (g.CompareTag("Coin"))
        {
            score++;
            Destroy(g);
        }else if (g.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("NewScene");
        }
    }
}
