using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	Transform myTransform;
	Vector2 speed;

	// Use this for initialization
	void Start () {
		myTransform = transform;

		ResetBall();
	}

	void Update(){
		myTransform.Translate(speed * Time.deltaTime);
	}

	public void ResetBall(){
		myTransform.position = Vector2.zero;
		speed = new Vector2(Random.Range(3, 5), Random.Range(-3, 3));
		if(Random.Range(0.0f,1.0f)>=0.5f){
			speed.x *= -1;
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(myTransform.position.x < 0)
			GameManager.instance.ScoreForRed();
		else
			GameManager.instance.ScoreForBlue();
	}

	void OnTriggerStay2D(Collider2D col){
		speed = Vector2.Lerp(speed, Vector2.zero, Time.deltaTime*3);
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			speed.x *= -1.2f;
		}
		else{
			speed.y *= -1.2f;
		}
	}
}
