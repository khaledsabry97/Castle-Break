using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Door : MonoBehaviour
{

	public bool gotplayerthrough;
	private bool Multiplayer;


	void Start()
	{
		if (GameObject.Find("NetworkManager") == null)
			Multiplayer = false;
		else
			Multiplayer = true;
		print("j");
	}

	void OnTriggerEnter(Collider collision)
	{
        
		if (collision.gameObject.tag == "Player")
		{
			if (Multiplayer)
				collision.gameObject.GetComponent<PlayerFirstEscape>().IsEscaped = true;
			else
			{
				gotplayerthrough = true;
			}
		}

	}
}
