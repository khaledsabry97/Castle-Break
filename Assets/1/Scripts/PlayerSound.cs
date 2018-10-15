﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour
{
   
	public List<Sound> Fx = new List<Sound>();
	Sound[] sound;
	// Use this for initialization
	void Start()
	{
		sound = SoundManager.instance.sound;
		AddSound();
		SoundManager.instance.AddAudioSource(gameObject, Fx);

	}

	void AddSound()
	{
		foreach (Sound s in sound)
		{
			if (s.Tag == gameObject.tag)
				Fx.Add(s);
		}
	}

	public void Play(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		if (s == null)
		{
			print("Couldn't find Clip : " + Name);
			return;
		}
		if (!s.Source.isPlaying)
			s.Source.Play();
	}

	public void PlayOneShot(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		if (s == null)
		{
			print("Couldn't find Clip : " + Name);
			return;
		}
		s.Source.Play();
	}



	public void Stop(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		if (s == null)
		{
			print("Couldn't find Clip : " + Name);
			return;
		}
		if (s.Source.isPlaying)
			s.Source.Stop();
	}

	public bool isPlaying(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		if (s == null)
		{
			print("Couldn't find Clip : " + Name);
			return false;
		}
		return  s.Source.isPlaying;
	}

	public bool CheckAudioClip(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		if (s == null)
		{
			print("Couldn't find Clip : " + Name);
			return false;
		}
		else
			return true;
	}
}
