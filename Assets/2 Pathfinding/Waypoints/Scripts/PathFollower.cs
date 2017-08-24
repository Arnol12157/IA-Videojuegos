using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour {

	public Transform[] path;
	public float speed = 5f;
	public float reachDist = .5f;
	public int currentPoint = 0;
	public float rotationSpeed = 3f;

	public bool change;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Vector3 dir = path [currentPoint].position - transform.position;
//		transform.position += dir * Time.deltaTime * speed;

		float dist = Vector3.Distance (path [currentPoint].position, transform.position);
		transform.position = Vector3.MoveTowards (transform.position, path [currentPoint].position, Time.deltaTime * speed);
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (path [currentPoint].position - transform.position), rotationSpeed * Time.deltaTime);


		if(/*dir.magnitude*/dist <= reachDist)
		{
			if(!change)
			{
				currentPoint++;
			}
			else
			{
				currentPoint--;
			}
		}

		if (currentPoint >= path.Length-1)
		{
//			currentPoint = 0;
			change = true;
		}
		else if(currentPoint==0)
		{
//			currentPoint = path.Length;
			change = false;
		}
	}

	void OnDrawGizmos()
	{
		if(path.Length > 0)
		{
			for (int i = 0; i < path.Length; i++)
			{
				if(path[i] != null)
				{
					Gizmos.DrawSphere (path [i].position, reachDist);
				}
			}
		}
	}
}