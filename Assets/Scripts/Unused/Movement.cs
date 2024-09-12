using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    public int moveSpeed = 5;
    float horizontalMovement;
    public int maxSpeed = 10;
    public int jumpForce = 5;
    bool jump = false;
    bool grounded = true;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundedRadius = 0.2f;
    public CameraScroll scrollInfo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update(){
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

        //compensate for camera scroll
        if(horizontalMovement != 0) horizontalMovement += scrollInfo.scrollSpeed;
        horizontalMovement *= Time.fixedDeltaTime * 50f;

        if(Input.GetKeyDown("w")){
            jump = true;
        }
    }
    
    void FixedUpdate()
    {
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
			}
		}

        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

        if(jump && grounded){
            rb.AddForce(new Vector2(0, jumpForce));
            grounded = false;
        }

        jump = false;

    }
}
