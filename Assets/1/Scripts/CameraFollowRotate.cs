﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class CameraFollowRotate : MonoBehaviour
{

	[SerializeField]
	private Transform target;

	[SerializeField]
	private Vector3 offsetPosition;

	[SerializeField]
	private Vector3 currentVelocity;

	[SerializeField]
	private float smoothness;

	[SerializeField]
	private float rotationSmoothness;

	[SerializeField]
	private Space offsetPositionSpace = Space.Self;

	[SerializeField]
	private bool lookAt = true;

	void Start()
	{
	}

	private void FixedUpdate()
	{
		if (target == null)
		{
			target = GameManager.instance.player.GetComponent<Transform>();
			if (target != null)
				offsetPosition = transform.position - target.position;
			else
			{
				return;
			}
		}

		if (!GameManager.instance.IsEscaped)
		{
			// compute position
			if (offsetPositionSpace == Space.Self)
			{
				transform.position = Vector3.Lerp(transform.position, target.TransformPoint(offsetPosition), 0.05f);
			}
			else
			{
				transform.position = target.position + offsetPosition;
			}

			// compute rotation
			if (lookAt)
			{
				transform.LookAt(target);
			}
			else
			{
				transform.rotation = target.rotation;
			}
		}


	}

}
