using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Student : MonoBehaviour
{
	
	BinaryReader reader;
	List<Vector3> waypoints;

    int start;
    int end;
	
    float timer;
    float high;

    public float speed;
    float x;

    Vector3 startPos;
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        float x, y, z;
        long streamEnd;
		waypoints = new List<Vector3>();
        reader = new BinaryReader(File.OpenRead(".\\Assets\\Scripts\\waypoints.bin"));
        streamEnd = reader.BaseStream.Length;
		while(reader.BaseStream.Position != streamEnd) {
            x = reader.ReadSingle(); // ensure proper order
            y = reader.ReadSingle();
            z = reader.ReadSingle();
			waypoints.Add(new Vector3(x, y, z));
			 // reader.ReadSingle() a 4 byte float
		}

        end = Random.Range(0,waypoints.Count);
		endPos = waypoints[end];
        timer = 0.0f;

        x = Random.Range(0.03f,1.0f);
        speed = (1.0f / x) + 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= (Time.deltaTime * speed);
        if(timer <= 0.0f) {
            start = end;
            if(Random.Range(0,2) == 0) {
                end++;
            }
            else {
                end--;
            }

            end = end % waypoints.Count;
            if(end < 0) end = waypoints.Count - 1;

            startPos = endPos;
            endPos = waypoints[end] + new Vector3(Random.Range(-2.0f,2.0f),0.0f,Random.Range(-2.0f,2.0f));

            timer = Vector3.Distance(startPos, endPos);
            high = timer;
			
			transform.LookAt(endPos);
        }
        
        if(0.0f < (timer / high) && (timer / high) < 1.0f) {
            transform.position = Vector3.Lerp(endPos, startPos, timer / high);
        }
        // timer is decreasing so it moves from start to end
    }
	
	void OnUseItem(GameObject player)
	{
		transform.position = player.transform.position;
		AudioSource aud;
		aud = GetComponent<AudioSource>();
		if(aud.clip != null) {
			aud.Play();
			// "Please put me down!!!"
		}
	}
}
