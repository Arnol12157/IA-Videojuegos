using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	public class RacingCenter : RacingRival {

		public GameObject player;

		// Use this for initialization
		void Start ()
		{
			m_Car = gameObject.GetComponent<CarController> ();
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		
		// Update is called once per frame
		public override void Update ()
		{
			Vector3 playerPos = player.transform.position;
			float dist = Vector3.Distance (transform.position, playerPos);
			if (dist > distanceThreshold)
			{
				targetPosition = player.transform.position;
				targetPosition.Set (transform.position.x, targetPosition.y, targetPosition.z);
				currentSpeed = 1f;
				base.Update ();
			}
			else
			{
				targetPosition = player.transform.position;
				targetPosition.Set (transform.position.x, targetPosition.y, targetPosition.z);
				currentSpeed = 0.1f;
				base.Update ();
			}
		}
		/*
		public override void adjustSpeed(Vector3 targetPosition)
		{
			base.adjustSpeed (targetPosition);
		}
		*/
	}
}