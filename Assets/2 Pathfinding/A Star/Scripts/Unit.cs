using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Transform target;
	public float speed = 10;
	Vector3[] path;
	public Transform[] targets;
	int targetIndex;

	public bool checkTarget;
	public int i;

	void Start()
	{
		i = 0;
		checkTarget = true;
	}

	void Update()
	{
		if(checkTarget)
		{
			checkTarget = false;
			PathRequestManager.RequestPath (transform.position, targets[i].position, OnPathFound);
		}
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessfull)
	{
		if (pathSuccessfull)
		{
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{

		Vector3 currentWaypoint = path [0];

		while (true)
		{
			if(transform.position == currentWaypoint)
			{
				targetIndex++;
				if(targetIndex >= path.Length)
				{
					if(i<targets.Length-1)
					{
						i++;
						checkTarget = true;
					}
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed*Time.deltaTime);
			yield return null;
		}
	}

	public void OnDrawGizmos()
	{ 
		if (path != null)
		{
			for(int i=targetIndex; i<path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if(i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i-1], path[i]);
				}
			}
		}
	}
}