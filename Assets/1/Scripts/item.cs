﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class item : NetworkBehaviour
{

	void OnTriggerEnter(Collider hit)
	{
		//if (GameManagerMultiplayer.instance != null)
		if (hit.tag == "Player")
		{
			if (hit.gameObject.GetComponent<PlayerMovement>().Multiplayer)
			{
				hit.gameObject.GetComponent<PlayerFirstEscape>().Collect_Item = hit.gameObject.GetComponent<PlayerFirstEscape>().num_of_collected_items + 1;
				CmdDestroyObject();
			}
			else
			{
				GameManager.instance.collect_item();
				Destroy(gameObject);
			}
		}
	}

	[Command]
	void CmdDestroyObject()
	{
		NetworkServer.Destroy(this.gameObject);
        
	}
}
