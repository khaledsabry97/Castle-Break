using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Configuration;
using UnityEngine.Networking.Types;
using UnityEngine.Experimental.UIElements;

public class Options : MonoBehaviour
{


	public void SoundSlider()
	{  
		SoundManager.instance.setVolumeOfAudioSource(UIProgressBar.current.value);
	}

	public void MuteSound()
	{
		SoundManager.instance.muteSound(UIToggle.current.value);
	}

	public void MusicList()
	{
		SoundManager.instance.Play_Main_Music(UIPopupList.current.value);
	}

	public void New_Game()
	{
		ApplicationManager.instance.changeScene("Level_One");
	}

	public void Exit()
	{
		ApplicationManager.instance.Exit();
	}

	public void MainMenu()
	{
		ApplicationManager.instance.changeScene("Main_Menu");
	}

	public void ChangeCharacter()
	{
		ApplicationManager.instance.changeScene("ChangeCharacter");
	}

	public void Option()
	{
		ApplicationManager.instance.changeScene("Options");
	}


	public void FullScreen()
	{
		ApplicationManager.instance.FullScreen(UIToggle.current.value);
	}

	public void Resolution()
	{
		string currentresolution = UIPopupList.current.value;
		string height = "";
		string width = "";
		bool w = true;
		for (int i = 0; i < currentresolution.Length; i++)
		{
			if (currentresolution[i] != 'x' && w)
			{
				width += currentresolution[i];
				continue;
			}
			else
				w = false;
			if (currentresolution[i] == 'x')
				i++;
            
			if (currentresolution[i] != '@' && !w)
			{
				height += currentresolution[i];
			}
			else
				break;
		}
		int x = int.Parse(width);
		int y = int.Parse(height);
		Resolution r = Screen.currentResolution;
		r.width = x;
		r.height = y;
		Screen.SetResolution(r.width, r.height, Screen.fullScreen);
	}
}
