using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

	public bool dead;
	private Animator anim;
	private CapsuleCollider cp;
	private PlayerSound playerSound;
	private string Hit;

	public bool Dead
	{
		get{ return dead; }
		set
		{
			dead = value;
			if (dead)
			{
				//cp.isTrigger = true;
			}
		}
	}
	// Use this for initialization
	void Start()
	{
		Dead = false;
		anim = GetComponent<Animator>();
		cp = GetComponent<CapsuleCollider>();
		playerSound = GetComponent<PlayerSound>();
		Hit = "Hit";
		
	}

	void OnCollisionEnter(Collision c)
	{
        
		if ((c.gameObject.name == "Shoulder_R" || c.gameObject.name == "Shoulder_L") && !dead)
		{
			print("damn2");
			anim.SetTrigger("Death");
			playerSound.Play(Hit);
		}
	}
	// Update is called once per frame
	void Update()
	{
		
	}

	public void Die()
	{
		print("damn");
		Dead = true;

	}
}
