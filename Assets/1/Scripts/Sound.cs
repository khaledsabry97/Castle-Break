using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Analytics;

[System.Serializable]
public class Sound
{

	public string Name;
	public AudioClip Clip;
	public string Tag;
	[Range(0f, 1f)]
	public float Volume;

	[Range(0f, 1f)]
	public float SpatialBlend;

	[HideInInspector]
	public AudioSource Source;

	public bool Mute;

	public bool Loop;

	public bool PlayOnAwake;
   


   
}
