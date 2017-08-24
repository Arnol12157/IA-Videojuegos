using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public float speed;
	public NavMeshAgent agent;
	public GameObject player;

	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		agent.destination = GameObject.FindGameObjectWithTag ("Player").transform.position;
//		agent.SetDestination (player.transform.position);
	}
}
