using UnityEngine;
using System.Collections;

public class HumanPongController : PongController {

	float input;
	
	// Update is called once per frame
	void Update () {
		input = Input.GetAxis("Vertical");

		if(input>0)
			MoveUp();
		else if(input<0)
			MoveDown();
		else
			Stop();

		ApplySpeed();
	}
}
