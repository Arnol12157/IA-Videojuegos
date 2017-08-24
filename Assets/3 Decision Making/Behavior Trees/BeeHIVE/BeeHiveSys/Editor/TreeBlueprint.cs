using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BeeHive;

public enum E_NodeType{selector, sequencer, inverter, succeder, repeater, repeatTilFail, leaf}

[System.Serializable]
public class TreeBlueprint : ScriptableObject {

	[SerializeField]
	public string sourceName;

	[SerializeField]
	public List<BlueprintNode> nodes;

	[SerializeField]
	public bool [] connectionMatrix;

}

[System.Serializable]
public class BlueprintNode{
	[SerializeField]
	public E_NodeType nodeType;
	[SerializeField]
	public string methodName="";
	[SerializeField]
	public float xPos;
	[SerializeField]
	public float yPos;

	public BlueprintNode(BeeHiveNode bhNode){
		if(bhNode.myNode is BH_Selector){
			nodeType = E_NodeType.selector;
		}
		else if(bhNode.myNode is BH_Sequence){
			nodeType = E_NodeType.sequencer;
		}
		else if(bhNode.myNode is BH_Inverter){
			nodeType = E_NodeType.inverter;
		}
		else if(bhNode.myNode is BH_Succeeder){
			nodeType = E_NodeType.succeder;
		}
		else if(bhNode.myNode is BH_Repeater){
			nodeType = E_NodeType.repeater;
		}
		else if(bhNode.myNode is BH_RepeatUntilFail){
			nodeType = E_NodeType.repeatTilFail;
		}
		else if(bhNode.myNode is BH_Leaf){
			nodeType = E_NodeType.leaf;
		}

		methodName = string.Copy(bhNode.methodName);

		xPos = bhNode.windowRect.position.x;
		yPos = bhNode.windowRect.position.y;
	}
}
