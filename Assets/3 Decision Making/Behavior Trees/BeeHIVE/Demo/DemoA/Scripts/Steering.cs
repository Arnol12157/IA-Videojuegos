using UnityEngine;
using System.Collections;

public class Steering : MonoBehaviour {

	public float maxVelocity=8;
	public float aceleration=2;
	public float mass = 1;

	CharacterController controller;
	Transform myTransform;

	Vector3 velocity=Vector3.zero;
	Vector3 input = Vector3.zero;
	Vector3 desiredVelocity;
	Vector3 steering=Vector3.zero;

	Vector3 localTarget;

	float slowingRadius;

	float timer;

	RaycastHit hit;

	const float gravity = -30;
	// Use this for initialization
	void Awake ()
	{
		slowingRadius=2*maxVelocity;
		controller = GetComponent<CharacterController> ();
		myTransform = GetComponent<Transform> ();
		timer = 0;
	}	


	public void ApplyMovement()
	{
		AvoidObstacles ();
		velocity = Vector3.Lerp (velocity, input, Time.deltaTime*aceleration);
		ApllyGravity ();
		controller.Move (velocity*Time.deltaTime);
		myTransform.forward = new Vector3(velocity.x,0,velocity.z);
		input = Vector3.zero;
	}

	void ApllyGravity()
	{
		velocity = new Vector3 (velocity.x, gravity, velocity.z);
	}

	public void Seek(Vector3 target)
	{
		desiredVelocity = (target - myTransform.position).normalized * maxVelocity;

		float distance = Vector3.Distance (myTransform.position, target);
		if (distance < slowingRadius)
		{

			desiredVelocity *= (distance / slowingRadius);
		}
			
		steering = desiredVelocity - velocity;
		steering = steering/mass;
		
		input += (velocity + steering);

//		Debug.DrawRay (myTransform.position, desiredVelocity, Color.red);
//		Debug.DrawRay (myTransform.position, velocity, Color.green);
//		Debug.DrawRay (myTransform.position, steering, Color.blue);
	}

	public void Flee(Vector3 target)
	{
		desiredVelocity = (myTransform.position-target).normalized * maxVelocity;
		
		float distance = Vector3.Distance (myTransform.position, target);
		if (distance > slowingRadius)
		{
			desiredVelocity *= (slowingRadius/distance);
		}		
		
		steering = desiredVelocity - velocity;
		steering = steering/mass;
		
		input += (velocity + steering);
	}

	public void Wander()
	{
		if (timer <= 0)
		{
			localTarget = new Vector3(myTransform.position.x + Random.Range(-slowingRadius, slowingRadius), myTransform.position.y, myTransform.position.z + Random.Range(-slowingRadius, slowingRadius));
			timer = Random.Range(4.0f, 8.0f);
		}
		timer -= Time.deltaTime;
		Seek (localTarget);
	}

	public void Pursuit(CharacterController target)
	{
		Vector3 futurePosition = target.transform.position + target.velocity * 2;
		Seek (futurePosition);
	}
	public void Evade(CharacterController target)
	{
		Vector3 futurePosition = target.transform.position - target.velocity * 2;
		Flee(futurePosition);
	}

	void AvoidObstacles()
	{
		if(Physics.Raycast(myTransform.position, myTransform.forward,out hit, maxVelocity))
		{
			input += (velocity - hit.transform.position).normalized*maxVelocity;
			Debug.DrawRay(myTransform.position, myTransform.forward*maxVelocity, Color.red);
		}
	}
}
