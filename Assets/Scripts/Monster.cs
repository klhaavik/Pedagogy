using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // Start is called before the first frame update
	
	private UnityEngine.AI.NavMeshAgent pathfinder;
	GameObject player;
	bool patrol;
	float chase;
	Vector3 patrolPoint;
	public Vector3 debugDest, debugPos;
	private int sightings = 0;
	public static bool caught = false;
	public Canvas ui;
	public Canvas title;
	public GameObject key;
	public Text objText;
	float timer = 0.0f;
	bool foundPlace = false;
	public GameObject model;
	Vector3 tempDestination;
	public GameObject jumpscare;
	float animTimer = 0;
	public static bool died = false;

	// private bool encounter;

    void Start()
    {
		chase = 0.0f;
        pathfinder = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player");
		died = false;
		/*do {
			// repeating until it has a destination
			patrolPoint = MakePatrolPoint();
		} while(!pathfinder.SetDestination(patrolPoint));
		transform.position = patrolPoint;*/

		//pathfinder.transform.forward = new Vector3(0,1,0);
	}
	
	public static Vector3 MakePatrolPoint()
	{
		Vector3 rp;
		RaycastHit hit;
		while(true) {
			rp = new Vector3(Random.Range(-700.0f,700.0f),Random.Range(8.0f,175.0f),Random.Range(-500.0f,200.0f));
			if(Physics.Raycast(rp,Vector3.down, out hit, 100.0f)) return hit.point;
		}
	}

    // Update is called once per frame
    void Update()
    {

		//Kai: do the animation thing

		RaycastHit hit;
		Vector3 rayOrg;
		rayOrg = transform.position + (transform.forward * transform.localScale.x * 1.2f);
		if (!caught) {
			return;
		}
		/*if(Physics.Raycast(rayOrg,player.transform.position - rayOrg,out hit,35.0f) && hit.collider.CompareTag("Player")) {
			if(chase < 0.0f) {
				chase = 0.0f;
			}
			chase += Time.deltaTime*3.0f;
			if(chase > 20.0f) {
				chase = 20.0f;
			}
		}
		chase -= Time.deltaTime;*/
		//if(player && chase > 0.0f) {
			//print("Chase Go to = " + player.transform.position);
		//pathfinder.transform.forward = transform.position - player.transform.position;
		//print("distance: " + Vector3.Distance(transform.position, player.transform.position));
		if (Vector3.Distance(transform.position, player.transform.position) < 9f) 
        {
			Player.movementEnabled = false;
			player.GetComponent<Rigidbody>().isKinematic = true;
			//player.transform.rotation = new Quaternion(0, -90, 0, player.transform.rotation.w);
			player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 180, 0);
			//player.transform.LookAt(jumpscare.transform.GetChild(0).transform);
			player.transform.position = jumpscare.transform.position - new Vector3(0,6f,-4.05f);
			jumpscare.transform.GetChild(0).GetComponent<Animator>().SetBool("MeshEnabled", true);
			jumpscare.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			jumpscare.transform.GetChild(0).GetComponent<Animator>().SetBool("scareActive", true);
			jumpscare.transform.GetChild(0).GetComponent<AudioSource>().Play();
			died = true;
			/*
			key.GetComponent<MeshRenderer>().enabled = true;
			key.GetComponent<Collider>().enabled = true;
			KeyScript.SetRanPos(key);
			UseDoors.openable = false;
			objText.text = " ";
			FadeScript.fade = false;
			ui.enabled = false;
			title.enabled = true;
			caught = false;
			FadeScript.fade = false;
			SceneManager.LoadScene("SampleScene");
			*/
		}
		if (died)
        {
			animTimer += Time.deltaTime;
        }
		if (animTimer > 2.2f)
        {
			key.GetComponent<MeshRenderer>().enabled = true;
			key.GetComponent<Collider>().enabled = true;
			KeyScript.foundPlace = false;
			//KeyScript.SetRanPos(key);
			UseDoors.openable = false;
			objText.text = "Press C to see controls";
			FadeScript.fade = false;
			title.enabled = false;
			ui.enabled = true;
			caught = false;
			died = false;
			Player.movementEnabled = true;
			SceneManager.LoadScene("SampleScene");
		}
		pathfinder.transform.forward = -(player.transform.position - transform.position);
		pathfinder.transform.rotation = new Quaternion(0, pathfinder.transform.rotation.y, pathfinder.transform.rotation.z, pathfinder.transform.rotation.w);
		if (!foundPlace)
		{
			//pathfinder.SetDestination(player.transform.position);
			NavMeshHit closestPoint;
			//DONT COMMENT THIS PRINT STATEMENT
			print("foundation: " + NavMesh.SamplePosition(transform.position, out closestPoint, 1.0f, NavMesh.AllAreas) + " destination: " + NavMesh.SamplePosition(player.transform.position, out closestPoint, 30f, NavMesh.AllAreas));
			pathfinder.SetDestination(closestPoint.position);
		}
		if (foundPlace)
        {
			//pathfinder.SetDestination(tempDestination);
			NavMeshHit hitt;
			NavMesh.SamplePosition(tempDestination, out hitt, 30f, NavMesh.AllAreas);
			pathfinder.SetDestination(hitt.position);
			//print("lmao: " + pathfinder.pathStatus + " lmfao: " + Vector3.Distance(tempDestination, transform.position) + " lmbfao: " + hitt.position);
			if (Vector3.Distance(tempDestination, transform.position) < 60f)
            {
				foundPlace = false;
            }
        }
		
		/*
		print("status: " + NavMeshPathStatus.PathInvalid);
		print("point: " + closestPoint.position);
		NavMeshHit closestPoint2;
		print("newfoundation: " + NavMesh.SamplePosition(transform.position, out closestPoint2, 1.0f, NavMesh.AllAreas) + " newdestination: " + NavMesh.SamplePosition(closestPoint.position, out closestPoint2, 30f, NavMesh.AllAreas));
		print("canwork: " + pathfinder.CalculatePath(closestPoint.position, new NavMeshPath()));
		print("canwork2: " + pathfinder.CalculatePath(player.transform.position, new NavMeshPath()));*/
		//print("newstatus: " + pathfinder.pathStatus);
		if (pathfinder.pathStatus == NavMeshPathStatus.PathPartial)
        {
			//print("newtimer: " + timer);
			timer += 20f * Time.deltaTime;
        } else
        {
			if (timer > 0f)
			{
				timer -= Time.deltaTime;
			}
        }
		//print("firstbs: " + (timer > 20f) + " second: " + (timer - 20f > 0.001f));
		if (timer > 20f)
		{
			//print("Amazing!");
			while (!foundPlace)
			{
				int objIndex = Random.Range(0, model.transform.childCount);
				//print("object: " + objIndex + " name: " + model.transform.GetChild(objIndex).name);
				if (model.transform.GetChild(objIndex).gameObject.activeSelf && model.transform.GetChild(objIndex).name.Contains("Floor") && !model.transform.GetChild(objIndex).name.Contains("Xterior") && !model.transform.GetChild(objIndex).name.Contains("Hallway.00") && model.transform.GetChild(objIndex).name.Contains("Hallway"))
				{
					//print("this is working too");
					tempDestination = model.transform.GetChild(objIndex).transform.position + new Vector3(0.5f * Random.Range(-1f, 1f) * model.transform.GetChild(objIndex).transform.localScale.x, 8f, 0.5f * Random.Range(-1f, 1f) * model.transform.GetChild(objIndex).transform.localScale.y);
					foundPlace = true;
				}
			}
			timer = 0f;
			/*
			if (pathfinder.pathStatus == NavMeshPathStatus.PathPartial)
            {
				timer = 30f;
            } else
            {
				timer = 0;
            }*/
		}
		//transform.LookAt(player.transform.position);
		//pathfinder.transform.forward = -pathfinder.transform.forward;

		//transform.Rotate(new Vector3(0, 180, 0));
		//}
		/*
		if(/*chase <= 0.0fcaught) {
			if(Vector3.Distance(pathfinder.destination, transform.position - new Vector3(0.0f,6.0f,0.0f)) < 5.0f) {
				// i adjust transform.position 6 units downward so that its on the navmesh
				// in the case the monster cannot find a path the Vector3.Distance check should pass and the code should still run
				do {
					// repeating until it has a destination
                    // WARNING!!!!!!!!
                    // If the navmesh / structure sucks, then this code will nearly crash the game
                    // by generating thousands of points until it finds one
					patrolPoint = MakePatrolPoint();
					//print("Patrol Go to = " + patrolPoint);
				} while (!pathfinder.SetDestination(patrolPoint));
			}
		}*/
	}
	
	void OnCollisionEnter(Collision col) {
		/*
        print("hi");
		if(col.collider.CompareTag("Player")) {
			SceneManager.LoadScene("SampleScene");
			caught = false;
			FadeScript.fade = false;
		}*/
		/*
		if (col.collider.gameObject.name.Contains("Door"))
        {
			print("worowr");
			pathfinder.velocity = Vector3.zero;
		}*/
	}

	void OnBecameVisible(){
		if(sightings==0){
			//play scary oneshot
		}
		sightings++;
	}
		
}
