using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//using System.IO.Ports;
using UnityEngine.Networking.Types;
using UnityEngine.Networking;

public class GameManagerMultiplayer : NetworkBehaviour
{

	public static GameManagerMultiplayer instance = null;

	/*Cameras*/
	[SerializeField]
	private GameObject CameraWinning;

	[SerializeField]
	private GameObject CameraCollected;

	public bool Multiplayer = false;

	//Private Variables
	public GameObject player;
	// the player with tag "Player"
	private GameObject[] Guards;
	// Castle guards that protect the castle with tag "Guard"
	private GameObject[] items;
	// Items to collect from the castle with tag "Item"
	public int num_of_items;
	// number of items in the castle
	private GameObject door;
	// Door of the castle with tag "Door"
	public GameObject[] particale;
	// fireworks that play when you win with tag "WinningParticle";
	private GameObject[] obj;

	public UILabel timerLabel;
	public UILabel CountRemainningItemsLabel;
	private bool gameStarted;
	//Check if You pressed a key at the start of your game or not
	public int defaultTime = 60;
	//the started time by default
	[SyncVar]
	public float time;
	// time clock for your game
	public GameObject presstoplaySprite;
	// the screen at the beginning of your game
	public GameObject[] Players;



	//this is shown at the end of the game
	public GameObject timerSprite;
	public GameObject endSprite;
	public UILabel collectedItems;
	public UILabel sectoken;
	public UILabel numberofattentions;
	public int numofattention = 0;


	public GameObject[] hideThingsAtFirst;
	public Color start;
	public Color end;
	/*
	public int Numofattention
	{
		get{ return numofattention; }
		set{ numofattention = value; }
	}
    */

	public bool GameStarted
	{
		get{ return gameStarted; }
		set
		{
			gameStarted = value;
			if (gameStarted)
			{
				foreach (GameObject m in hideThingsAtFirst)
				{
					m.SetActive(true);
				}
			}

		}
	}


	void Awake()
	{
		//Multiplayer = GameObject.Find("NetworkManager");
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}


	// Use this for initialization
	void Start()
	{
		initialization();
	}

	void initialization()
	{
		foreach (GameObject g in Players)
		{
			if (g.name == ApplicationManager.instance.Charactername)
			{
				NetworkManager.singleton.playerPrefab = g;
				break;
			}
		}

		Guards = GameObject.FindGameObjectsWithTag("Guard");
		door = GameObject.FindGameObjectWithTag("Door");
		items = GameObject.FindGameObjectsWithTag("Item");

		foreach (GameObject m in particale)
			m.SetActive(false);

		CameraWinning.SetActive(false);
		GameStarted = true;
		timerSprite.GetComponentInChildren<UILabel>().text = defaultTime.ToString();
		defaultTime = 60;
		CountRemainningItemsLabel.text = num_of_items.ToString();
	}

	void Update()
	{   
		presstoplaySprite.SetActive(false);
		StopAllCoroutines();
		time = defaultTime;
		//StartCoroutine("Rpctimer");

	}

   

	[ClientRpc]
	void Rpctimer()
	{
		timerLabel.text = (time).ToString();
		if (time == 0)
		{
			time = 0;
		}

		time--;
		Invoke("Rpctimer", 1);

	}

	#region Door

	public void Open_Door()
	{
		door.gameObject.GetComponent<Animator>().SetTrigger("Open_Door");

	}

	public void Close_Door()
	{
		door.GetComponent<Animator>().SetTrigger("Close_Door");
	}

	#endregion

	public	void GameEnded()
	{
		foreach (GameObject m in hideThingsAtFirst)
			m.SetActive(false);
		StopCoroutine("timer");
		timerSprite.SetActive(false);
		endSprite.SetActive(true);
		sectoken.text = (defaultTime - time).ToString();
		numberofattentions.text = numofattention.ToString();
	}






}
