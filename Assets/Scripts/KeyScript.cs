using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    public static bool foundPlace = false;
    public GameObject model;
    public static GameObject staticModel;
    public GameObject player;
    public static GameObject staticPlayer;
    public Text objText;
    public static Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = Monster.MakePatrolPoint();
        staticModel = model;
        staticPlayer = player;
        /*if (position == new Vector3(0, 0, 0))
        {
            position = SetRanPos(gameObject, model);
        } else
        {
            transform.position = position;
        }*/
        position = transform.position;
        print("repurpose: " + position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(1, 0, 0);
        /*if (transform.position == new Vector3(-50,10,10))
        {
            staticModel = model;
            staticPlayer = player;
            //print("apparentname: " + model.name);
            position = SetRanPos(gameObject, model);
            //print("this is doing");
            //print("repos2: " + position);
        }*/
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == player)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            objText.text = "Leave through the front doors!";
            UseDoors.openable = true;
            UseDoors.overwrite = true;
        }
    }

    public static Vector3 SetRanPos(GameObject setObject, GameObject model)
    {
        while (!foundPlace)
        {
            int objIndex = Random.Range(0, model.transform.childCount);
            //print("object: " + objIndex + " name: " + model.transform.GetChild(objIndex).name);
            if (model.transform.GetChild(objIndex).gameObject.activeSelf && model.transform.GetChild(objIndex).name.Contains("Floor") && !model.transform.GetChild(objIndex).name.Contains("Xterior") && !model.transform.GetChild(objIndex).name.Contains("Hallway.00"))
            {
                setObject.transform.position = model.transform.GetChild(objIndex).transform.position + new Vector3(0.5f * Random.Range(-1f, 1f) * model.transform.GetChild(objIndex).transform.localScale.x, 8f, 0.5f * Random.Range(-1f, 1f) * model.transform.GetChild(objIndex).transform.localScale.y);
                foundPlace = true;
            }
        }
        return setObject.transform.position;
    }
}
