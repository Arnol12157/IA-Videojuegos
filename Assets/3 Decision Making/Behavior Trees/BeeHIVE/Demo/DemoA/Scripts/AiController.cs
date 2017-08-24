using UnityEngine;
using System.Collections;
using BeeHive;

[RequireComponent( typeof( Steering ))]
[RequireComponent( typeof( Sight ))]
public class AiController : BeeHIVEAgent {

	Steering steering;
	Sight sight;
	public LayerMask foodLayer;

	const float maxStatus = 100;
	public float hunger;

	[HideInInspector]
	public Transform food;

	void Start ()
	{
		hunger = 10;
		steering = GetComponent<Steering> ();
		sight = GetComponent<Sight> ();

		InitBeeHIVE();
	}

	void Update()
	{
		steering.ApplyMovement ();
	}

	public BH_Status FoodOnSight()
	{
		if (food == null)
		{
			return BH_Status.Failure;
		}
		else
		{
			return BH_Status.Success;
		}
	}

	public BH_Status LookForFood()
	{
		steering.Wander ();

		food = sight.LookFor (foodLayer);
		if (food != null)
		{
			return BH_Status.Success;
		}
		else
		{
			return BH_Status.Running;
		}
	}

	public BH_Status Eat()
	{
		if (food == null)
		{
			return BH_Status.Failure;
		}

		if (Vector3.Distance (transform.position, food.position) <= 1)
		{
			Destroy (food.gameObject);
			hunger += 25;
			if (hunger > maxStatus)
			{
				hunger = maxStatus;
			}
			food = null;
			return BH_Status.Success;
		}
		else
		{
			steering.Seek(food.position);
			return BH_Status.Running;
		}
	}

	public BH_Status Wander()
	{
		steering.Wander ();
		return BH_Status.Running;
	}
}
