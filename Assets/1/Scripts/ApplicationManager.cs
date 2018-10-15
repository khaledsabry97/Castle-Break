﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//using System.Diagnostics.PerformanceData;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using System.Security.Cryptography;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour
{

	private GameObject Close_Scene;
	private GameObject Opening_Scene;
	static	public bool isFull = false;
	static private	List<string> resolutionstrings = new List<string>();
	public static ApplicationManager instance = null;
	private int characternum;
	public string charactername;
	//private int characterstyle;

	public int Characternum
	{
		set{ characternum = value; }
		get{ return characternum; }
	}

	public string Charactername
	{
		set{ charactername = value; }
		get{ return charactername; }
	}

	/*public int Characterstyle
	{
		set{ characterstyle = value; }
		get{ return characterstyle; }
	}
*/
	void Awake()
	{
		if (Charactername == "")
			Charactername = "Character_Viking_White";
		LoadedSCene();
		if (instance == null)
			instance = this;
		else if (instance != this)
		{
			Destroy(this.gameObject);

		}
		DontDestroyOnLoad(gameObject);


	}

	void Start()
	{
		Resolution();
		Screen.fullScreen = isFull;
		Characternum = 0;

	}

	void  LoadedSCene()
	{
		if (Opening_Scene == null)
		{
			Opening_Scene = GameObject.Find("O/C IMAGE");
		}
		Opening_Scene.GetComponent<Animator>().SetTrigger("Opening_Scene");

		OptionScene();
	}

	public void OptionScene()
	{
		
		if (GameObject.FindGameObjectWithTag("Resolutions"))
			addResolution(GameObject.FindGameObjectWithTag("Resolutions"));
		if (GameObject.FindGameObjectWithTag("FullScreen"))
			GameObject.FindGameObjectWithTag("FullScreen").GetComponent<UIToggle>().value = isFull;
	}

	public void Move_To_level_One()
	{
		changeScene("Level_One");
	}

	public void Exit()
	{
		Application.Quit();
	}

	#region Change Scene

	public void changeScene(string sceneName)
	{
		if (Close_Scene == null)
		{
			Close_Scene = GameObject.Find("O/C IMAGE");
		}
		Close_Scene.GetComponent<Animator>().SetTrigger("Closing_Scene");
		StartCoroutine("CHANGE_SCENE", sceneName);
	}

	IEnumerator CHANGE_SCENE(string name)
	{
		yield return new WaitForSeconds(0.75f);
		SceneManager.LoadScene(name, LoadSceneMode.Single);
        
	}

	#endregion

	#region Resolution

	public void FullScreen(bool value)
	{
		Screen.fullScreen = isFull = value;
	}

	public void Resolution()
	{
		Resolution[] resolution = Screen.resolutions;

		for (int i = resolution.Length - 1; i >= resolution.Length - 5; i--)
		{
			if (resolutionstrings.Count > 5)
				break;
			resolutionstrings.Add(resolution[i].width
				+ "x" + resolution[i].height);
		}
	}

	
	public void addResolution(GameObject obj)
	{
		
		UIPopupList current = obj.GetComponent<UIPopupList>();
		if (current.value != "")
			return;
		current.Clear();
		current.items = resolutionstrings;
		string res = Screen.width.ToString() + "x" + Screen.height.ToString();
		print(res);
		current.value = res;
	}

	#endregion
}
