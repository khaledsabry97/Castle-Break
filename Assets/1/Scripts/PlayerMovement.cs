using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement :NetworkBehaviour
{
	[SerializeField][Range(0, 360)] float movingTurnSpeed = 300;
	[SerializeField][Range(0, 360)] float stationaryTurnSpeed = 150;
	[SerializeField][Range(0, 50)] float jumpPower = 5;
	[SerializeField][Range(0, 1)] float runCycleOffset = 0.2f;
	[SerializeField][Range(0, 1)] float heightCapsule = 0.5f;

	[Header("Speed Up")]
	[SerializeField][Range(0, 5)] float gravityMultiplier = 2;
	[SerializeField][Range(0, 5)] float moveSpeedMultiplier = 1;
	[SerializeField][Range(0, 5)] float animSpeedMultiplier = 1;
	[SerializeField][Range(0, 1)] float checkGroundDistance = 0.2f;

	//rigidbody for the model
	private Rigidbody rig;
	//animator for the model
	private Animator anim;
	//check if the character on the ground or not
	private bool isGround;
	//this determined by the movingturnspeed and stationaryturnspeed
	private float turnAmount;
	//this detemined by move.magnitude
	private float forwardAmount;
	//this to change capsule height while crouch
	private float capsuleHeight;
	//this to change capsule center while crouch
	private Vector3 capsuleCenter;
	//this is refernce to the capsule collider to change while crouching
	private CapsuleCollider capsule;
	//to get the normal vector on the ground
	private Vector3 groundNormal;
	//to get the transform of the main camera
	private Transform cam;
	// vector to determind the blue axis and detemined by cam.forward
	private Vector3 m_cam_forward;
	//vector that combine horizontal and vertical movement
	private Vector3 move;
	//determine if the character jumped or not
	private bool jump;
	//for input axis horizontal
	private float h;
	//for input axis vertical
	private float v;
	//determine if the character crouch or not;
	private bool crouch;
	//determine if the character is already crouch or not
	private bool crouching;
	//determine whether the game is multiplayer or not
	public bool Multiplayer;
	//
	private NetworkAnimator NetAnim;
	private PlayerHealth health;
	private PlayerSound playerSound;
	[HideInInspector] public bool MovementAllowed;

	private string Run;

	//not used now (used only for triggers)

	void Start()
	{
		rig = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		capsule = GetComponent<CapsuleCollider>();
		NetAnim = GetComponent<NetworkAnimator>();
		health = GetComponent<PlayerHealth>();
		playerSound = GetComponent<PlayerSound>();

		cam = Camera.main.transform;
		turnAmount = stationaryTurnSpeed;
		forwardAmount = move.z;
		capsuleHeight = capsule.height;
		capsuleCenter = capsule.center;
		rig.constraints = RigidbodyConstraints.FreezeRotation;
		isGround = true;
		jump = false;
		crouch = false;
		crouching = false;
		Run = "Running";


		//GameManager.instance.player = gameObject;
		if (GameObject.Find("NetworkManager") == null)
			Multiplayer = false;
		else
			Multiplayer = true;

	}

	private void FixedUpdate()
	{
		if (Multiplayer)
		{
			if (!isLocalPlayer)
			{
				return;
			}
		}
		if (health.Dead)
		{
			return;
		}
		if (Multiplayer)
			GetInputs();
		else if (MovementAllowed)
			GetInputs();
		if (move.magnitude > 0.1)
			move.Normalize();

		move = transform.InverseTransformDirection(move);
		checkGround();
		move = Vector3.ProjectOnPlane(move, groundNormal);
		turnAmount = Mathf.Atan2(move.y, move.x);
		turnAmount = move.x;
		forwardAmount = move.z;
		applyExtraRotation();
		handleGroundMovement();
		handleJumpMovement();
		ScaleCapsuleForCrouching();
		UpdateAnimation();
		jump = false;



	}
        

	//get the vector move
	private void GetInputs()
	{
		// get input from multiple platforms 
		h = CrossPlatformInputManager.GetAxis("Horizontal");
		v = CrossPlatformInputManager.GetAxis("Vertical");
		if (!jump)
		{
			jump = CrossPlatformInputManager.GetButtonDown("Jump");

		}

		// to get the forward move normalized to some value
		m_cam_forward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1).normalized);

		// vector that determine the move of the character
		// we could replace m_cam_forward with cam.forward but this is better for normalizing
		move = v * m_cam_forward + h * cam.right;

		//this can be modified if it doesn't work with blend tree of jump
		if (!isGround)
		{
			if (move.z < 0)
				move.z = 0;
		}
		// if you want to speed up the move
		if (Input.GetKey(KeyCode.E))
		{
			move *= 2;
		}
		/* if (Input.GetKey(KeyCode.C))
        {
            move *= 0.5f;
        }*/

		if (Input.GetKey(KeyCode.LeftAlt))
		{
			crouch = true;
		}
		else
			crouch = false;
	}
       
	// update the animation parameters
	private void UpdateAnimation()
	{
		anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
		anim.SetBool("Crouch", crouching);
		anim.SetBool("OnGround", isGround);
		anim.SetBool("Jump", jump);
		if (forwardAmount > 0.1)
		{
			playerSound.Play(Run);
		}
		else
		{
			playerSound.Stop(Run);
		}
		//for online

		if (!isGround)
			anim.SetFloat("Jump", rig.velocity.y);
		else
			anim.SetBool("Jump", false);


		if (isGround && move.magnitude > 0.1)
			anim.speed = animSpeedMultiplier;
		else
			anim.speed = 1;


	}
        

	// change height and center for capsule collider to make the crouching work
	private void ScaleCapsuleForCrouching()
	{
		if (isGround && crouch)
		{
			if (crouching)
				return;
			capsule.height *= heightCapsule;
			capsule.center *= heightCapsule;
			crouching = true;
		}
		else
		{
			capsule.height = capsuleHeight;
			capsule.center = capsuleCenter;
			crouching = false;
		}
	}

	private void handleGroundMovement()
	{
		if (isGround)
		{
			if (jump && !crouch && anim.GetCurrentAnimatorStateInfo(0).IsName("Ground"))
			{
				rig.velocity = new Vector3(rig.velocity.x, jumpPower, rig.velocity.z);
				isGround = false;
			}
		}
	}
	//
	private void handleJumpMovement()
	{
		if (!isGround)
		{
			Vector3 graviteForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
			rig.AddForce(graviteForce);
		}
	}

	// rotate the body
	private void applyExtraRotation()
	{
		float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, Mathf.Abs(forwardAmount));
		if (forwardAmount < 0.1f)
			turnSpeed *= 3;
		transform.Rotate(0, turnSpeed * turnAmount * Time.deltaTime, 0);

	}
	// to check the ground and get the normal vector on the ground
	private void checkGround()
	{
		RaycastHit hit;
		Vector3 postion = transform.position + new Vector3(0, 0.1f, 0);
		if (Physics.Raycast(postion, Vector3.down, out hit, checkGroundDistance))
		{
			groundNormal = hit.normal;
			isGround = true;    
		}
		else
		{
			groundNormal = Vector3.up;
			isGround = false;
		}
     
	}
	/*
    IEnumerator changeSpeed(float speed, out bool change)
    {
        change = true;
        Vector3 current = move;
        yield return new WaitForSeconds(0.2f);
        move *= speed;
        yield return new WaitForSeconds(5f);
        move = current;
        change = false;
        StopCoroutine("changeSpeed");
    }*/
}
