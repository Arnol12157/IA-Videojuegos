using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour {

	public float radius=20;
	public float fieldOfView = 110;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	public Transform LookFor(LayerMask mask)
	{
		Collider[] found = Physics.OverlapSphere(transform.position, radius, mask);

		Vector3 direction; 
		float angle;
		RaycastHit hit;
		foreach (Collider c in found)
		{
			direction = c.transform.position - transform.position;
			angle = Vector3.Angle(direction, transform.forward);

			if(angle<fieldOfView*0.5f)
			{
				Debug.DrawRay(transform.position , direction.normalized*radius, Color.green);
				if(Physics.Raycast(transform.position, direction.normalized, out hit, radius))
				{
					return c.transform;
				}
			}
			else
			{
				Debug.DrawRay(transform.position , direction.normalized*radius, Color.red);
			}
		}
		return null;
	}
}