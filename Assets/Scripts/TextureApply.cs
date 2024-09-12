using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TextureApply : MonoBehaviour
{
    public Material baseMaterial;
    public List<Texture> textureList = new List<Texture>(1);
    public List<Texture> normalMaps = new List<Texture>();
    Material texturizer;
    Shader shader;
    float xChange, yChange;
    MeshRenderer rend;
    /*
     * interior wall ratio should be 1/5 on x and 1/6 on z; maybe exterior and blue should be the same
     * still figuring out floor, heres my progress:
        * if y is less, multiply it for x by 3/2. if x is less, divide it for y by 3/2
        * ratio between texture x and y is equivalent to object x and y's ratio
        * this is a special object and will have its own way of tiling
        * i have no idea how this works, don't mess with the code, and dont ask me about it because i know as much as you do
     * remember to switch x and z because thing is dumb
     * for offset, 0.6 on x is 1 global unit, if that makes sense. most differences are by this tiny 0.6 thing or a multiple of it
     * misalignments occur when zscale + yposition is not even, must be changed by 0.6?
    */
    // Start is called before the first frame update
    void Start()
    {
        texturizer = new Material(Shader.Find("HDRP/Lit"));
        rend = GetComponent<MeshRenderer>();
        Material[] mats = rend.materials;
        for (int i=0; i<mats.Length; i++)
        {
            mats[i] = texturizer;
        }
        rend.materials = mats;
        texturizer.EnableKeyword("_NORMALMAP");        
        texturizer.EnableKeyword("_NORMALMAP_TANGENT_SPACE");

        if (name.Contains("Window"))
        {
            return;
        } else if (name.Contains("Roof"))
        {
            texturizer.SetTexture("_BaseColorMap", textureList[4]);
            texturizer.mainTextureScale = new Vector2(100f, 100f);
            return;
        } else if (name.Contains("Interior") && !name.Contains("Stand"))
        {
            xChange = 5.0f / 1.5f;
            yChange = 6.0f / 1.5f;
            texturizer.SetTexture("_BaseColorMap", textureList[0]);
            texturizer.SetTexture("_NormalMap", normalMaps[0]);
        } else if (name.Contains("Xterior"))
        {
            xChange = 5.0f / 3.0f;
            yChange = 6.0f / 3.0f;
            texturizer.SetTexture("_BaseColorMap", textureList[1]);
            texturizer.SetTexture("_NormalMap", normalMaps[1]);
        } else if ((name.Contains("Floor") || name.Contains("Stand")) && !name.Contains("Hallway.00"))
        {
            texturizer.SetTexture("_BaseColorMap", textureList[3]);
            texturizer.SetTexture("_NormalMap", normalMaps[2]);
            if (transform.localScale.y / transform.localScale.x > 0.3f && transform.localScale.y / transform.localScale.x < 1.3f)
            {
                xChange = (2.0f / 3.0f);
                texturizer.SetTextureScale("_BaseColorMap", new Vector2(transform.localScale.y / xChange, (transform.localScale.y / xChange) * (transform.localScale.y / transform.localScale.x)));
            } else
            {
                xChange = (2.3f / 3.0f);
                texturizer.SetTextureScale("_BaseColorMap", new Vector2(transform.localScale.x * xChange, (transform.localScale.x * xChange) * (transform.localScale.y / transform.localScale.x)));
            }
            return;
        } else if (name.Contains("Stair")||name.Contains("Case"))
        {
            //print("here");
            xChange = 1;
            yChange = 1;
            texturizer.SetTexture("_BaseColorMap", textureList[4]);
            texturizer.SetTexture("_NormalMap", normalMaps[1]);
        }
        else
        {
            xChange = 40.0f / 18.0f;
            yChange = 2;
            if (transform.localScale.y > transform.localScale.x)
            {
                //xChange = 1/xChange;
                //yChange = 2;
                texturizer.SetTexture("_BaseColorMap", textureList[2]);
            }
            else
            {
                texturizer.SetTexture("_BaseColorMap", textureList[5]);
            }
            //return;
        }

        texturizer.SetFloat("_NormalMapSpace", 0.0f);
        texturizer.SetFloat("_NormalScale", 1.0f);

        if (transform.localScale.z==1)
        {
            texturizer.SetTextureScale("_BaseColorMap", new Vector2(transform.localScale.x / xChange, transform.localScale.y / yChange));
        } else if (transform.localScale.y < transform.localScale.x)
        {
            texturizer.SetTextureScale("_BaseColorMap", new Vector2(transform.localScale.z / xChange, transform.localScale.x / yChange));
        } else
        {
            texturizer.SetTextureScale("_BaseColorMap", new Vector2(transform.localScale.z / xChange, transform.localScale.y / yChange));
        }
        if (transform.position.y + transform.localScale.z % 2 != 0)
        {
            //offset is whack, nothing works :(
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
