using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Policy;
using NUnit.Framework.Constraints;
using UnityEngine.Analytics;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Runtime.Remoting.Messaging;
using UnityEngine.SceneManagement;

public class GameManager_RoundWinner : MonoBehaviour
{
	[Header("Camera")]
	public GameObject CameraMain;
	public GameObject CameraWinning;

	[Header("Players&Agent")]
	public GameObject[] Players;
	public List<GameObject> guards = new List<GameObject>();
	private  GameObject[] guardsLocations;
	private GameObject player;
	private List<GameObject> InstaniatedAgents = new List<GameObject>();
	private GameObject door;
	public GameObject[] items;
	

	[Header("Time")]
	public int noOfAgents;
	
	public float time;
	public float StartingTime;
	private bool Gamestarted;
	public int ItemsCount;

	[Header("Items On Screen")]
	public GameObject EndScreen;
	public GameObject GameRunning;
	public UILabel textCurrentCollectedItems;
	public UILabel CollecteditemsText;
	public UILabel NumOfTimeAttentionGuards;
	public UILabel timer;
	public UILabel TimeToEscape;
	public UILabel Result;



	// Use this for initialization
	void Start()
	{
		items = GameObject.FindGameObjectsWithTag("Item");
		ItemsCount = items.Length;
		guardsLocations = GameObject.FindGameObjectsWithTag("GuardLocation");
		StartingTime = time;
		door = GameObject.FindGameObjectWithTag("Door");
		SpawnAgents(noOfAgents);
		Set_Player();
		
		StartGame();
	}

	//set the main player
	void Set_Player()
	{
		for (int i = 0; i < Players.Length; i++)
		{
			if (Players[i].name == ApplicationManager.instance.Charactername)
			{
				{
					GameObject k = Instantiate(Players[i].gameObject, GameObject.Find("Spot1").GetComponent<Transform>().position, GameObject.Find("Spot1").GetComponent<Transform>().rotation);
					player = k;
					break;
				}
			}
		}
            
	}
	// Update is called once per frame
	void Update()
	{
		//check the game state
		CheckGameState();
		//update the timer
		UpdateIcons();
	}

	void CheckGameState()
	{
		//if the player collected all the items then open the door
		if (items.Length == 0)
		{
			Open_Door();
		}

		//if the player got throght the door so the player won
		if (door.GetComponent<Door>().gotplayerthrough)
		{
			ShowEnd("Winner");
		}

		// if the player dead then show loser end
		if (player.GetComponent<PlayerHealth>().dead)
		{
			ShowEnd("Loser");
		}




	}

	//spawn agents in random
	void SpawnAgents(int number)
	{
		//this list to ensure that every agent will spawn in different areas
		List<GameObject> Locations = new List<GameObject>();
		Locations.AddRange(guardsLocations);
		for (int k = 0; k < number; k++)
		{
			int i = Random.Range(0, 1000) % guards.Count;
			int j = Random.Range(0, 1000) % Locations.Count;
			GameObject G = Instantiate(guards[i], Locations[j].transform.position, Locations[j].transform.rotation);
			Locations.RemoveAt(j);
			InstaniatedAgents.Add(G);
		}
	}

	void StartGame()
	{
		GameRunning.SetActive(true);

		UpdateMovementAllowed(true);
	}

	void UpdateIcons()
	{
		time -= Time.deltaTime;
		timer.text = time.ToString("#");
		if (time <= 0)
			ShowEnd("TimeEnded");
		items = GameObject.FindGameObjectsWithTag("Item");
		textCurrentCollectedItems.text = (items.Length).ToString("#");

	}

	public void Open_Door()
	{
		door.GetComponent<Animator>().SetTrigger("Open_Door");
	}

	public void Close_Door()
	{
		door.GetComponent<Animator>().SetTrigger("Close_Door");
	}

	void UpdateMovementAllowed(bool State)
	{
		player.GetComponent<PlayerMovement>().MovementAllowed = State;
		foreach (GameObject g in InstaniatedAgents)
			g.GetComponent<NavMeshMovement>().MovementAllowed = State;
	}

	void ShowEnd(string State)
	{
		UpdateMovementAllowed(false);
		CameraWinning.SetActive(true);
		CameraMain.SetActive(false);
		EndScreen.SetActive(true);
		GameRunning.SetActive(false);
		CollecteditemsText.text = (ItemsCount - items.Length).ToString();
		NumOfTimeAttentionGuards.text = GetAgentsAttentions().ToString();
		TimeToEscape.text = (StartingTime - time).ToString("#");
		Result.text = State;
	}

	int GetAgentsAttentions()
	{
		int sum = 0;
		foreach (GameObject g in guards)
			sum += g.GetComponent<NavMeshMovement>().Attention;
		return sum;
        
	}

}
