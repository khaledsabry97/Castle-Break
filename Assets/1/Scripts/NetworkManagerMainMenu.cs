﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerMainMenu : NetworkManager
{

	/* void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            UIButton button = GameObject.Find("Host Game").GetComponent<UIButton>();
                    }
    }*/


	public void Host_Game()
	{
		NetworkManager.singleton.networkPort = 7777;
		//	NetworkManager.singleton.networkAddress = "127.0.0.1";
		NetworkManager.singleton.StartHost();

	}

	public void Join_Game()
	{
		NetworkManager.singleton.networkPort = 7777;
		NetworkManager.singleton.networkAddress = "127.0.0.2";
		NetworkManager.singleton.StartClient();
	}
}
