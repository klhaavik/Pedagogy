using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeLanding : MonoBehaviour
{
    KaiMovement movement;
    Rigidbody rb;

    [Header("Ledge Detection")]
    public LayerMask whatIsLedge;
    public Transform feetPos;
    public float ledgeDetectionLength;
    public float ledgeDetectionSphereCastRadius;
    RaycastHit ledgeHit;
    Vector3 ledgeLandPoint;
    bool landing = false;

    [Header("Ledge Landing")]
    public float moveToLedgeSpeed;
    public float maxLedgeLandDistance;
    public float minTimeOnLedge = 0.15f;
    float timeOnLedge = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<KaiMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        LedgeDetection();
        SubStateMachine();
    }

    void SubStateMachine(){
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool anyInputKeyPressed = horizontalInput != 0 || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);

        if(landing){
            rb.AddForce((ledgeLandPoint - feetPos.position).normalized * moveToLedgeSpeed, ForceMode.Force);
        }
    }

    void LedgeDetection(){
        bool ledgeDetected = Physics.SphereCast(feetPos.position, ledgeDetectionSphereCastRadius, rb.velocity, out ledgeHit, ledgeDetectionLength, whatIsLedge);

        if(!ledgeDetected) return;

        ledgeLandPoint = GetLedgeLandPoint();

        float distanceToLedge = Vector3.Distance(feetPos.position, ledgeLandPoint);

        if(distanceToLedge < maxLedgeLandDistance && !landing){
            BeginLanding();
        }
    }

    Vector3 GetLedgeLandPoint(){
        return ledgeHit.collider.ClosestPoint(feetPos.position) - feetPos.localPosition;
    }

    void BeginLanding(){
        landing = true;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    void FreezeRigidbodyOnHold(){
        rb.useGravity = false;

        Vector3 directionToLedge = ledgeLandPoint - feetPos.position;
        float distanceToLedge = Vector3.Distance(feetPos.position, ledgeLandPoint);

    }

    void ExitLedgeHold(){
        landing = false;
    }
}
