using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartWipe : MonoBehaviour
{
    public float duration = 1f;
    public float xSize = 11520;
    public float ySize = 6480;
    RectTransform rt;
    //AudioScript scriptInstance = null;
    int counter = 0;
    // TutorialManager tm;

    void Start()
    {
        // tm = GameObject.Find("Canvas").GetComponent<TutorialManager>();
        
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = Vector2.zero;
        GameObject tempObj = GameObject.Find("Heart");
 //       scriptInstance = tempObj.GetComponent<AudioScript>();
        FadeIn();
    }

    public void FadeOut(){
        StartCoroutine(Fade(new Vector2(xSize, ySize), Vector2.zero));
 //       scriptInstance.ResetMusic();
 //       GetComponent<AudioScript>().ResetMusic();
    }

    public void FadeIn(){
        StartCoroutine(Fade(Vector2.zero, new Vector2(xSize, ySize)));
    }

    private IEnumerator Fade(Vector2 startingSize, Vector2 endingSize){
        rt.sizeDelta = startingSize;

        float t = 0f;
        while(t < 1f){
            rt.sizeDelta = Vector2.Lerp(startingSize, endingSize, t);
            t += Time.deltaTime / duration;
            yield return null;
        }
        rt.sizeDelta = endingSize;
        
        /*if(endingSize == Vector2.zero){
            SceneManager.LoadScene("MainGame");
        }*/
        
        /*if(!tm.TutorialHasStarted()){
            tm.StartTutorial();
            // print("starting tutorial");
        }*/
    }
}
