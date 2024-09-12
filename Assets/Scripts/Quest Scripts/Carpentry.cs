using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carpentry : MonoBehaviour
{
    Vector3 mousepos;
    int counter = 0;
    RectTransform rt;
    public static Image image;
    public static bool active = false;
    Animator anim;
    bool play = false;
    float t;
    
    // Start is called before the first frame update

    /*
     * Very important game ideas: Find 5 screws to fix the later. Right screw to find is screw large.
     */

    void Start()
    {
           rt = GetComponent<RectTransform>();
           image = GetComponent<Image>();
           anim = GameObject.Find("ladder001").GetComponent<Animator>();
           anim.SetBool("carpentryQuest", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!active){
            return;
        }

        if(play){
            anim.SetBool("carpentryQuest", true);
            t += Time.deltaTime;
            if(t > 2.0f){
                play = false;
                anim.SetBool("carpentryQuest", false);
                t = 0.0f;
            }
        }
        
        //check if building animation is complete
        if(counter == 15){
            QuestSystem.CompleteQuest();
            active = false;
            Player.checkpoint = GameObject.Find("carpentryTrigger").transform.position;
        }
        
        if(Input.GetMouseButtonDown(0)){
            Vector2 mouse = Input.mousePosition;
            mousepos.x = mouse.x;
            mousepos.y = mouse.y;
            mousepos.z = 0;
            Debug.Log(mousepos);
            Debug.Log(rt.anchoredPosition);
        }

        if((Mathf.Abs(mousepos.x-rt.anchoredPosition.x)<80.0f)&&(Mathf.Abs(mousepos.y-rt.anchoredPosition.y)<80.0f)){
            Debug.Log(counter);
            counter++;
            int x = Random.Range(0, Screen.width);
            int y = Random.Range(0, Screen.height);
            rt.anchoredPosition = new Vector3(x, y, 0);
            //play through a couple frames of the ladder building animation
            play = true;
        }

    }

    public static void StartQuest(){
        active = true;
        Cursor.visible = true;
        image.enabled = true;

    }

    public static void PauseQuest(){
        active = false;
        Cursor.visible = false;
        image.enabled = false;
    }
}
