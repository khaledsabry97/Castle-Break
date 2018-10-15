﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	public GameObject Player;
	public float rotx = 45f;
	public float roty = 45f;
	public float dis = 40;
	private float MaxRot = 55;
	private float MinRot = 5;
	private float MaxDis = 13;
	private float MinDis = 3;

	// Use this for initialization
	void Start()
	{
		//Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
	{
		rotx += Input.GetAxis("Mouse X");
		roty += Input.GetAxis("Mouse Y");
		dis -= Input.GetAxis("Mouse ScrollWheel");
		roty = Mathf.Clamp(roty, MinRot, MaxRot);
		dis = Mathf.Clamp(dis, MinDis, MaxDis);

	}


	void LateUpdate()
	{
		if (Player == null)
		{
			Player = GameObject.FindWithTag("Player");
			return;
		}
			
		Vector3 place = new Vector3(0, 0, -dis);
		Quaternion rotation = Quaternion.Euler(roty, rotx, 0);
		transform.position = Player.transform.position + rotation * place;
		transform.LookAt(Player.transform.position);
	}
}
