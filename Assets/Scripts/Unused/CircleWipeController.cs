using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleWipeController : MonoBehaviour
{
    public Shader shader;
    private Material mat;
    [Range(0, 1.2f)] public float radius = 0f;
    public float horizontal = 16;
    public float vertical = 9;
    float radiusSpeed;
    public float duration = 1f;
    public Color fadeColor;
    public Texture maskTexture;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        radius = 0f;
        mat = new Material(shader);
        mat.SetFloat("_Vertical", vertical);
        mat.SetFloat("_Horizontal", horizontal);
        radiusSpeed = Mathf.Max(horizontal, vertical);
        mat.SetTexture("_MaskTexture", maskTexture);
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination){
        Graphics.Blit(source, destination, mat);
    }

    public void FadeOut(){
        StartCoroutine(Fade(1.2f,  0f));
    }

    public void FadeIn(){
        StartCoroutine(Fade(0f,  1.2f));
    }

    private IEnumerator Fade(float start, float end){
        radius = start;
        UpdateShader();
        float t = 0f;
        while(t < 1f){
            radius = Mathf.Lerp(start, end, t);
            t += Time.deltaTime / duration;
            UpdateShader();
            yield return null;
        }
        radius = end;
        UpdateShader();
        if(end == 0f) SceneManager.LoadScene("DemoScene");
    }

    void UpdateShader(){
        mat.SetFloat("_Radius", radius);
        mat.SetFloat("_RadiusSpeed", radiusSpeed);
        mat.SetColor("_FadeColor", fadeColor);
    }
}
