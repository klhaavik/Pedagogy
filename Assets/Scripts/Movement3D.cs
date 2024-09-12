using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    float playerHeight = 0.6f;
    
    public float moveSpeedWithBag = 3f;
    public float moveSpeedWithoutBag = 5f;
    float effectiveMoveSpeed;
    float horizontalMovement;

    public float initialJumpForceWithBag = 1f;
    public float initialJumpForceWithoutBag = 1.5f;
    public float effectiveInitialJumpForce;
    float continuousJumpForce;
    public float jumpBoostFactor = 1.0f;
    public float decayRate = 3f;
    public int jumpLimit = 1;
    public int numCoyoteFrames = 5;

    List<Transform> groundChecks;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundedDst = 0.1f;
    public float colSize = 0.05f;
    float maxSlopeAngle = 85f;
    RaycastHit slopeHit;

    Vector3 spawnPoint;

    bool hasBag = true;
    bool jump = false;
    bool jumpEnabled = true;
    bool moveRightEnabled = true;
    bool moveLeftEnabled = true;
    bool grounded = true;
    bool inCoyoteFrames = false;
    bool coyoteFramesDisabled = false;
    bool movementEnabled = true;
    int jumpCounter = 0;


    public Vector3 defaultCamOffset;
    public Vector3 defaultCamDamping;


    GameObject cam;
    CinemachineFramingTransposer camTransposer;
    Transform model;
    GameObject bag;
    HeartWipe heartWipeController;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        heartWipeController = GameObject.Find("Heart").GetComponent<HeartWipe>();
        rb = GetComponent<Rigidbody>();
        model = GameObject.Find("Pigeon").GetComponent<Transform>();
        bag = GameObject.Find("Mailbag");
        cam = GameObject.Find("Camera");
        CinemachineVirtualCamera vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        // print(vcam);
        camTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        // print(camTransposer);

        effectiveMoveSpeed = moveSpeedWithBag;
        effectiveInitialJumpForce = initialJumpForceWithBag;
        spawnPoint = GameObject.Find("Spawnpoint").transform.position;

        groundChecks = new List<Transform>();

        int count = groundCheck.childCount;
        for(int i = 0; i < count; i++)
        {
            groundChecks.Add(groundCheck.GetChild(i));
        }

        coyoteFramesDisabled = true;
    }

    bool GetKeyJump(){
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);
    }

    bool GetKeyDownJump(){
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
    }

    bool GetKeyUpJump(){
        return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow);
    }


    void Update(){
        if(!movementEnabled) return;

        horizontalMovement = Input.GetAxis("Horizontal");
        if(!moveRightEnabled) horizontalMovement = Mathf.Clamp(horizontalMovement, -1, 0);
        if(!moveLeftEnabled) horizontalMovement = Mathf.Clamp(horizontalMovement, 0, 1);
        horizontalMovement *= effectiveMoveSpeed;

        if(horizontalMovement < 0){
            model.localEulerAngles = new Vector3(0, 90, 0);
            bag.transform.localEulerAngles = new Vector3(0, 0, 25);
            bag.transform.localPosition = new Vector3(-0.06f, 0.072f, -0.113f);
        }else if(horizontalMovement > 0){
            model.localEulerAngles = new Vector3(0, -90, 0);
            bag.transform.localEulerAngles = new Vector3(0, 0, -25);
            bag.transform.localPosition = new Vector3(0.06f, 0.072f, -0.113f);
        }

        if(!jumpEnabled) return;

        if(hasBag){
            // print("here");
            if(GetKeyJump() && jumpCounter < jumpLimit){
                jump = true;
                // print("started jump");
            }
            if(GetKeyUpJump() && !grounded){
                jumpCounter++;
            }
        }else{
            if(GetKeyDownJump()){
                jump = true;
            }
        }


        //test checkpoints

    }
    
    void FixedUpdate()
    {
        if(!movementEnabled) return;

        Vector3 horizontal;
        if(OnSlope()){
            horizontal = GetSlopeMoveDir() * horizontalMovement;
            print("slope movement");
        }else{
            horizontal = transform.right * horizontalMovement;
        }
        rb.velocity = new Vector3(horizontal.x, horizontal.y + rb.velocity.y, horizontal.z);

        rb.useGravity = !OnSlope();

        if(Input.GetKeyUp(KeyCode.R)){
            StartCoroutine(Reset(0, 1.5f));
        }

        if(!jumpEnabled) return;

        if(jump){
            if(hasBag){
                print("jumping");
                if(continuousJumpForce > 0f){
                    rb.AddForce(Vector3.up * continuousJumpForce, ForceMode.Impulse);
                    continuousJumpForce -= decayRate;
                }
                if(grounded){
                    //rb.AddForce(Vector3.up * effectiveInitialJumpForce, ForceMode.Impulse);
                    rb.velocity = new Vector3(rb.velocity.x, effectiveInitialJumpForce, rb.velocity.z);
                    continuousJumpForce = effectiveInitialJumpForce * jumpBoostFactor;
                    grounded = false;
                }
            }else{
                float velY = Mathf.Clamp(rb.velocity.y, 0, Mathf.Infinity);
                //rb.velocity = new Vector3(rb.velocity.x, effectiveInitialJumpForce, rb.velocity.z);
                rb.velocity += Vector3.up * effectiveInitialJumpForce;
                continuousJumpForce = effectiveInitialJumpForce * jumpBoostFactor;
            }
            jump = false;
            
        }

        /*if(!coyoteFramesDisabled && !inCoyoteFrames && !GroundCheck() && grounded){
            print("Coyote Frames");
            StartCoroutine(CoyoteFrames(numCoyoteFrames));
            inCoyoteFrames = true;
        }*/
    }

    bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDir(){
        return Vector3.ProjectOnPlane(transform.right, slopeHit.normal).normalized;
    }

    bool GroundCheck(){
        foreach(Transform t in groundChecks){
            // print(t);
            if(Physics.Raycast(t.position, -transform.up, groundedDst, whatIsGround)){
                print("can jump");
                return true;
            }
        }
        return false;
    }

    private IEnumerator CoyoteFrames(int numFrames){
        int x = 0;
        while(x < numFrames){
            x++;
            yield return null;
        }
        grounded = false;
        jumpCounter++;
        continuousJumpForce = 0f;
    }

    public void ResetJump(){
        grounded = true;
        inCoyoteFrames = false;
        coyoteFramesDisabled = false;
        continuousJumpForce = 0f;
        jumpCounter = 0;
        print("reset jump");
    }


    void OnCollisionEnter(Collision col){
        // print("col");
        print("colliding");
        if(col.gameObject.layer == 9){
            StartCoroutine(Reset(2, 1.5f));
        }else if(col.gameObject.layer == 6){
            print("death");
            StartCoroutine(Reset(0, 1.5f));
        }else if(GroundCheck()) {
            ResetJump();
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
        spawnPoint = newPos;
    }

    public void ResetToCheckpoint(){
        rb.velocity = Vector3.zero;
        transform.position = spawnPoint;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        cam.transform.position = spawnPoint + defaultCamOffset;

        coyoteFramesDisabled = true;
        horizontalMovement = 0f;
        jump = false;

        model.localEulerAngles = new Vector3(0, -90, 0);
        bag.transform.localEulerAngles = new Vector3(0, 0, -25);
        bag.transform.localPosition = new Vector3(0.06f, 0.072f, -0.113f);
        
        camTransposer.m_XDamping = 0;
        camTransposer.m_YDamping = 0;
        camTransposer.m_ZDamping = 0;
        // Debug.Break();
    }

    private IEnumerator Reset(float reloadDelay, float reloadDuration){
        SetEnableMovement(false);
        yield return new WaitForSeconds(reloadDelay);

        heartWipeController.FadeOut();
        yield return new WaitForSeconds(reloadDuration);

        ResetToCheckpoint();
        heartWipeController.FadeIn();
        yield return new WaitForSeconds(0.05f);

        SetEnableMovement(true);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        camTransposer.m_XDamping = defaultCamDamping.x;
        camTransposer.m_YDamping = defaultCamDamping.y;
        camTransposer.m_ZDamping = defaultCamDamping.z;
    }




    public void ToggleCorner(){
        movementEnabled = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(TurnCorner());
    }

    private IEnumerator TurnCorner()
    {
        float initialAngle = transform.localEulerAngles.y;
        float rotAngle = initialAngle - 90f;
        float t = 0f;

        while(true){
            t += Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, Mathf.SmoothStep(initialAngle, rotAngle, t), 0);
            if (t >= 1f) break;
            yield return null;
        }
        movementEnabled = true;
    }

    public void ToggleBagOff() {
        if (hasBag == true)
        {
            bag.SetActive(false);
            //play animation
            hasBag = false;
            effectiveMoveSpeed = moveSpeedWithoutBag;
            effectiveInitialJumpForce = initialJumpForceWithoutBag;
        }
    }
    public void ToggleBagOn() {
        if (hasBag == false)
        {
            bag.SetActive(true);
            //play animation
            hasBag = true;
            effectiveMoveSpeed = moveSpeedWithBag;
            effectiveInitialJumpForce = initialJumpForceWithBag;
        }
    }

    public bool HasBag(){
        return hasBag;
    }
}
