﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class cube : NetworkBehaviour
{

	float time = 3;
	float timerunning = 0;
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	[ServerCallback]
	void Update()
	{
		timerunning += Time.deltaTime;
		//if (timerunning > time)
		//NetworkServer.Destroy(gameObject);
	}
}
