using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using BeeHive;
using System;


public class BeeHiveNode : ScriptableObject, IComparable<BeeHiveNode> {

	public string methodName="";
	public BH_Node myNode; 
	public List<BeeHiveNode> children = new List<BeeHiveNode>(); 
	public int amountOfChildren{get{return children.Count;}} 
	public BeeHiveNode parent = null;
	public Rect windowRect;
	public string windowTitle = "";

	int methodIndex=0;

	Texture myTexture;
	Vector2 windowSize = new Vector2(64, 96);
	public float ioBarHeight = 16;
	Vector2 deleteConnetionBtnSize = new Vector2(17, 17);

	public void BuildNode(BH_Node node, Vector2 position){
		myNode = node;

		if(myNode is BH_Selector){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.selectorIcon );
			windowTitle = "Selector";
		}
		else if(myNode is BH_Sequence){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.sequenceIcon );
			windowTitle = "Sequence";
		}
		else if(myNode is BH_Inverter){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.inverterIcon );
			windowTitle = "Inverter";
		}
		else if(myNode is BH_Succeeder){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.succederIcon );
			windowTitle = "Succeder";
		}
		else if(myNode is BH_Repeater){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.repeaterIcon );
			windowTitle = "Repeater";
		}
		else if(myNode is BH_RepeatUntilFail){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.repeatTilFailIcon );
			windowTitle = "Until fail";
		}
		else if(myNode is BH_Leaf){
			myTexture = BTGuiLoader.GetTexture( E_TextureNames.leafIcon );
			windowTitle = "Leaf";
			windowSize = new Vector2(100, 56);

			UpdateMethodIndex();
		}

		windowRect = new Rect(position.x, position.y, windowSize.x,windowSize.y);
	}

	public void DrawWindow(){	

		GUILayout.Space(16);

		GUILayout.Box (myTexture, GUIStyle.none, GUILayout.Height(windowSize.y * 0.83f), GUILayout.ExpandWidth(true));

		if(!(myNode is BH_Leaf)){
			if( GUI.Button(new Rect(0, windowSize.y - ioBarHeight, windowSize.x, ioBarHeight),"▼", GUI.skin.box) )
				OnBottonConnectorClicked();
		}
		else{
			if(NodeEditor.typeIndex==0)
				methodName = GUI.TextField(new Rect(0, windowSize.y - ioBarHeight, windowSize.x, ioBarHeight), methodName);
			else{
				GUILayout.BeginArea(new Rect(0, windowSize.y - ioBarHeight, windowSize.x, ioBarHeight));
				methodIndex = EditorGUILayout.Popup(methodIndex, NodeEditor.methodOptions.ToArray());
				GUILayout.EndArea();
				try {
					methodName = NodeEditor.methodOptions[methodIndex];	
				} catch (Exception) {
					UpdateMethodIndex();
					methodName = NodeEditor.methodOptions[methodIndex];
				}
			}

		}
	}

	public void DrawCurves(){
		float spacing = windowSize.x / children.Count;

		for ( int i = 0; i < children.Count; i++ ) {
			NodeEditor.DrawNodeCurve( children[i].GetTopRect() , GetBottonRect(spacing, i));
		}
	}

	public void NodeDeleted(){
		SetParent(null);
		foreach (BeeHiveNode b in children) {
			b.parent = null;
		}
		children.Clear();
	}

	public void SetParent(BeeHiveNode parent){
		if(this.parent != null){			
			this.parent.RemoveChild(this);
		}

		this.parent = parent;

	}

	public void AddChild(BeeHiveNode newChild){
		if(myNode is BH_Leaf || this.GetInstanceID() == newChild.GetInstanceID() || children.Contains(newChild) || ( parent != null && parent.GetInstanceID() == newChild.GetInstanceID() ))
			return;
		else{
			if(myNode is BH_Decorator){
				if(children.Count > 0){
					children[0].SetParent(null);
					children.Clear();
				}
			}

			children.Add(newChild);
			newChild.SetParent(this);	

			SortChildren();
		}
	}

	public void RemoveChild(BeeHiveNode child){
		try {
			children.Remove(child);
			child.parent = null;
		} catch (Exception ex) {
			Debug.LogException(ex); 
		}
	}

	public Rect GetTopRect(){
		return new Rect(windowRect.position.x + windowRect.size.x*0.5f - deleteConnetionBtnSize.x*0.5f, windowRect.position.y-deleteConnetionBtnSize.y, deleteConnetionBtnSize.x, deleteConnetionBtnSize.y);		 
	}

	public Rect GetBottonRect(){
		return new Rect(windowRect.x, windowRect.y+windowSize.y-ioBarHeight, windowSize.x, ioBarHeight);
	}

	public Rect GetBottonRect(float spacing, int xOffset){
		return new Rect(windowRect.x + xOffset*spacing + spacing*0.5f, windowRect.y+windowSize.y-ioBarHeight, 0.1f, ioBarHeight);
	}

	public void SortChildren(){
		children.Sort();
	}

	public void SortSiblings(){
		if(parent != null)
			parent.SortChildren();
	}

	public void UpdateMethodIndex(){
		methodIndex=0;
		for(int i=0; i<NodeEditor.methodOptions.Count; i++){
			if(NodeEditor.methodOptions[i] == methodName){
				methodIndex=i;
				return;
			}
		}
	}

	public int CompareTo(BeeHiveNode other)
	{
		if (this.windowRect.position.x > other.windowRect.position.x) {
			return 1;            
		}

		else if(this.windowRect.position.x < other.windowRect.position.x){
			return -1;
		}

		else{
			return 0;	
		}
	}

	public event EventHandler bottonConnectorClick; 
	protected virtual void OnBottonConnectorClicked(){		
		if(bottonConnectorClick!=null){
			bottonConnectorClick(this,EventArgs.Empty);
		}	
	}
}
