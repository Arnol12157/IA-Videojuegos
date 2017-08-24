using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour {

	public DNode[] nodes;
	public List<GameObject> path;
	public GameObject player;
	public float speed;
	public float rotationSpeed;
	int currentPoint;
	bool finishDijkstra;

	// Use this for initialization
	void Start ()
	{
		currentPoint = 0;
		finishDijkstra = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			clearEverything ();
			beginDijkstra ();
			findPath ();
		}
		if(finishDijkstra)
		{
			float dist = Vector3.Distance (path [currentPoint].transform.position, player.transform.position);
			player.transform.position = Vector3.MoveTowards (player.transform.position, path [currentPoint].transform.position, Time.deltaTime * speed);
			player.transform.rotation = Quaternion.Slerp (player.transform.rotation, Quaternion.LookRotation (path [currentPoint].transform.position - player.transform.position), rotationSpeed * Time.deltaTime);
			if(dist <= 0.5f)
			{
				currentPoint++;
			}
			if (currentPoint >= path.Count)
			{
				currentPoint = 0;
			}
		}
	}

	public void beginDijkstra()
	{
		for(int i=0; i<nodes.Length; i++)
		{
			DNode currentNode = nodes [i];
			for(int j=0; j<currentNode.edges.Length; j++)
			{
				DNode nextNode = currentNode.edges [j].getNextNode ();
				float distance = Vector3.Distance (currentNode.transform.position, nextNode.transform.position);
				currentNode.edges [j].setWeight (distance);
				if(nextNode.getWeight() > 0)
				{
					nextNode.changeWeight (Mathf.Min (distance + currentNode.getWeight (), nextNode.getWeight ()));
					if((distance + currentNode.getWeight ())<nextNode.getWeight ())
					{
						nextNode.changeBackNode (currentNode.name);
					}
				}
				else
				{
					nextNode.changeWeight (distance + currentNode.getWeight ());
					nextNode.changeBackNode (currentNode.name);
				}
			}
		}
		finishDijkstra = true;
	}

	public void findPath()
	{
		string backNode = "Node6";
		while(backNode!="Node1")
		{
			GameObject waypoint = GameObject.Find (backNode);
			path.Add (waypoint);
			backNode = waypoint.GetComponent<DNode> ().getBackNode ();
		}
		path.Add (GameObject.Find ("Node1"));
		path.Reverse ();
	}

	public void showWeights()
	{
		for(int i=0; i<nodes.Length; i++)
		{
			DNode currentNode = nodes [i];
			Debug.Log (currentNode.getWeight ());
		}
	}

	public void clearEverything()
	{
		for(int i=0; i<nodes.Length; i++)
		{
			DNode currentNode = nodes [i];
			currentNode.changeBackNode ("");
			currentNode.changeWeight (0);
			for(int j=0; j<currentNode.edges.Length; j++)
			{
				Edge currentEdge = currentNode.edges [j];
				currentEdge.setWeight (0);
			}
		}
		path.Clear ();
	}

	void OnDrawGizmos()
	{
		if(nodes.Length > 0)
		{
			for (int i = 0; i < nodes.Length; i++)
			{
				if(i==0)
				{
					Gizmos.color = Color.green;
				}
				else if(i==nodes.Length-1)
				{
					Gizmos.color = Color.red;
				}
				else
				{
					Gizmos.color = Color.white;
				}
				Gizmos.DrawSphere (nodes [i].transform.position, .5f);
				for(int j=0; j<nodes[i].edges.Length; j++)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawLine (nodes[i].transform.position, nodes[i].edges [j].transform.position);
				}
			}
		}
	}
}
