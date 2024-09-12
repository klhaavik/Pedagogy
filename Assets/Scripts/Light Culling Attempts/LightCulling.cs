using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCulling : MonoBehaviour
{
    List<Light> lights;
    List<Light> roomLights;

    //keeps track of all gameobjects that store lighting so they can be turned on and off whenever necessary
    public GameObject firstFloorHalls, secondFloorHalls, thirdFloorHalls;
    public GameObject firstFloorRooms, secondFloorRooms, thirdFloorRooms;
    public GameObject firstFloorRoomsFront, firstFloorRoomsBack;
    public GameObject firstFloorRoomsLeft, firstFloorRoomsMiddle, firstFloorRoomsRight;
    public GameObject secondFloorRoomsLeft, secondFloorRoomsMiddle, secondFloorRoomsRight;
    public GameObject thirdFloorRoomsLeft, thirdFloorRoomsMiddle, thirdFloorRoomsRight;
    public GameObject leftStairs, middleStairs, rightStairs;

    int firstFloorLimit = 10;
    int secondFloorLimit = 65;
    int leftStairLimit = 280;
    int middleStairLimit = 95;
    int rightStairLimit = -405;
    int leftLimit = 80;
    int middleLimit = -200;
    int backLimit = -100;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        
        /*GameObject[] arrLights = GameObject.FindGameObjectsWithTag("Lighting");
        GameObject[] arrRoomLights = GameObject.FindGameObjectsWithTag("RoomLighting");
        foreach(GameObject g in arrLights){
            lights.Add(g.GetComponent<Light>());
        }
        foreach(GameObject g in arrRoomLights){
            roomLights.Add(g.GetComponent<Light>());
        }*/

        //enable lights close to player position on start
        EnableSecondFloorLights();
    }

    // Update is called once per frame
    void Update()
    {
        //detects a significant shift in the player position, significant enough that which lights are visible 
        //may have changed and so the lights will need to be updated
        pos = transform.position;
        
        //updating stairs
        if(Mathf.Abs(pos.x - leftStairLimit) < 20){
            EnableLeftStairs();
        }else if (Mathf.Abs(pos.x - middleStairLimit) < 20){
            EnableMiddleStairs();
        }else if (Mathf.Abs(pos.x - rightStairLimit) < 20){
            EnableRightStairs();
        }

        //update specific rooms
        UpdateRoomLights("x");

        //updating by floor
        if(pos.y < firstFloorLimit){
            EnableFirstFloorLights();
        }else if (pos.y < secondFloorLimit){
            EnableSecondFloorLights();
        }else{
            EnableThirdFloorLights();
        }

        //update specific rooms on first floor
        UpdateRoomLights("z");
    }

    void EnableFirstFloorLights(){
        firstFloorHalls.SetActive(true);
        secondFloorHalls.SetActive(false);
        thirdFloorHalls.SetActive(false);

        firstFloorRooms.SetActive(true);
        secondFloorRooms.SetActive(false);
        thirdFloorRooms.SetActive(false);
    }

    void EnableSecondFloorLights(){
        firstFloorHalls.SetActive(false);
        secondFloorHalls.SetActive(true);
        thirdFloorHalls.SetActive(false);

        firstFloorRooms.SetActive(false);
        secondFloorRooms.SetActive(true);
        thirdFloorRooms.SetActive(false);
    }

    void EnableThirdFloorLights(){
        firstFloorHalls.SetActive(false);
        secondFloorHalls.SetActive(false);
        thirdFloorHalls.SetActive(true);

        firstFloorRooms.SetActive(false);
        secondFloorRooms.SetActive(false);
        thirdFloorRooms.SetActive(true);
    }

    void UpdateRoomLights(string axis){
        
        //only lights in nearby classrooms should be visible
        if(axis == "x"){
            if(pos.x > leftLimit){
                firstFloorRoomsLeft.SetActive(true);
                secondFloorRoomsLeft.SetActive(true);
                thirdFloorRoomsLeft.SetActive(true);

                firstFloorRoomsMiddle.SetActive(false);
                secondFloorRoomsMiddle.SetActive(false);
                thirdFloorRoomsMiddle.SetActive(false);

                firstFloorRoomsRight.SetActive(false);
                secondFloorRoomsRight.SetActive(false);
                thirdFloorRoomsRight.SetActive(false);
            }else if (pos.x > middleLimit){
                firstFloorRoomsLeft.SetActive(false);
                secondFloorRoomsLeft.SetActive(false);
                thirdFloorRoomsLeft.SetActive(false);

                firstFloorRoomsMiddle.SetActive(true);
                secondFloorRoomsMiddle.SetActive(true);
                thirdFloorRoomsMiddle.SetActive(true);

                firstFloorRoomsRight.SetActive(false);
                secondFloorRoomsRight.SetActive(false);
                thirdFloorRoomsRight.SetActive(false);
            }else{
                firstFloorRoomsLeft.SetActive(false);
                secondFloorRoomsLeft.SetActive(false);
                thirdFloorRoomsLeft.SetActive(false);

                firstFloorRoomsMiddle.SetActive(false);
                secondFloorRoomsMiddle.SetActive(false);
                thirdFloorRoomsMiddle.SetActive(false);

                firstFloorRoomsRight.SetActive(true);
                secondFloorRoomsRight.SetActive(true);
                thirdFloorRoomsRight.SetActive(true);
            }
        }else{
            if(pos.z < backLimit){
                firstFloorRoomsBack.SetActive(true);
                firstFloorRoomsFront.SetActive(false);
                secondFloorRooms.SetActive(false);
                thirdFloorRooms.SetActive(false);
            }else{
                firstFloorRoomsBack.SetActive(false);
                firstFloorRoomsFront.SetActive(true);
                secondFloorRooms.SetActive(true);
                thirdFloorRooms.SetActive(true);
            }
        }
    }

    void EnableLeftStairs(){
        leftStairs.SetActive(true);
        middleStairs.SetActive(false);
        rightStairs.SetActive(false);
    }

    void EnableMiddleStairs(){
        leftStairs.SetActive(false);
        middleStairs.SetActive(true);
        rightStairs.SetActive(false);
    }

    void EnableRightStairs(){
        leftStairs.SetActive(false);
        middleStairs.SetActive(false);
        rightStairs.SetActive(true);
    }
}
