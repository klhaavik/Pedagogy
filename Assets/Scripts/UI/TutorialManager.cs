using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    string[] tutorialTexts = {
        "Press D/Right Arrow to move right",
        "Press A/Left Arrow to move left",
        "Move right",
        "Press Space/Up Arrow to Jump",
        ""
    };
    int index = 0;
    public bool testingDisable;
    Text tutorialTxt;
    public float fadeTime;
    public Color textColor;
    // Movement3D movement;
    KaiMovement movement;
    public GameObject moveLeft, moveRight, jump;
    bool hasStarted = false;
    bool hasMoved = false;
    
    // Start is called before the first frame update
    void Start()
    { 
        tutorialTxt = GameObject.Find("Tutorial Text").GetComponent<Text>();
        tutorialTxt.text = tutorialTexts[index];

        // movement = GameObject.Find("Player").GetComponent<Movement3D>();
        movement = GameObject.Find("Player").GetComponent<KaiMovement>();
        if(testingDisable){
            movement.FullMovement();
        }else{
            movement.NoMovement();
        }
    }

    public void StartTutorial(){
        if(testingDisable) return;
        // StartCoroutine(FadeImage(0, 1, fadeTime));
        movement.OnlyMoveRight();
        moveRight.SetActive(true);
        hasStarted = true;
    }

    public bool TutorialHasStarted(){
        return hasStarted;
    }

    public void UpdateTutorial(){
        SetNextTutorialValues();
    }

    void Update(){
        if(movement.GetKeyRight() && !hasMoved){
            movement.FullMovement();
            moveLeft.SetActive(true);
            jump.SetActive(true);
            hasMoved = true;
        }
    }

    void SetNextTutorialValues(){
        /*StartCoroutine(FadeImage(1, 0, fadeTime));
        switch(index){
            case 0:
                movement.FullMovement();
                movement.SetJumpEnabled(false);
                break;
            case 1:
                break;
            case 2:
                movement.SetJumpEnabled(true);
                break;
            case 3:
                break;
            default:
                break;
        }

        float t = 0f;
        while(t < 1f){
            t += Time.deltaTime / fadeTime;
            yield return null;
        }*/

        index++;
        switch(index){
            case 1:
                moveLeft.SetActive(false);
                moveRight.SetActive(false);
                jump.SetActive(false);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
        // tutorialTxt.text = tutorialTexts[index];

        // StartCoroutine(FadeImage(0, 1, fadeTime));
    }

    private IEnumerator FadeImage(GameObject image, float startAlpha, float endAlpha, float fadeTime){
        Color color = textColor;
        color.a = startAlpha;
        // tutorialTxt.color = color;

        float t = 0f;
        while(t < 1f){
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            tutorialTxt.color = color;
            t += Time.deltaTime / fadeTime;
            yield return null;
        }

        color.a = endAlpha;
        tutorialTxt.color = color;
    }
}
