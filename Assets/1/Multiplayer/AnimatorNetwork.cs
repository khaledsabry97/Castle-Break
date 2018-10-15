using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimatorNetwork : NetworkBehaviour
{
	void Start()
	{
		GetComponent<NetworkAnimator>().animator = gameObject.GetComponent<Animator>();

		for (int i = 0; i < gameObject.GetComponent<Animator>().parameters.Length; i++)
			GetComponent<NetworkAnimator>().SetParameterAutoSend(i, true);
	}

}
