using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject blood;
    public GameObject p;
    GameObject x;
    private StudentSpawn s;
    public GameObject person;
    [SerializeField]private int numPeople;
    bool startFade = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numPeople; i++){
            x = Instantiate(person, p.transform.position, Quaternion.identity);
           x.transform.parent = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startFade){
            RenderSettings.ambientIntensity = 0f;
            if(RenderSettings.ambientIntensity<=0){
                startFade = false;
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Player")){
            //start game
            Debug.Log("game started");
            Debug.Log("everything goes dark");
            Debug.Log("player will investigate and find blood on walls");
            GetComponent<BoxCollider>().enabled = false;
            blood.SetActive(true);
            GameObject.Find("Monster").GetComponent<MeshRenderer>().enabled = true;
            Delete();
            startFade = true;
        }
    }

    public void Delete(){
        for(var i = 0; i < numPeople; i++){
            Destroy(p.transform.GetChild(i));
        }
    }

}
