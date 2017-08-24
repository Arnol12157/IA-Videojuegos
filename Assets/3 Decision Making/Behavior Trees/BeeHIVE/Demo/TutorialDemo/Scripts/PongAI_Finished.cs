using UnityEngine;
using System.Collections;
using BeeHive;

public class PongAI_Finished : BeeHIVEAgent {

	PongController pongController;

	// Use this for initialization
	void Start () {
		pongController = GetComponent<PongController>();

		InitBeeHIVE();
	}

	// Update is called once per frame
	void Update () {
		pongController.ApplySpeed();
	}
	
	public BH_Status MoveUp(){
		pongController.MoveUp();
		return BH_Status.Success;
	}

	public BH_Status MoveDown(){
		pongController.MoveDown();
		return BH_Status.Success;
	}

	public BH_Status StopMoving(){
		pongController.Stop();
		return BH_Status.Success;
	}

	public BH_Status BallIsAbove(){
		float myHeight = pongController.myTransform.position.y;
		float ballHeight = pongController.ball.position.y;
		if(myHeight < ballHeight)
			return BH_Status.Success;
		else 
			return BH_Status.Failure;
	}
}
