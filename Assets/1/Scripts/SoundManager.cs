using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	public Sound[] sound;
	//public Sound[] Game_Music;
	//Sound is mute or not
	public  bool Mute;
	// the volume of the sound
	public  float volume;
	public string MainClipRunning;
	// the clip that plays at the background
	//	public AudioClip Main_Sound;
	// refrence to audiosource from application manager
	//	private AudioSource audioSource;
	// refrence to application manager
	//private ApplicationManager app;

	// the clips at music list
	//public AudioClip[] audioclips;
	// refrence to mute icon
	private UIToggle MuteIcon;
	// reference to volume bar
	private UIProgressBar volumeBar;
	private UIPopupList Music_List;
	// music list with key by name and its clip
	//	Dictionary<string,AudioClip> musicList = new Dictionary<string, AudioClip>();

	void Awake()
	{


		if (instance == null)
			instance = this;
		else if (instance != this)
		{
			Destroy(this.gameObject);
		}


		DontDestroyOnLoad(gameObject);


		foreach (Sound s in sound)
		{
			if (s.Tag == "MainMusic")
			{
				s.Source = gameObject.AddComponent<AudioSource>();
				s.Source.clip = s.Clip;
				s.Source.volume = s.Volume;
				s.Source.spatialBlend = s.SpatialBlend;
				s.Source.loop = s.Loop;
				s.Source.playOnAwake = s.PlayOnAwake;
			}

			if (s.Clip != null)
			{
				s.Name = s.Clip.name;

			}

		}
           
		initial();

	}

	void initial()
	{
		if (GameObject.FindGameObjectWithTag("Mute"))
			MuteIcon = GameObject.FindGameObjectWithTag("Mute").GetComponent<UIToggle>();
		if (GameObject.FindGameObjectWithTag("Volume"))
			volumeBar = GameObject.FindGameObjectWithTag("Volume").GetComponent<UIProgressBar>();
		if (GameObject.FindGameObjectWithTag("Music_List"))
			Music_List = GameObject.FindGameObjectWithTag("Music_List").GetComponent<UIPopupList>();
		if (volumeBar != null)
			volumeBar.value = SoundManager.instance.volume;

		if (MuteIcon != null)
			MuteIcon.value = SoundManager.instance.Mute;

		if (Music_List != null)
		{
			Music_List.items.Clear();
			foreach (Sound s in sound)
			{
				if (s.Tag == "MainMusic")
					Music_List.items.Add(s.Name);
			}

			Music_List.GetComponentInChildren<UILabel>().text = MainClipRunning;

		}
        
	}

	void Start()
	{
		volume = 0.5f;
		Mute = false;
		Play_Main_Music("The Forest");

		//	app = GameObject.Find("ApplicationManager").GetComponent<ApplicationManager>();
		//	audioSource = app.GetComponent<AudioSource>();
		//AddToMusicList();
		//SetMusic();
		//	setVolumeOfAudioSource(volume);
	}


	/*
	public void Play(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		s.Source.Play();
	}

	public void Stop(string Name)
	{
		Sound s = Array.Find(sound, sound => sound.Name == Name);
		s.Source.Stop();
	}

    */

	public void Play_Main_Music(string Name)
	{
		foreach (Sound s in sound)
		{
			if (s.Tag == "MainMusic")
				s.Source.Stop();
		}
		Sound sd = Array.Find(sound, sound => sound.Name == Name);
		MainClipRunning = sd.Name;
		sd.Source.volume = sd.Volume * volume;
		sd.Source.Play();
	}


	//set the volume of the music
	public void setVolumeOfAudioSource(float MusicVolume)
	{     
		volume = MusicVolume;

		foreach (Sound s in sound)
		{
			if (s.Tag == "MainMusic")
				s.Source.volume = s.Volume * volume;
		}

	}

	public void muteSound(bool mute)
	{

		Mute = mute;
		foreach (Sound s in sound)
		{
			if (s.Tag == "MainMusic")
				s.Source.mute = mute;
		}

	}

	public void AddAudioSource(GameObject game, List<Sound> Music_List)
	{
		foreach (Sound s in Music_List)
		{
			s.Source = game.AddComponent<AudioSource>();
			s.Source.clip = s.Clip;
			s.Name = s.Clip.name;
			s.Source.volume = s.Volume * volume;
			s.Source.spatialBlend = s.SpatialBlend;
			s.Source.loop = s.Loop;
			s.Source.playOnAwake = s.PlayOnAwake;
			if (s.Mute || Mute)
				s.Source.mute = true;


		}
	}
}
