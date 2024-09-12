using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyWalls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CapsuleCollider>().transform.localScale = new Vector3(GetComponent<CapsuleCollider>().transform.localScale.x/1.5f, GetComponent<CapsuleCollider>().transform.localScale.y, GetComponent<CapsuleCollider>().transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
