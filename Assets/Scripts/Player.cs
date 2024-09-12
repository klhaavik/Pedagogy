using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float lookSpeed;
	[SerializeField] private float jumpHeight;
	[SerializeField] private float maximumSpeed;
	private float effectiveMaximumSpeed;
	private Camera cam;
	private GameObject pivot;
	private GameObject hand;
	private float rotX, rotY;
	private Vector3 accel;
	private Rigidbody rb;
	private PhysicMaterial pm;
	private Vector3 flatVelocity;
	private RaycastHit hit;
	public static bool movementEnabled = true;
	
	RectTransform hpBar;
	RectTransform stamBar;
	int hp;
	[SerializeField] int maxHp;
	float stam;
	[SerializeField] float maxStam;
	float stamRecoveryTimer;
    int slot;
	
    [SerializeField] private int INVENTORY_SLOTS = 3;

	public const float PLAYER_HEIGHT = 12.0f;
	public const float PLAYER_HEIGHT_HALF = (PLAYER_HEIGHT / 2.0f);
	
	private GameObject[] inventory;
	public static Vector3 checkpoint;

    GameObject heldDisplay;
	
	[SerializeField] Sprite sprSlot;
	[SerializeField] Sprite sprSlotSelected;
	GameObject[] uiSlots;
	GameObject[] uiItems;

	public GameObject[] slotObjs = new GameObject[3];
	public GameObject[] dispObjs = new GameObject[3];
    Text itemNameText;
	string itemName;

	float visbilityTimer = 0;
	bool visible = false;
	public Canvas endScreen;

	Vector3 ZeroVectorComponents(Vector3 v, byte filter)
    {
        Vector3 ret = new Vector3();
        ret = v;
        if((filter & 0x1) != 0) ret.x = 0;
        if((filter & 0x2) != 0) ret.y = 0;
        if((filter & 0x4) != 0) ret.z = 0;
        return ret;
    }

	float MaxinVec3(Vector3 v) {
		int i;
		float max;
		max = v[0];
		for(i=1;i<3;i++) {
			if(v[i] > max) max = v[i]; 
		}
		return max;
	}
	
	int IntegerClamp(int x, int lo, int hi) {
		if(x < lo) return lo;
		if(x > hi) return hi;
		return x;
	}
	
	private static readonly float[] STANDING_OFFSETS =
	{
		+0.0f, +0.0f,
		+1.5f, +1.5f,
		+1.5f, -1.5f,
		-1.5f, +1.5f,
		-1.5f, -1.5f,
	};
	
	private static readonly float[] SLOT_RENDER_LOCS =
	{
		3.95f, -1001.59f, 7.85f, // slot 0
		3.95f, -1000.0f, 3.93f, // slot 1
		3.95f, -998.4f, 0.0f, // slot 2
	};
	
	public int Slot {
		get {
			return slot;
		}
		set {
			int newSlot;
			newSlot = IntegerClamp(value,0,INVENTORY_SLOTS-1);
			if (slot != newSlot) {
				uiSlots[slot].GetComponent<Image>().sprite = sprSlot;
				uiSlots[newSlot].GetComponent<Image>().sprite = sprSlotSelected;
			}
			slot = newSlot;
		}
	}
	
	public int Health {
		get {
			return hp;
		}
		set {
			if(value <= 0 /*&& !UseLadder.onLadder*/) {
				Die();
				return;
			}
			hp = IntegerClamp(value,0,maxHp);
			hpBar.localScale = new Vector3((float)hp / (float)maxHp, 1.0f, 1.0f);
		}
	}
	
	public float Stamina {
		get {
			return stam;
		}
		set {
			if (value < 0) {
				stam = 0;
			}
			else if (value > maxStam) {
				stam = maxStam;
			} else {
				stam = value;
			}
			stamBar.localScale = new Vector3(stam/maxStam, 1.0f, 1.0f);
		}
	}
	
	bool GetKeyDownJump()
	{
		return Input.GetKeyDown("space");
	}
	
	bool GetKeySprint()
	{
		return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
	}

    
    bool GetLeftClickDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    bool GetItemPickupDown()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
			print("passes 1");
            return true;
        }
        return false;
    }

    int GetEmptyInventorySlot()
    {
        int i;
        for(i=0;i<INVENTORY_SLOTS;i++) {
            if(inventory[i] == null) return i;
        }
        return -1;
    }
	
    // Start is called before the first frame update
    void Start()
    {
		int i;
		pivot = GameObject.Find("Pivot");
		hand = GameObject.Find("handblend");
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		rb = GetComponent<Rigidbody>();
		//whoever keeps uncommenting this, dont
        //Cursor.visible = false;
		hp = maxHp;
		pm = GetComponent<CapsuleCollider>().material;
		stam = maxStam;
		hpBar = GameObject.Find("HpBar").GetComponent<RectTransform>();
		stamBar = GameObject.Find("StamBar").GetComponent<RectTransform>();
		stamRecoveryTimer = 9999.0f;
        heldDisplay = GameObject.Find("HeldDisplay");
		
		slot = 0;
		inventory = new GameObject[INVENTORY_SLOTS];
		uiSlots = new GameObject[INVENTORY_SLOTS];
		uiItems = new GameObject[INVENTORY_SLOTS]; // these items are way below the map
		for(i=0;i<INVENTORY_SLOTS;i++) {
			// bad code
			uiSlots[i] = GameObject.Find("Slot" + i);
			uiItems[i] = null;
		}

        itemNameText = GameObject.Find("ItemName").GetComponent<Text>();
		UpdateItemName();

		checkpoint = GameObject.Find("startGameTrigger").transform.position;
    }
	
	public static void Die()
	{
		Cursor.visible = true;
		GameObject.Find("Player").transform.position = checkpoint;
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.gameObject.name == "Terrain (1)")
        {
			WinScreen.won = true;
			endScreen.enabled = true;
			UnfadeScript.unfade = true;
        }
		float damage;
		// damage = col.impulse.magnitude;
		damage = col.impulse.y;
		if(damage <= 40.0f) return;
		//print(damage);
		damage = col.impulse.magnitude - 40.0f;
		damage /= 8.0f;
		damage *= damage;
		damage += 2;
		Health -= (int)damage;
		return;
	}

    void UpdateItemName()
    {
		char c;
		int i;
		string pre, str;

		if (!inventory[Slot]) {
			itemNameText.text = "";
			return;
		}
		
		str = "";
		pre = inventory[Slot].name;
		
		if(pre.EndsWith("(Clone)")) {
			pre = pre.Remove(pre.Length - 7);
		}
		for(i=0;i<pre.Length;i++) {
			c = pre[i];
            if(i == 0) {
                c = char.ToUpper(c);
            }
			if(c == '_') {
				c = ' ';
			}
			if(char.IsUpper(c) && i != 0) {
				str += " ";
				c = char.ToLower(c);
			}
            str += c;
		}

		itemNameText.text = str;
		itemName = str;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		int i;
		Vector3 tempVec;
		Vector3 boundsSize;
		GameObject disp;
		
		if(transform.position.y < -200.0f) {
			Die();
		}

		//makes inventory visible
		/*if (visible)
        {
			foreach (GameObject g in dispObjs)
			{
				if (g != null)
				{
					g.GetComponent<MeshRenderer>().enabled = true;
				}
			}
			visbilityTimer += Time.deltaTime;
			//itemNameText.GetComponent<Renderer>().enabled = true;
			UpdateItemName();
		} else
        {
			foreach (GameObject g in slotObjs)
			{
				g.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
			}
			foreach (GameObject g in dispObjs)
			{
				if (g != null)
				{
					g.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			//itemNameText.GetComponent<Renderer>().enabled = false;
			itemNameText.text = " ";
		}
		if (visbilityTimer > 5)
        {
			visbilityTimer = 0;
			visible = false;
        }*/

		// view rotation
		
		if(!movementEnabled){
			return;
		}

		rotY -= Input.GetAxis("Mouse Y") * lookSpeed; //Time.deltaTime
		rotX += Input.GetAxis("Mouse X") * lookSpeed; //Time.deltaTime
		rotY = Mathf.Clamp(rotY, -90, 90);
		/*
		if(rotY > 90) {
			rotY = 90;
		}
		if(rotY < -90) {
			rotY = -90;
		}
		*/
		
		pivot.transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);
		//hand.transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);

		//if on ladder, you shouldnt move or use items
		if (UseLadder.onLadder)
        {
			return;
        }
		
		// player movement
		accel = 
			(
			cam.transform.right * Input.GetAxis("Horizontal") +
			cam.transform.forward * Input.GetAxis("Vertical")
			)
			* speed;
			
		accel = new Vector3(accel.x, 0.0f, accel.z);
		
		if(accel.magnitude > 0.1f) {
			pm.dynamicFriction = 0.0f;
		} else {
			pm.dynamicFriction = 5.0f;
		}
		
		effectiveMaximumSpeed = maximumSpeed;
			
		if(GetKeySprint()) {
			stamRecoveryTimer = 0.0f;
			Stamina -= Time.deltaTime * 30;
			if(Stamina > 0) {
				accel *= 3;
				effectiveMaximumSpeed *= 3;
			}
		}
		
		if(stamRecoveryTimer > 1.0f) {
			Stamina += Time.deltaTime * 10;
		}
		
		stamRecoveryTimer += Time.deltaTime;
		
		rb.AddForce(accel, ForceMode.Acceleration);
		
		flatVelocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
		if(flatVelocity.magnitude > effectiveMaximumSpeed) {
			flatVelocity = Vector3.Normalize(flatVelocity) * effectiveMaximumSpeed;
			rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
		}
		
		bool isGrounded;
		if(GetKeyDownJump()) {
			isGrounded = false;
			for(i=0;i<STANDING_OFFSETS.Length;i += 2) {
				tempVec = transform.position;
				tempVec += new Vector3(STANDING_OFFSETS[i], 0.0f, STANDING_OFFSETS[i+1]);
				if(Physics.Raycast(tempVec, Vector3.down, PLAYER_HEIGHT_HALF+0.1f)) {	// if raycast hits
					isGrounded = true;
					break;
				}
			}
			if(isGrounded) {
				rb.velocity += new Vector3(0.0f, jumpHeight, 0.0f);
			}
		}
		
		/*
		
		Basic documentation on the game's inventory system:
		
		- Any gameObject can be tagged as "Item" and will be able to be picked
		up if the gameObject has a collider.
		
		- Any script on the gameObject can be given an "OnUseItem" function to
		program something in when the item is used (via a left click when its
		inventory slot is selected). The player's gameObject is passed in as
		the only parameter to this function.

		- Creating a sprite for the gameObject in the inventory is not
		necessary. (An isometric view of the object is shown automatically.)

        - Item names are generated automatically from the gameObject names.

		*/
		
		// maybe improve this
		if(Input.GetKeyDown("1") || Input.GetKeyDown("2") || Input.GetKeyDown("3"))
        {
			foreach (GameObject g in slotObjs)
			{
				g.GetComponent<Image>().color = new Color(g.GetComponent<Image>().color.r, g.GetComponent<Image>().color.g, g.GetComponent<Image>().color.b, 1);
			}
			visible = true;
		}
		if(Input.GetKeyDown("1")) {
			Slot = 0;
            UpdateItemName();
		}
		if(Input.GetKeyDown("2")) {
			Slot = 1;
            UpdateItemName();
		}
		if(Input.GetKeyDown("3")) {
			Slot = 2;
            UpdateItemName();
		}
		
		if(GetLeftClickDown()) {
			Vector3 temp;
			if(inventory[Slot] != null) {
				temp = inventory[Slot].transform.localScale;
				inventory[Slot].transform.localScale = Vector3.zero; // so its still invisible
				inventory[Slot].SetActive(true);
				inventory[Slot].SendMessage("OnUseItem", gameObject, SendMessageOptions.DontRequireReceiver);
				//inventory[Slot].SetActive(false);
				//inventory[Slot].GetComponent<MeshRenderer>().enabled = false;
				//inventory[Slot].GetComponent<BoxCollider>().enabled = false;
				inventory[Slot].transform.localScale = temp;
			}
		}

        if(GetItemPickupDown()) {
			Physics.Raycast(transform.position, cam.transform.forward, out hit, 10.0f);
			//print("passes 2: " + hit.collider.gameObject.name);
			if (inventory[Slot] != null) {
                heldDisplay.SetActive(false);
                inventory[Slot].transform.position = transform.position + ZeroVectorComponents(cam.transform.forward, 0x2) * 10.0f; // put down
                inventory[Slot].SetActive(true);
                inventory[Slot] = null;
				// delete gameObject used to display the item in the inventory
				Destroy(uiItems[Slot]);
                UpdateItemName();
            }
            else if(hit.collider.gameObject.CompareTag("Item")) {
                // intentionally using the slot selected
				inventory[Slot] = hit.collider.gameObject; // pick up
				boundsSize = hit.collider.bounds.size; // for later
				inventory[Slot].SetActive(false);

				//heldDisplay.SetActive(true);
				//heldDisplay.GetComponent<MeshFilter>().mesh = inventory[Slot].GetComponent<MeshFilter>().mesh;
				//heldDisplay.GetComponent<MeshRenderer>() = inventory[slot].GetComponent<MeshRenderer>();

				// create gameObject used to display the item in the inventory
				disp = Instantiate(
				hit.collider.gameObject,
				new Vector3(SLOT_RENDER_LOCS[Slot*3], SLOT_RENDER_LOCS[Slot*3+1], SLOT_RENDER_LOCS[Slot*3+2]),
				Quaternion.identity
				);
				// disp will be inactive since the item we just copied is inactive
				Component[] components = disp.GetComponents(typeof(Component)); // not sure why this line works
				// we get all components of the object in components
				foreach(Component c in components) {
					// obliterate all but the mesh filter and mesh renderer (and transform)
					if(!(c is Transform || c is MeshFilter || c is MeshRenderer)) {
						Destroy(c);
					}
				}
				// get bounds
				if(disp.GetComponent<MeshFilter>()) {
					boundsSize = disp.GetComponent<MeshFilter>().mesh.bounds.size;
				}
				else if(disp.GetComponent<Renderer>()) {
					disp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					boundsSize = disp.GetComponent<Renderer>().bounds.size;
				}
				// otherwise we will have the colliders
				for(i=0;i<3;i++) boundsSize[i] = Mathf.Abs(boundsSize[i] * disp.transform.localScale[i]);
				disp.transform.localScale /= MaxinVec3(boundsSize);
				uiItems[Slot] = disp;
				disp.SetActive(true);
				dispObjs[Slot] = disp;
				UpdateItemName();
            }
        }
    }
}
