using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Timer : MonoBehaviour
{
	public UILabel TimeTxt;

  
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		TimeTxt.text = GameObject.Find("GameManager").GetComponent<GmMgNt>().timer.ToString("#");
	}



}
