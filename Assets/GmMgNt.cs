﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GmMgNt : NetworkBehaviour
{

	[SyncVar]
	public float timer = 60f;
	public GameObject Guardian;
	public Transform[] GuardiansSpots;
	public int j = 0;

	private void OnStartServer()
	{

		Spawn();
	}


	void Spawn()
	{
		if (!isServer)
			return;

		int i = UnityEngine.Random.Range(0, GuardiansSpots.Length);
		GameObject e = Instantiate(Guardian, GuardiansSpots[i].position, GuardiansSpots[i].rotation);
		//i = UnityEngine.Random.Range(0, Players.Count);
		//e.GetComponent<NavMeshMovement>().Player = Players[i];
		NetworkServer.Spawn(e);
		j++;
		if (j < 3)
			Invoke("Spawn", 1);
	}
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!isServer)
			return;
		timer -= Time.deltaTime;
		if (timer < 0)
			timer = 0;
	}
}
