using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

	public float speedAI;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void followPlayer()
	{
		transform.position = Vector3.MoveTowards (transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, Time.deltaTime * speedAI);
		transform.LookAt (GameObject.FindGameObjectWithTag("Player").transform.position);
		Debug.Log ("Siguiendo");
		Debug.Log (Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag ("Player").transform.position));
	}
}
