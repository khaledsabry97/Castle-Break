﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using AnimationOrTween;
using System.Net.NetworkInformation;

public class PlayerFirstEscape : NetworkBehaviour
{
	static GameManagerMultiplayer gamemanager;
	[SyncVar]
	public bool winner = false;
	public GameObject endscreen;

	/*Cameras*/
	[SerializeField]
	private GameObject CameraWinning;

	[SerializeField]
	private GameObject CameraCollected;

	public bool cought;
	public bool isEscaped;
	public bool isCollected;

	private GameObject[] items;
	// Items to collect from the castle with tag "Item"
	public int num_of_collected_items;
	// numbers of items you have collected
	public int num_of_items;
	// number of items in the castle
   
	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Camera.main.GetComponent<Main_Camera>().target = this.gameObject.transform;
		//Camera.main.GetComponent<MainCamera>().Player = this.gameObject;

		//GameObject.Find("NetworkManager").GetComponent<Network>().Players.Add(this.gameObject);
	}


	public int Collect_Item
	{
		set
		{
			num_of_collected_items = value;
			if (num_of_collected_items == num_of_items)
				IsCollected = true;
		}
	}

	public bool IsCollected
	{
		get { return isCollected; } 
		set
		{
			isCollected = value;
			if (isCollected)
				RpcOpen_Door();
		}
	}


	public bool IsEscaped
	{
		get{ return isEscaped; }
		set
		{
			isEscaped = value;
			winner = value;
			if (isEscaped)
				RpcEndGame();
            

		}
	}
	// Use this for initialization
	void Start()
	{
		items = GameObject.FindGameObjectsWithTag("Item");
		//Camera.main.GetComponent<MainCamera>().Player = this.gameObject;
		//gamemanager = GameObject.Find("GameManager").GetComponent<GameManagerMultiplayer>();
		num_of_items = items.Length;
		//num_of_collected_items = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
			return;
	}

	/*
	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Door")
		{
			IsEscaped = true;
			RpcEndGame();
		}
           
	}
*/
	[ClientRpc]
	public void RpcOpen_Door()
	{
		if (isLocalPlayer)
		{
			gamemanager.Open_Door();
		}

		//RpcEndGame();
	}


	void exit()
	{
		ApplicationManager.instance.Exit();
	}

	[ClientRpc]
	void RpcEndGame()
	{
		GameObject g = Instantiate(endscreen);
		NetworkServer.Spawn(g);
		if (isLocalPlayer)
		{
			StartCoroutine("Won");
			gamemanager.collectedItems.text = num_of_collected_items.ToString();
			g.GetComponent<UILabel>().text = "winner";
		}
		else
		{
			StartCoroutine("Lost");
			gamemanager.collectedItems.text = num_of_collected_items.ToString();
			g.GetComponent<UILabel>().text = "loser";
		}


		
	}


	public  IEnumerator Won()
	{
		StopCoroutine("timer");
		foreach (GameObject m in gamemanager.particale)
		{
			m.SetActive(true);
		}
		CameraWinning.SetActive(true);
		yield return new WaitForSeconds(1);
		GameEnded();

		/*
        foreach (GameObject en in Guards)
        {
            en.GetComponent<NavMeshMovement>().ChangeState(Action.Idle);
        }
*/
		yield return new WaitForSeconds(10);

		exit();
		StopCoroutine("Won");
	}

	public  IEnumerator Lost()
	{
		StopCoroutine("timer");
		CameraWinning.SetActive(true);
		yield return new WaitForSeconds(1);
		GameEnded();
		yield return new WaitForSeconds(10);
		exit();
		StopCoroutine("Lost");
	}

	public  void GameEnded()
	{
		foreach (GameObject m in gamemanager.hideThingsAtFirst)
			m.SetActive(false);
		//	StopCoroutine("timer");
		gamemanager.timerSprite.SetActive(false);
		gamemanager.endSprite.SetActive(true);
		gamemanager.sectoken.text = (gamemanager.defaultTime - gamemanager.time).ToString();
		gamemanager.numberofattentions.text = gamemanager.numofattention.ToString();
	}
}
