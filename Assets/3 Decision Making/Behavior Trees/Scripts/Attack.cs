using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public Color alertColor;
	public float changeRate;
	public Color normalColor;
	private float addedTime;

	// Use this for initialization
	void Start ()
	{
		normalColor = GetComponent<Renderer>().material.color;
		addedTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void attackPlayer()
	{
		addedTime += Time.deltaTime;
		if (addedTime >= changeRate)
		{
			if (GetComponent<Renderer>().material.color.Equals(normalColor))
			{
				GetComponent<Renderer>().material.color = alertColor;
			}
			else
			{
				GetComponent<Renderer>().material.color = normalColor;
			}
			addedTime = 0;
		}
	}
}
