using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
	public GameObject Shoulder_R;


	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}


	public void beginFight()
	{
		if (Shoulder_R != null)
			Shoulder_R.GetComponent<CapsuleCollider>().enabled = true;
	}

	public void EndFight()
	{
		if (Shoulder_R != null)
			Shoulder_R.GetComponent<CapsuleCollider>().enabled = false;
	}
}
