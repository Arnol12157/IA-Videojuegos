using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

	float weight;
	DNode nextNode;

	void Start()
	{
		this.nextNode = this.gameObject.GetComponent<DNode> ();
	}

	public Edge(float weight)
	{
		this.weight = weight;
		this.nextNode = this.gameObject.GetComponent<DNode> ();
	}

	public float getWeight()
	{
		return this.weight;
	}

	public void setWeight(float newWeight)
	{
		this.weight = newWeight;
	}

	public DNode getNextNode()
	{
		return this.nextNode;
	}

	public void setNextNode(DNode newNextNode)
	{
		this.nextNode = newNextNode;
	}

}
