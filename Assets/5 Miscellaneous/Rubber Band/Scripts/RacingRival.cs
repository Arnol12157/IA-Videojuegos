using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
	public class RacingRival : MonoBehaviour {

		public float distanceThreshold;
		public float maxSpeed;
		protected Vector3 targetPosition;
		protected float currentSpeed;
		protected RacingCenter ghost;

		public CarController m_Car;

		// Use this for initialization
		void Start ()
		{
			ghost = FindObjectOfType<RacingCenter> ();
		}
		
		// Update is called once per frame
		public virtual void Update ()
		{
			adjustSpeed (targetPosition, currentSpeed);
		}

		public virtual void adjustSpeed(Vector3 targetPosition, float speed)
		{
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed);
			/*
			float h = 0;
			float v = 2;
			float handbrake = CrossPlatformInputManager.GetAxis("Jump");
			m_Car.Move(h, v, v, handbrake);
			*/
		}
	}
}