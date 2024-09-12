using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDisplay : MonoBehaviour
{
    public RectTransform moveLeft;
    public RectTransform moveRight;
    public RectTransform jump;
    public float fadeTime;
    public Color color;
    KaiMovement movement;
    public bool testingDisable;
    bool hasStarted = false;
    bool hasMoved = false;
    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.Find("Player").GetComponent<KaiMovement>();
        if(!testingDisable) {
            movement.NoMovement();
            StartCoroutine(StartTutorialAfterDelay(1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.GetKeyRight() && !hasMoved && hasStarted && !testingDisable){
            movement.FullMovement();
            StartCoroutine(FadeUIElement(moveLeft, 0, 1, fadeTime, color));
            StartCoroutine(FadeUIElement(jump, 0, 1, fadeTime, color));
            hasMoved = true;
        }
    }

    private IEnumerator StartTutorialAfterDelay(float delay){
        float t = 0;
        while(t < delay){
            t += Time.deltaTime;
            yield return null;
        }

        print("here");
        movement.OnlyMoveRight();
        StartCoroutine(FadeUIElement(moveRight, 0, 1, fadeTime, color));
        hasStarted = true;
    }

    void OnTriggerEnter(){
        if(testingDisable || !hasMoved) return;
        StartCoroutine(FadeUIElement(moveLeft, 0, 1, fadeTime, color));
        StartCoroutine(FadeUIElement(moveRight, 0, 1, fadeTime, color));
        StartCoroutine(FadeUIElement(jump, 0, 1, fadeTime, color));
    }

    void OnTriggerExit(){
        if(testingDisable || !hasMoved) return;
        StartCoroutine(FadeUIElement(moveLeft, 1, 0, fadeTime, color));
        StartCoroutine(FadeUIElement(moveRight, 1, 0, fadeTime, color));
        StartCoroutine(FadeUIElement(jump, 1, 0, fadeTime, color));
    }

    private IEnumerator FadeUIElement(Transform obj, float startAlpha, float endAlpha, float fadeTime, Color fadeColor){
        print("fading element");
        Image key = obj.GetChild(0).GetComponent<Image>();
        print(key);
        Image arrow = obj.GetChild(1).GetComponent<Image>();
        print(arrow);
        Text txt = obj.GetChild(2).GetComponent<Text>();
        print(txt);

        Color color = fadeColor;
        color.a = startAlpha;

        float t = 0;

        while(t < 1f){
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            key.color = color;
            arrow.color = color;
            txt.color = color;
            t += Time.deltaTime / fadeTime;
            yield return null;
        }

        color.a = endAlpha;
        key.color = color;
        arrow.color = color;
        txt.color = color;
    }
}
