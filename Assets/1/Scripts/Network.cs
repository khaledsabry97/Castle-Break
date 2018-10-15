using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.AI;

//using UnityEditor.VersionControl;

public class Network : NetworkBehaviour
{
	/*
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject pla = GameObject.Instantiate(playerPrefab);
        GameManager.instance.player = pla;
        NetworkServer.AddPlayerForConnection(conn, pla, playerControllerId);
    }
    */
	// Use this for initialization
	public GameObject Guardian;
	public Transform[] GuardiansSpots;
	public int j = 0;
	float timer;
	//public List<GameObject> Players = new List<GameObject>();

	void Start()
	{
		
	}

	public override void OnStartServer()
	{
		//base.OnStartServer();
//		print("Hello");
		Invoke("CmdFire", 0.5f);
		timer = 0;
	}

	// Update is called once per frame
	void Update()
	{
		/*
		timer += Time.deltaTime;
		if (timer > 2)
		{
			CmdFire();
			timer = 0;
		}
        */
		//if (NetworkServer.active)
		//Invoke("CmdFire", 2);
	}

	void CmdFire()
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
			Invoke("CmdFire", 1);
	}

	/*
	public void Random_Objects(List<GameObject> OBJS, GameObject Distenation)
	{
		int i = UnityEngine.Random.Range(0, OBJS.Count);
		Distenation = OBJS[i];
	}*/


}
