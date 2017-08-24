using UnityEngine;
using System.Collections;

public class FollowPlayer : State {

	public float speedAI;
	public State attackState;

	void OnEnable()
	{
		attackState.enabled = false;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.MoveTowards (transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, Time.deltaTime * speedAI);
		transform.LookAt (GameObject.FindGameObjectWithTag("Player").transform.position);
		Debug.Log ("Siguiendo");
		Debug.Log (Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag ("Player").transform.position));
	}

	public override void CheckExit()
	{
		if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= 2)
		{
			stateMachine.ChangeState (attackState);
		}
	}
}
