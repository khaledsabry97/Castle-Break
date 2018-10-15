using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEditor.VersionControl;

public class ChangeCharacter : MonoBehaviour
{
	public static ChangeCharacter instance;
	private GameObject[] obj;
	private GameObject[] styleobj;
	int i = 0;
	// Use this for initialization
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
		{
			Destroy(this.gameObject);
		}
	}

	void Start()
	{
		i = ApplicationManager.instance.Characternum;
		obj = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject g in obj)
			g.SetActive(false);
		obj[i].SetActive(true);
	}

	public void ChangeRight()
	{
		obj[i].SetActive(false);  
		if (i == obj.Length - 1)
			i = 0;
		else
			i++;
		obj[i].SetActive(true);
		ApplicationManager.instance.Characternum = i;
		ApplicationManager.instance.Charactername = obj[i].name;
	}

	public void ChangeLeft()
	{
		obj[i].SetActive(false);  
       
		if (i == 0)
			i = obj.Length - 1;
		else
			i--;
		obj[i].SetActive(true);
		ApplicationManager.instance.Characternum = i;
	}

}
