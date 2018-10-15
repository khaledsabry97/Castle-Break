﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//using System.IO.Ports;
using UnityEngine.Networking.Types;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

	public static GameManager instance = null;

	/*Cameras*/
	[SerializeField]
	private GameObject CameraWinning;
  
	[SerializeField]
	private GameObject CameraCollected;

	//for debug process
	public bool cought;
	public bool isEscaped;
	public bool isCollected;

	public bool Multiplayer = false;




	public bool MovementAllowed;


	//Private Variables
	public GameObject player;
	// the player with tag "Player"
	private GameObject[] Guards;
	// Castle guards that protect the castle with tag "Guard"
	private GameObject[] items;
	// Items to collect from the castle with tag "Item"
	public int num_of_collected_items;
	// numbers of items you have collected
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
	private float time;
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
	private int numofattention = 0;


	public GameObject[] hideThingsAtFirst;
	public Color start;
	public Color end;

	public int Numofattention
	{
		get{ return numofattention; }
		set{ numofattention = value; }
	}

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

	public bool IsCollected
	{
		get { return isCollected; } 
		set
		{
			isCollected = value;
			if (isCollected)
				Open_Door();
		}
	}

	public bool IsEscaped
	{
		get{ return isEscaped; }
		set
		{
			isEscaped = value;
			if (isEscaped)
				StartCoroutine("Won");
			print(isEscaped);
            
		}
	}


	void Awake()
	{
		//player = GameObject.Find(ApplicationManager.instance.Charactername);

		//player.SetActive(true);
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			//  Destroy(gameObject);
		}
		//initialization();
		//  DontDestroyOnLoad(gameObject);


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
				GameObject k = GameObject.Instantiate(g);
				k.GetComponent<Transform>().position = GameObject.Find("Spot1").GetComponent<Transform>().localPosition;
				k.SetActive(true);
				player = k;
				Camera.main.GetComponent<Main_Camera>().target = player.transform;
				break;
			}
		}
		Guards = GameObject.FindGameObjectsWithTag("Guard");
		door = GameObject.FindGameObjectWithTag("Door");
		items = GameObject.FindGameObjectsWithTag("Item");

		foreach (GameObject m in particale)
			m.SetActive(false);
		foreach (GameObject m in hideThingsAtFirst)
		{
			m.SetActive(false);
		}
		num_of_items = items.Length;
		num_of_collected_items = 0;
		isCollected = false;
		isEscaped = false;
		cought = false;
		CameraWinning.SetActive(false);
		gameStarted = false;
		timerSprite.GetComponentInChildren<UILabel>().text = defaultTime.ToString();
		MovementAllowed = false;
		numofattention = 0;
		defaultTime = 60;
		CountRemainningItemsLabel.text = num_of_items.ToString();
	}

	void Update()
	{   

		if (!gameStarted)
		{
			PressToPlay();
			return;
		}


		if (Input.GetKey(KeyCode.A))
		{
			player.SetActive(true);
		}
	}

	void PressToPlay()
	{
		if (Input.GetKey(KeyCode.X))
		{
			GameStarted = true;
			MovementAllowed = true;
			presstoplaySprite.SetActive(false);
			StopAllCoroutines();
			time = defaultTime;
			StartCoroutine("timer");
		}
		
	}

	IEnumerator timer()
	{
		timerLabel.text = (time).ToString();
		if (time == 0)
			GameEnded();
		yield return new WaitForSeconds(1f);
		time--;


		StartCoroutine("timer");

	}

	public void collect_item()
	{
		num_of_collected_items++;
		CountRemainningItemsLabel.text = (num_of_items - num_of_collected_items).ToString();
		if (num_of_collected_items == num_of_items)
			IsCollected = true;
	}

	public void Open_Door()
	{
		door.gameObject.GetComponent<Animator>().SetTrigger("Open_Door");
	}

	public void Close_Door()
	{
		door.GetComponent<Animator>().SetTrigger("Close_Door");
	}

   
	IEnumerator Won()
	{
		StopCoroutine("timer");
		foreach (GameObject m in particale)
		{
			m.SetActive(true);
		}
		CameraWinning.SetActive(true);
		yield return new WaitForSeconds(1);
		GameEnded();
		foreach (GameObject en in Guards)
		{
			en.GetComponent<NavMeshMovement>().ChangeState(Action.Idle);
		}

		yield return new WaitForSeconds(10);


		StopCoroutine("Won");
	}

	void GameEnded()
	{
		MovementAllowed = false;
		foreach (GameObject m in hideThingsAtFirst)
			m.SetActive(false);
		StopCoroutine("timer");
		timerSprite.SetActive(false);
		endSprite.SetActive(true);
		collectedItems.text = num_of_collected_items.ToString();
		sectoken.text = (defaultTime - time).ToString();
		numberofattentions.text = numofattention.ToString();
	}






}
