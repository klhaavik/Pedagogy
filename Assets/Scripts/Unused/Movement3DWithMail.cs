using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3DWithMail : MonoBehaviour
{
    Rigidbody rb;
    public int moveSpeed = 5;
    float horizontalMovement;
    public int maxSpeed = 10;
    public int initialJumpForce = 1000;
    public float continuousJumpForce = 50f;
    public float decayRate = 5f;
    public float gravityScale = 5f;
    Vector3 gravity;
    bool jump = false;
    bool grounded = true;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundedRadius = 0.2f;
    public CameraScroll3D scrollInfo;

    bool movingInZDir = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity * gravityScale;


    }

    // Update is called once per frame
    /*IEnumerator DoJump()
    { 
        //the initial jump
        rb.AddForce(Vector3.up * initialJumpForce);

        yield return null;

        //can be any value, maybe this is a start ascending force, up to you
        continuousJumpForce = initialJumpForce;
        
        while(Input.GetKey(KeyCode.Space) && currentForce > 0)
        {
            rb.AddForce(Vector3.up * currentForce);
                
            continuousJumpForce -= decayRate * Time.deltaTime;

            yield return null;
        }
    }*/

    void Update(){

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

        //compensate for camera scroll
        /*if(horizontalMovement != 0) horizontalMovement += scrollInfo.scrollSpeed;
        horizontalMovement *= Time.fixedDeltaTime * 50f;*/

        if(Input.GetKey("w")){
            jump = true;
        }
    }
    
    void FixedUpdate()
    {
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
			}
		}

        if(movingInZDir){
            rb.velocity = new Vector3(0, rb.velocity.y, horizontalMovement);
        }else{
            rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, 0);
        }
        

        rb.AddForce(gravity, ForceMode.Acceleration);

        /*if(jump && rb.velocity.y >= 0){
            if(grounded){
                rb.AddForce(new Vector3(0, initialJumpForce, 0));
                grounded = false;
            }
            rb.AddForce(Vector3.up * continuousJumpForce);
        }*/

        if(jump){
            if(grounded){
                rb.AddForce(Vector3.up * initialJumpForce);
                /*if (!hasBag)
                {
                    rb.AddForce(Vector3.up * 3 * initialJumpForce);
                }*/
                continuousJumpForce = initialJumpForce;
            }
            if(continuousJumpForce > 0){
                rb.AddForce(Vector3.up * continuousJumpForce);   
                continuousJumpForce -= decayRate;
            }
        }

        jump = false;

    }

    public void TurnCorner(){
        movingInZDir = !movingInZDir;
    }
}
