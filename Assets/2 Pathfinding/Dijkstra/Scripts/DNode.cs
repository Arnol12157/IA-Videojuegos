using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNode : MonoBehaviour {

	string name;
	float weight;
	string backNode;
	public Edge[] edges;

	public DNode(float weight, string backNode)
	{
		this.backNode = backNode;
		this.weight = weight;
	}

	public string getBackNode()
	{
		return this.backNode;
	}

	public float getWeight()
	{
		return this.weight;
	}

	public void changeBackNode(string newBackNode)
	{
		this.backNode = newBackNode;
	}

	public void changeWeight(float newWeight)
	{
		this.weight = newWeight;
	}
}
