using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class shoot : NetworkBehaviour
{

	public GameObject cube;
	private Transform t;
	// Use this for initialization
	void Start()
	{
		t = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
			return;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFIRE();
		}
	}



	[Command]
	void CmdFIRE()
	{
		for (int i = 0; i < 10000; i++)
		{
			GameObject g = Instantiate(cube, t.position + new Vector3(20, 3, 10), t.rotation);
//		print("true");
			NetworkServer.Spawn(g);
		}
	}
}
