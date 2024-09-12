using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Carpentry()
    {
        EventsScript.staticText.text = QuestSystem.objectives[2];
        AppearScript.appear = false;
        FadeScript.fade = true;
        Cursor.visible = false;
        EventsScript.index++;
    }
    public void Chemistry()
    {
        EventsScript.staticText.text = QuestSystem.objectives[3];
        AppearScript.appear = false;
        FadeScript.fade = true;
        Cursor.visible = false;
        EventsScript.index++;
    }
}
