﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class NavMeshController : NetworkBehaviour
{
	NavMeshAgent nav;
	private Vector3 move;
	float turnAmount;
	float forwardAmount;
	private Animator anim;
	private Action action;
	private PlayerSound playerSound;
	private string petrolsound;
	private string chasingsound;


	public Action actionType
	{
		get{ return action; }
		set{ action = value; }
	}
	//private Action action;
	// Use this for initialization
	void Start()
	{
		nav = gameObject.GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		action = GetComponent<NavMeshMovement>().actionType;
		playerSound = GetComponent<PlayerSound>();
		petrolsound = "Petrol";
		chasingsound = "Chasing";
            
	}


	// Update is called once per frame
	void Update()
	{
		Move();
	}

	//public void RefreshAnimator()

	public void Move()
	{
		move = nav.desiredVelocity;
		if (move.magnitude > 0.1)
			move.Normalize();

		move = transform.InverseTransformDirection(move);
		//  move = Vector3.ProjectOnPlane(move, groundNormal);
		turnAmount = Mathf.Atan2(move.y, move.x);
		turnAmount = move.x;
		forwardAmount = move.z;
		applyExtraRotation();
		ChangeSpeed();
		anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
		//print("sdfsdf");
	}


	private void applyExtraRotation()
	{
		float turnSpeed = Mathf.Lerp(180, 220, Mathf.Abs(forwardAmount));
		if (forwardAmount < 0.1f)
			turnSpeed *= 3;
		transform.Rotate(0, turnSpeed * turnAmount * Time.deltaTime, 0);

	}


	void ChangeSpeed()
	{
		if (action == Action.petrol)
		{
			forwardAmount = Mathf.Clamp(forwardAmount, 0, 0.5f);
			playerSound.Play(petrolsound);
			playerSound.Stop(chasingsound);
		}
		else if (action == Action.chasing)
		{
			forwardAmount = Mathf.Clamp(forwardAmount, 0.5f, 1);
			playerSound.Stop(petrolsound);
			playerSound.Play(chasingsound);

		}
		else
		{
			if (playerSound.CheckAudioClip(petrolsound))
				playerSound.Stop(petrolsound);
			if (playerSound.CheckAudioClip(chasingsound))
				playerSound.Stop(chasingsound);

		}
	}
        


}
