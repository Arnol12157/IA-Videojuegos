using UnityEngine;
using System.Collections;

public class PongController : MonoBehaviour {

	public Transform myTransform{get; private set;}
	public Transform ball{get; private set;}
	public float maxSpeed=10;

	float speed;
	// Use this for initialization
	void Start () {
		myTransform = transform;
		ball = GameObject.Find("ball").transform;
	}

	public void MoveUp()
	{
		speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime*6);
	}

	public void MoveDown()
	{
		speed = Mathf.Lerp(speed, -maxSpeed, Time.deltaTime*6);
	}

	public void Stop(){
		speed = 0;
	}

	public void ApplySpeed()
	{
		if( (speed > 0 && myTransform.position.y>=4 ) || (speed < 0 && myTransform.position.y<=-4 ))
			return;
		
		myTransform.Translate(Vector2.up*speed*Time.deltaTime);
	}
}
