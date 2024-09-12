using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeightCheckProjectileThrow : MonoBehaviour
{
    Transform orientation;
    KaiMovement movement;
    public GameObject projectilePrefab;
    bool thrown = false;
    int speed = 20;
    int numFrames = 30;
    float time = 0.3f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<KaiMovement>();
        orientation = GameObject.Find("Orientation").GetComponent<Transform>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > movement.maxHeight && !thrown){
            StartCoroutine(ThrowAfterDelay(numFrames));
            thrown = true;
        }
    }

    void ThrowProjectile(){
        //Vector3 camCornerPoint = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 4f));
        //camCornerPoint = new Vector3(camCornerPoint.x, camCornerPoint.y, transform.position.z);
        movement.SetEnableMovement(false);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        Vector3 radius = Quaternion.Euler(Random.Range(0f, 180f), 0, 0) * orientation.right * 6f;
        //Vector3 radius = (camCornerPoint - transform.position) * 1.2f;
        print("Radius: " + radius);
        //radius = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * radius;
        //print("New radius: " + radius);
        Vector3 spawnPos = transform.position + radius;
        print("Spawnpoint: " + spawnPos);

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        /*float time = radius.magnitude / speed;
        print("Time: " + time);*/
        Vector3 adjustedPos = transform.position + GetComponent<Rigidbody>().velocity * time + Physics.gravity * time * time / 2;
        print("Adjusted Pos: " + adjustedPos);
        //Vector3 dir = (adjustedPos - projectile.transform.position).normalized;
        //Vector3 dir = (transform.position - spawnPos).normalized;*/
        float deltaX = movement.IsMovingAlongZAxis() ? (adjustedPos.z - spawnPos.z) : (adjustedPos.x - spawnPos.x);
        float deltaY = adjustedPos.y - spawnPos.y;

        float velX = deltaX / time;
        float velY = (deltaY - Physics.gravity.y * time * time / 2) / time;
        print("Velocity: " + velX + ", " + velY + ", 0");

        projectile.GetComponent<Rigidbody>().velocity = movement.IsMovingAlongZAxis() ? new Vector3(0f, velY, velX) : new Vector3(velX, velY, 0f);
    }

    private IEnumerator ThrowAfterDelay(int numFrames){
        float t = 0;
        while(t < numFrames / 60f){
            t += Time.deltaTime;
            yield return null;
        }
        ThrowProjectile();
    }
}
