using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class KaiMovement : MonoBehaviour
{
    [Header("Ground Movement")]
    public float moveSpeedWithBag = 3f;
    public float moveSpeedWithoutBag = 5f;
    float moveSpeed;
    float horizontalMovement;
    Vector3 moveDirection;
    public float velocityDamping;
    bool movingAlongZAxis = true;

    [Header("Jumping")]
    public float jumpForceWithBag = 1f;
    public float jumpForceWithoutBag = 1.5f;
    public float jumpCooldown;
    public float airMultiplier;
    float jumpForce;
    // float continuousJumpForce;
    // public float jumpBoostFactor = 1.0f;
    // public float decayRate = 3f;
    // public int jumpLimit = 1;
    public int numCoyoteFrames = 5;
    bool readyToJump = true;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public float groundedDst;
    public float maxSlopeAngle = 85f;
    RaycastHit slopeHit;
    public float playerHeight = 0.6f;
    public float playerWidth = 0.61f;
    bool touchingLadder;

    [Header("Respawn")]
    public Transform orientation;
    Transform spawnPoint;
    bool resetInProgress = false;
    public bool ResetInProgress {get; set;}
    bool turningCorner = false;
    public float reloadDelay = 2f;
    public float reloadDuration = 1.5f;

    bool hasBag = true;
    bool jumpEnabled = true;
    bool moveRightEnabled = true;
    bool moveLeftEnabled = true;
    bool grounded = true;
    bool exitingSlope = false;
    // bool inCoyoteFrames = false;
    // bool coyoteFramesDisabled = false;
    bool movementEnabled = true;
    InitialValues initialValues;


    [Header("Camera")]
    public Transform camFollowObj;
    public Vector3 defaultCamDamping;
    public Vector3 camDampingWhileFlying;
    Vector3 camDamping;
    public float defaultVerticalAimDamping;
    public float verticalAimDampingWhileFlying;
    float verticalAimDamping;
    public Vector3 defaultCamOffset;
    public Vector3 camOffsetWhileFlying;
    Vector3 camOffset;
    Vector3 camRot;
    public float maxHeight = 15f;
    GameObject cam;
    CinemachineFramingTransposer camTransposer;
    CinemachineComposer camComposer;

    Transform model;
    GameObject bag;
    HeartWipe heartWipeController;
    Rigidbody rb;
    RigidbodyConstraints currentConstraints;
    Text speedTxt;

    void UpdateSpeedText(float speed){
        speedTxt.text = "Speed: " + speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        heartWipeController = GameObject.Find("Heart").GetComponent<HeartWipe>();
        rb = GetComponent<Rigidbody>();
        model = GameObject.Find("Pigeon").GetComponent<Transform>();
        bag = GameObject.Find("Mailbag");
        cam = GameObject.Find("Camera");
        CinemachineVirtualCamera vcam = GameObject.Find("Virtual Camera For Cinematic Shots").GetComponent<CinemachineVirtualCamera>();
        // print(vcam);
        camTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        camComposer = vcam.GetCinemachineComponent<CinemachineComposer>();
        speedTxt = GameObject.Find("Speed").GetComponent<Text>();
        // print(camTransposer);

        moveSpeed = moveSpeedWithBag;
        jumpForce = jumpForceWithBag;
        readyToJump = true;
        spawnPoint = GameObject.Find("Spawnpoint").transform;

        initialValues = new InitialValues();
        UpdateInitialValues();

        // coyoteFramesDisabled = true;
    }

    void UpdatePlayerGraphic(){
        if(!hasBag){
            bag.SetActive(false);
        }else{
            bag.SetActive(true);
        }
        
        if(horizontalMovement < 0){
            model.localEulerAngles = new Vector3(0, 90, 0);
            bag.transform.localEulerAngles = new Vector3(0, 0, 25);
            bag.transform.localPosition = new Vector3(-0.06f, 0.072f, -0.113f);
        }else if(horizontalMovement > 0){
            model.localEulerAngles = new Vector3(0, -90, 0);
            bag.transform.localEulerAngles = new Vector3(0, 0, -25);
            bag.transform.localPosition = new Vector3(0.06f, 0.072f, -0.113f);
        }
    }

    public bool GetKeyLeft(){
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    public bool GetKeyRight(){
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public bool GetKeyJump(){
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
    }

    bool GetKeyDownJump(){
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKey(KeyCode.W);
    }

    bool GetKeyUpJump(){
        return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)|| Input.GetKey(KeyCode.W);
    }

    public bool IsMovingAlongZAxis(){
        return movingAlongZAxis;
    }

    struct InitialValues{
        public bool hasBag;
        public bool movingAlongZAxis;
        public Vector3 spawnPoint;
        public Vector3 spawnRot;
        public RigidbodyConstraints initialConstraints;
    }

    void ResetToInitialValues(){
        hasBag = initialValues.hasBag;
        movingAlongZAxis = initialValues.movingAlongZAxis;
        transform.position = initialValues.spawnPoint;
        orientation.localEulerAngles = initialValues.spawnRot;
        print(initialValues.spawnRot);
        print(transform.eulerAngles);
        // Debug.Break();
    }

    public void UpdateInitialValues(){
        initialValues.hasBag = hasBag;
        initialValues.movingAlongZAxis = movingAlongZAxis;
        spawnPoint.position = transform.position;
        initialValues.spawnPoint = spawnPoint.position;
        spawnPoint.eulerAngles = orientation.localEulerAngles;
        initialValues.spawnRot = spawnPoint.eulerAngles;
        initialValues.initialConstraints = rb.constraints;
        // print(initialValues.initialConstraints);
    }

    void GetInput(){
        horizontalMovement = 0;
        if(moveRightEnabled && GetKeyRight()) horizontalMovement++;
        if(moveLeftEnabled && GetKeyLeft()) horizontalMovement--;

        if(!jumpEnabled) return;
        if(GetKeyJump() && readyToJump && (grounded || !hasBag)){
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //testing purposes
        if(Input.GetKeyUp(KeyCode.R)){
            StartCoroutine(Reset(0, reloadDuration));
        }
    }

    void UpdateCameraTrackingValues(){
        camTransposer.m_XDamping = camDamping.x;
        camTransposer.m_YDamping = camDamping.y;
        camTransposer.m_ZDamping = camDamping.z;

        camTransposer.m_ScreenX = camOffset.x;
        camTransposer.m_ScreenY = camOffset.y;
        camTransposer.m_CameraDistance = camOffset.z;

        camComposer.m_VerticalDamping = verticalAimDamping;
        camFollowObj.localEulerAngles = camRot;
    }
    
    Vector3 GetCameraRotWhileFlying(){
        float percentage = transform.position.y / maxHeight;
        return new Vector3(Mathf.Lerp(-30f, 45f, percentage), 0, 0);
    }

    void Update(){
        if(!movementEnabled) return;

        GetInput();
        UpdatePlayerGraphic();
        StateControl();
        SpeedControl();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + groundedDst, whatIsGround);
        
    }
    
    void FixedUpdate()
    {
        MovePlayer();

        /*if(jump){
            if(hasBag){
                print("jumping");
                if(continuousJumpForce > 0f){
                    rb.AddForce(Vector3.up * continuousJumpForce, ForceMode.Impulse);
                    continuousJumpForce -= decayRate;
                }
                if(grounded){
                    //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    continuousJumpForce = jumpForce * jumpBoostFactor;
                    grounded = false;
                }
            }else{
                float velY = Mathf.Clamp(rb.velocity.y, 0, Mathf.Infinity);
                //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                rb.velocity += Vector3.up * jumpForce;
                continuousJumpForce = jumpForce * jumpBoostFactor;
            }
            jump = false;
            
        }*/

        /*if(!coyoteFramesDisabled && !inCoyoteFrames && !GroundCheck() && grounded){
            print("Coyote Frames");
            StartCoroutine(CoyoteFrames(numCoyoteFrames));
            inCoyoteFrames = true;
        }*/
    }

    void MovePlayer(){
        if(!movementEnabled) return;

        /*if(CanLandOnLedge()){
            print("can land on ledge");
            transform.position = Vector3.MoveTowards(transform.position, GetLedgeLandPoint(), Time.fixedDeltaTime);
            return;
        }*/

        moveDirection = orientation.right * horizontalMovement;

        if(OnSlope() && !exitingSlope){
            // print("on slope");

            moveDirection = GetSlopeMoveDir() * moveSpeed;
            rb.velocity = Vector3.MoveTowards(rb.velocity, moveDirection, Time.fixedDeltaTime / velocityDamping);

        }else{
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 targetFlatVel = Vector3.MoveTowards(flatVel, moveDirection * moveSpeed, Time.fixedDeltaTime / velocityDamping);
            rb.velocity = new Vector3(targetFlatVel.x, rb.velocity.y, targetFlatVel.z);
        }
        
        // rb.useGravity = !OnSlope();
    }

    void Jump(){
        exitingSlope = true;
        
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump(){
        readyToJump = true;
        exitingSlope = false;
    }

    void StateControl(){

        if(hasBag){
            moveSpeed = moveSpeedWithBag;
            jumpForce = jumpForceWithBag;

            camDamping = defaultCamDamping;
            camOffset = defaultCamOffset;
            verticalAimDamping = defaultVerticalAimDamping;
            camRot = new Vector3(0, camFollowObj.localEulerAngles.y, 0);
        }else{
            moveSpeed = moveSpeedWithoutBag;
            jumpForce = jumpForceWithoutBag;

            if(!grounded) {
                camDamping = camDampingWhileFlying;
                camOffset = camOffsetWhileFlying;
                verticalAimDamping = verticalAimDampingWhileFlying;
                camRot = GetCameraRotWhileFlying();
            }else{
                camDamping = defaultCamDamping;
                camOffset = defaultCamOffset;
                verticalAimDamping = defaultVerticalAimDamping;
                camRot = new Vector3(0, camFollowObj.localEulerAngles.y, 0);
            }
        }

        // UpdateCameraTrackingValues();

        if(turningCorner) return;

        if(movingAlongZAxis){
            currentConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
        }else{
            currentConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            // print(currentConstraints);
        }
        // rb.constraints = currentConstraints;
    }

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + groundedDst, whatIsGround)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            print(angle);
            // print(slopeHit.collider);
            // print("Normal: " + slopeHit.normal + "   Angle: " + angle + "      exiting slope: " + exitingSlope);
            return angle < maxSlopeAngle && angle >= 1f;
        }/*else if(Physics.Raycast(transform.position, moveDirection, out slopeHit, playerWidth * 0.5f + 0.1f, whatIsLadder)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            // print(angle);
            return angle < maxSlopeAngle && angle >= 1f;
        }*/
        return false;
    }

    bool OnLadder()
    {
        if(touchingLadder && Physics.Raycast(transform.position, moveDirection, out slopeHit, playerWidth * 0.5f + 0.1f, whatIsGround)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            print(slopeHit.normal);
            // print(angle);
            return angle >= 1f;
        }

        return false;
    }

    Vector3 GetSlopeMoveDir(){
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void SpeedControl(){
        if(OnSlope() && !exitingSlope){
            if(rb.velocity.magnitude > moveSpeed){
                rb.velocity = rb.velocity.normalized * moveSpeed;
                UpdateSpeedText(rb.velocity.magnitude);
            }
            
        }else{

            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if(flatVel.magnitude > moveSpeed){
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                UpdateSpeedText(limitedVel.magnitude);
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }else{
                UpdateSpeedText(flatVel.magnitude);
            }
        }
        
    }

    /*private IEnumerator CoyoteFrames(int numFrames){
        int x = 0;
        while(x < numFrames){
            x++;
            yield return null;
        }
        grounded = false;
        jumpCounter++;
        continuousJumpForce = 0f;
    }*/


    void OnCollisionEnter(Collision col){
        // print("colliding");
        if(col.gameObject.layer == 9){
            StartCoroutine(Reset(reloadDelay, reloadDuration));
        }else if(col.gameObject.CompareTag("Ladder")){
            // print("on ladder");
            touchingLadder = true;
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.layer == 6 && !ResetInProgress){
            StartCoroutine(Reset(0, reloadDuration));
            ResetInProgress = true;
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.CompareTag("Ladder")){
            touchingLadder = false;
            // print("off ladder");
        }
    }

    public void SetEnableMovement(bool input){
        movementEnabled = input;
    }

    public void SetJumpEnabled(bool enabled){
        jumpEnabled = enabled;
    }

    public void OnlyMoveLeft(){
        moveLeftEnabled = true;
        moveRightEnabled = false;
        jumpEnabled = false;
    }

    public void OnlyMoveRight(){
        moveLeftEnabled = false;
        moveRightEnabled = true;
        jumpEnabled = false;
    }

    public void FullMovement(){
        moveLeftEnabled = true;
        moveRightEnabled = true;
        jumpEnabled = true;
    }

    public void NoMovement(){
        moveLeftEnabled = false;
        moveRightEnabled = false;
        jumpEnabled = false;
    }



    public void SetSpawnPoint(Vector3 newPos){
        spawnPoint.position = newPos;
    }

    public void ResetMovement(){
        rb.velocity = Vector3.zero;

        // coyoteFramesDisabled = true;
        horizontalMovement = 0f;

        model.localEulerAngles = new Vector3(0, -90, 0);
        bag.transform.localEulerAngles = new Vector3(0, 0, -25);
        bag.transform.localPosition = new Vector3(0.06f, 0.072f, -0.113f);
        
        camTransposer.m_XDamping = 0;
        camTransposer.m_YDamping = 0;
        camTransposer.m_ZDamping = 0;
        // print("reset movement");
        // Debug.Break();
    }

    private IEnumerator Reset(float reloadDelay, float reloadDuration){
        SetEnableMovement(false);
        horizontalMovement = 0;
        yield return new WaitForSeconds(reloadDelay);

        heartWipeController.FadeOut();
        yield return new WaitForSeconds(reloadDuration);

        ResetMovement();
        ResetToInitialValues();
        ResetInProgress = true;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(0.1f);

        heartWipeController.FadeIn();
        

        SetEnableMovement(true);
        rb.constraints = initialValues.initialConstraints;
        camTransposer.m_XDamping = camDamping.x;
        camTransposer.m_YDamping = camDamping.y;
        camTransposer.m_ZDamping = camDamping.z;
        // print("finished reset");

        ResetInProgress = false;
    }




    public void ToggleCorner(){
        movementEnabled = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(TurnCorner());
    }

    private IEnumerator TurnCorner()
    {
        float initialAngle = orientation.localEulerAngles.y;
        float rotAngle = initialAngle - 90f;
        float t = 0f;
        movingAlongZAxis = !movingAlongZAxis;
        turningCorner = true;
        // print(movingAlongZAxis);

        while(true){
            t += Time.deltaTime;
            orientation.localEulerAngles = new Vector3(0, Mathf.SmoothStep(initialAngle, rotAngle, t), 0);
            if (Mathf.Approximately(orientation.localEulerAngles.y % 90f, 0f)) break;
            yield return null;
        }

        movementEnabled = true;
        turningCorner = false;
    }

    public void ToggleBagOff() {
        hasBag = false;
    }
    public void ToggleBagOn() {
        hasBag = true;
    }

    public bool HasBag(){
        return hasBag;
    }
}
