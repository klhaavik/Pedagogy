using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    float timer = 0;
    public List<GameObject> objs = new List<GameObject>();
    public GameObject again, quit;
    public static bool won = false;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (won)
        {
            timer += Time.deltaTime;
            if (/*(objs[index].GetComponent<Image>() && objs[index].GetComponent<Image>().color.a < 1 ) || (objs[index].GetComponent<Text>() &&*/ objs[index].GetComponent<Text>().color.a < 1/*)*/)
            {
                /*if (objs[index].GetComponent<Image>())
                {
                    objs[index].GetComponent<Image>().color += new Color(0, 0, 0, 0.05f);
                } else if (objs[index].GetComponent<Text>())
                {*/
                    objs[index].GetComponent<Text>().color += new Color(0, 0, 0, 0.03f);
                //}
            }
            else if (index < objs.Count - 1)
            {
                index++;
            }
            else
            {
                again.SetActive(true);
                quit.SetActive(true);
                won = false;
            }
        }
    }
}
