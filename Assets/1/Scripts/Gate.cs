using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
	
	void OnTriggerEnter(Collider hit)
	{

		if (hit.gameObject.CompareTag("Player"))
		{
			if (hit.gameObject.GetComponent<PlayerMovement>().Multiplayer)
			{
				hit.gameObject.GetComponent<PlayerFirstEscape>().IsEscaped = true;
				GameManagerMultiplayer.instance.Close_Door();
				gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			else
			{
				GameManager.instance.IsEscaped = true;
				GameManager.instance.Close_Door();
				gameObject.GetComponent<BoxCollider>().enabled = false;
			}
		}
	}
 
}
