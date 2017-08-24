using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayesClassifier : MonoBehaviour {

	public enum BCLabel
	{
		POSITIVE,
		NEGATIVE
	}

	public int numAttributes;
	public int numExamplesPositive;
	public int numExamplesNegative;

	public List<bool> attrCountPositive;
	public List<bool> attrCountNegative;

	public bool[] attributesTrain;
	public bool[] attributesTrainpos; //2 array de train
	public bool[] attributesTest;

	void Awake()
	{
		attrCountPositive = new List<bool> ();
		attrCountNegative = new List<bool> ();
	}

	// Use this for initialization
	void Start ()
	{
		updateClassifier (attributesTrainpos, BCLabel.POSITIVE);
		updateClassifier (attributesTrain, BCLabel.NEGATIVE); //train para el 2 array
		Predict (attributesTest);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void updateClassifier(bool[] attributes, BCLabel label)
	{
		if (label == BCLabel.POSITIVE)
		{
			numExamplesPositive++;
			attrCountPositive.AddRange (attributes);
		}
		else
		{
			numExamplesNegative++;
			attrCountNegative.AddRange (attributes);
		}
	}

	public float Probabilities(ref bool[] attributes, bool[] counts, float m, float n)
	{
		float prior = m / (m + n);
		float p = 1f;
		for(int i=0; i<numAttributes; i++)
		{
			p /= m;
			if(attributes[i])
			{
				p *= counts [i].GetHashCode ();
			}
			else
			{
				p *= m - counts [i].GetHashCode ();
			}
		}
		return prior * p;
	}

	public bool Predict(bool[] attributes)
	{
		float nep = numExamplesPositive;
		float nen = numExamplesNegative;
		float x = Probabilities (ref attributes, attrCountPositive.ToArray (), nep, nen);
		float y = Probabilities (ref attributes, attrCountNegative.ToArray (), nen, nep);
		if(x>=y)
		{
			Debug.Log ("POSITIVE");
			return true;
		}
		Debug.Log ("NEGATIVE");
		return false;
	}
}
