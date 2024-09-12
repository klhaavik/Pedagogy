using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTriggers : MonoBehaviour
{
    public string roomName = "bad";
    public static bool available = false;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            print("does " + roomName);
            EventsScript.BeginEvent(roomName);
            //QuestSystem.ShowQuestActive(roomName);
        }
    }
}
