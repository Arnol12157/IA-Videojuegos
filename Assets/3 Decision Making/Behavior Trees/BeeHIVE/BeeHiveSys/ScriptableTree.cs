using UnityEngine;
using System.Collections;
using BeeHive;

[System.Serializable]
public class ScriptableTree : ScriptableObject {

	[SerializeField, HideInInspector]
	public byte[] behaviorTreeData;
}
