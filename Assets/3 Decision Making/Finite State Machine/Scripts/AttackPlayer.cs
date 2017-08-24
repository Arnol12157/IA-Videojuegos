using UnityEngine;
using System.Collections;

public class AttackPlayer : State {

	public State followState;
	public Color alertColor;
	public float changeRate;
	private Color normalColor;
	private float addedTime;

	void OnEnable()
	{
		normalColor = GetComponent<Renderer>().material.color;
		addedTime = 0;
		followState.enabled = false;
	}

	void Update()
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

	public override void CheckExit()
	{
		if (Vector3.Distance(transform.position,GameObject.FindGameObjectWithTag("Player").transform.position) > 2)
		{
			GetComponent<Renderer>().material.color = normalColor;
			stateMachine.ChangeState(followState);
		}
	}
}
