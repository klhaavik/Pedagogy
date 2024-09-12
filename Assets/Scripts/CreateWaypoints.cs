using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateWaypoints : MonoBehaviour
{
  // Start is called before the first frame update
	
	// THIS SCRIPT SHOULD BE DISABELED BY DEFAULT:
	// It's merely for development
	
	BinaryWriter writer;

    void Start()
    {
	    writer = new BinaryWriter(File.OpenWrite(".\\waypoints.txt"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("y")) {
            // create waypoint
			writer.Write(transform.position.x);
			writer.Write(transform.position.y);
			writer.Write(transform.position.z);
        }
		
		if(Input.GetKeyDown("u")) {
            // close file
			writer.Flush();
			writer.Dispose();
        }
    }


}
