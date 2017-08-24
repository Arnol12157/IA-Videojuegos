using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeHive;

public class EnemyControllerBT : BeeHIVEAgent {

	Attack attack;
	Follow follow;

	// Use this for initialization
	void Start ()
	{
		InitBeeHIVE ();
		attack = GetComponent<Attack> ();
		follow = GetComponent<Follow> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public BH_Status followBT()
	{
		GetComponent<Renderer>().material.color = attack.normalColor;
		follow.followPlayer ();
		return BH_Status.Running;
	}

	public BH_Status attackBT()
	{
		attack.attackPlayer ();
		return BH_Status.Success;
	}

	public BH_Status isToFarBT()
	{
		if(Input.GetAxis("Horizontal")==0 || Input.GetAxis("Vertical")==0)
//		if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) > 2)
		{
			return BH_Status.Success;
		}
		return BH_Status.Failure;
	}
}
