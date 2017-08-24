using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BeeHive;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

public class NodeEditor : EditorWindow {

	List<BeeHiveNode> nodes = new List<BeeHiveNode>();

	Vector2 mousePosition;

	BeeHiveNode selectedNode;

	bool makeTransitionMode = false;

	bool dragMode = false;
	Vector2 dragDeltaPos;

	Vector2 deleteBtnSize = new Vector2(16, 16);
	Vector2 deleteBtnOffset = new Vector2(4, 0);

	Vector2 selectionBorderSize = new Vector2(1, 1);

	Vector2 sideMenuSize = new Vector2(220, 800);
	Color sideMenuColor = new Color(40.0f/255.0f, 42.0f/255.0f, 50.0f/255.0f);

	bool skipNodeDrawing = false;

	string btName = "";

	TreeBlueprint blueprintToLoad = null;

	GUIStyle bigTextStyle = new GUIStyle();

	GUIStyle deleteStyle;
	Color deleteColor = new Color(192.0f/255.0f, 97.0f/255.0f, 97.0f/255.0f);

	GUIStyle selectionStyle;
	Color selectionColor = new Color(0.07f, 0.04f, 0.11f, 0.3f);


	List<Type> derivedTypes;
	public List<string> typeOptions = new List<string>();
	public static int typeIndex=0;

	public static List<string> methodOptions ;


	Type sourceType{
		get{
			try {
				return derivedTypes[typeIndex-2];
			} 
			catch (Exception) {
				return null;
			}
				
		}
	}

	[MenuItem("Window/BeeHive Editor")]
	static void ShowEditor(){
		EditorWindow.GetWindow<NodeEditor>();
		BTGuiLoader.initGUILoader ();
	}

	void OnEnable(){
		derivedTypes = FindDerivedTypes().ToList();

		typeOptions.Add("None");
		typeOptions.Add("All");


		foreach (Type t in derivedTypes) {
			typeOptions.Add(t.Name);
		}

		UpdateMethodOptions();
	
	}


	void OnValidate(){
		BTGuiLoader.initGUILoader ();
	}

	void OnGUI(){
		DrawSideMenu();

		GUILayout.BeginArea(new Rect(sideMenuSize.x+20, 0, 2000, 2000));

		bigTextStyle.fontSize = 40;
		bigTextStyle.normal.textColor = Color.gray;

		GUI.Label(new Rect(40,40,256,56), btName, bigTextStyle);

		HandleInput();
		DrawAllLines();
		HandleSelection();
		DrawAllNodes();
		DrawConnectionBoxes();
		DrawSelectionGUI();

		GUILayout.EndArea(); 

	}

	void DrawSideMenu(){		
		GUI.backgroundColor = sideMenuColor;
		GUILayout.BeginVertical(GUI.skin.FindStyle("Box"), GUILayout.Width(sideMenuSize.x), GUILayout.ExpandHeight(true));
		GUI.backgroundColor = Color.white;

		GUILayout.Box (BTGuiLoader.textures[(int)E_TextureNames.logo],GUIStyle.none);


		EditorGUILayout.BeginVertical(GUI.skin.FindStyle("Box"), GUILayout.ExpandWidth(true));//-------------------------


		GUILayout.Label( "BEHAVIOR TREE" );
		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("Source:");

		EditorGUI.BeginChangeCheck();
		typeIndex = EditorGUILayout.Popup(typeIndex, typeOptions.ToArray());
		if(EditorGUI.EndChangeCheck()){
			UpdateMethodOptions();
			UpdateNodesMethodsIndexes();
		}

		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("Name:");

		EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

		btName = GUILayout.TextField (btName);

		if(GUILayout.Button ("Save", GUILayout.ExpandWidth(false))){
			if(string.Equals(btName, "")){
				EditorUtility.DisplayDialog("ERROR", "You can not save without a name!", "OK");
			}
			else{
				SaveBlueprint(btName);
				BuildTree(btName+"_tree"); 	 
			}

		}

		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button ("New tree")) {
			NewTree(); 
			skipNodeDrawing = true;
		}

		EditorGUILayout.EndVertical();//----------------------------------------------------



		EditorGUILayout.BeginVertical(GUI.skin.FindStyle("Box"), GUILayout.ExpandWidth(true), GUILayout.MinHeight(70));//-------------------------


		GUILayout.Label( "NODE INSPECTOR" );
		EditorGUILayout.Separator ();
		if(selectedNode != null && nodes.Count>0){
			if(selectedNode.parent != null){
				EditorGUILayout.LabelField("Parent: " + selectedNode.parent.windowTitle);
			}

			if(selectedNode.myNode is BH_Composite)
				EditorGUILayout.LabelField("Amount of Childs: " + selectedNode.amountOfChildren.ToString()); 
			else if(selectedNode.myNode is BH_Decorator){
				if( selectedNode.amountOfChildren == 0)
					EditorGUILayout.LabelField("Connected Child: FALSE");
				else
					EditorGUILayout.LabelField("Connected Child: TRUE");
			}


		}	
		EditorGUILayout.EndVertical();//----------------------------------------------------



		EditorGUILayout.BeginVertical(GUI.skin.FindStyle("Box"), GUILayout.ExpandWidth(true));//-------------------------

		GUILayout.Label( "BLUEPRINTS" );

		EditorGUILayout.Separator ();

		blueprintToLoad = ( EditorGUILayout.ObjectField("", blueprintToLoad, typeof(TreeBlueprint), false ) ) as TreeBlueprint;

		EditorGUILayout.BeginHorizontal();

		if(GUILayout.Button ("Load")) {
			NewTree();
			LoadBlueprint(blueprintToLoad);
		}

		if(GUILayout.Button ("Insert")) {
			//LoadBlueprint(blueprintToLoad);
		}
		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button ("Delete")) {
			if(EditorUtility.DisplayDialog("Warning", "Are you sure that you want to delete this?", "Yes", "No"))
				DeleteBlueprint();
		}

		EditorGUILayout.EndVertical();//-----------------------------------------------------


		EditorGUILayout.BeginVertical( GUI.skin.FindStyle("Box"), GUILayout.ExpandWidth(true) );//-------------------------


		GUILayout.Label( "NODES" );

		if(GUILayout.Button ( new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.leafIcon), "Actions. Represent Methods."), GUILayout.Width(120), GUILayout.Height(32))){
			CreateNewNode(new BH_Leaf(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button ( new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.selectorIcon), "Return a success if any of its children succeed\nand not process any further children." ),  GUILayout.Width(50), GUILayout.Height(50))){
			CreateNewNode(new BH_Selector(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}

		if(GUILayout.Button ( new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.sequenceIcon), "Visit each child in order. If any child fails it\nwill immediately return failure to the parent." ),  GUILayout.Width(50), GUILayout.Height(50))){
			CreateNewNode(new BH_Sequence(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}

		EditorGUILayout.EndHorizontal();	

		EditorGUILayout.BeginHorizontal();

		if(GUILayout.Button ( new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.inverterIcon), "Invert the result of their child node." ),  GUILayout.Width(50), GUILayout.Height(50))){
			CreateNewNode(new BH_Inverter(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}
		if(GUILayout.Button (new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.succederIcon), "Always return success to the parent." ),  GUILayout.Width(50), GUILayout.Height(50))){
			CreateNewNode(new BH_Succeeder(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}
		if(GUILayout.Button ( new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.repeaterIcon), "Reprocess its child node each time its child\nreturns a result." ), GUILayout.Width(50), GUILayout.Height(50))){
			CreateNewNode(new BH_Repeater(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}
		if(GUILayout.Button ( new GUIContent( BTGuiLoader.GetTexture(E_TextureNames.repeatTilFailIcon), "Reprocess their child until the child returns\n a failure, than it will return success to its parent." ),  GUILayout.Width(50), GUILayout.Height(50))){
			CreateNewNode(new BH_RepeatUntilFail(), new Vector2(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300)));
		}

		EditorGUILayout.EndHorizontal();


		EditorGUILayout.EndVertical();//-----------------------------------------------------


		GUILayout.EndVertical();
	}

	void HandleInput(){
		Event e = Event.current;

		mousePosition = e.mousePosition;


		if(e.button == 1 && !makeTransitionMode){
			if(e.type == EventType.MouseDown){
				bool clickedOnNode = CheckForClickOnNode();

				if(!clickedOnNode){
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Add selector"), false, Callback, "Selector");
					menu.AddItem(new GUIContent("Add sequencer"), false, Callback, "Sequencer");
					menu.AddItem(new GUIContent("Add inverter"), false, Callback, "Inverter");
					menu.AddItem (new GUIContent ("Add succeder"), false, Callback, "Succeder");
					menu.AddItem (new GUIContent ("Add repeater"), false, Callback, "Repeater");
					menu.AddItem (new GUIContent ("Add repeater until fail"), false, Callback, "RepeatTilFail");
					menu.AddItem (new GUIContent ("Add leaf"), false, Callback, "Leaf");
					menu.ShowAsContext();
					e.Use();
				}
				else{
					GenericMenu menu = new GenericMenu();
					if( !(selectedNode.myNode is BH_Leaf) ){
						menu.AddItem(new GUIContent("Make link"), false, Callback, "makeLink");
					}
					menu.AddItem(new GUIContent("Delete"), false, Callback, "delete");
					menu.ShowAsContext();
					e.Use();
				}
			}
		}

		else if(e.button == 0 ){
			if(e.type == EventType.MouseDown){
				BeeHiveNode oldNode = selectedNode;

				bool clickedOnNode = CheckForClickOnNode();

				if(makeTransitionMode){
					if(clickedOnNode){
						oldNode.AddChild(selectedNode);
					}
					makeTransitionMode = false;
					e.Use();
				}
				else if(!clickedOnNode){
					dragMode = true;
					dragDeltaPos = e.mousePosition;
				}
			}
			if(e.type == EventType.MouseUp){
				dragMode = false;

				if(selectedNode != null){
					selectedNode.SortSiblings();
				}
			}
		}

		if(makeTransitionMode && selectedNode !=null){
			Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 1,1);

			DrawNodeCurve(mouseRect, selectedNode.GetBottonRect());

			Repaint();
		}

	}

	void DrawAllLines(){
		foreach(BeeHiveNode b in nodes){
			b.DrawCurves();
		}
	}

	void HandleSelection(){
		if(selectedNode != null){	
			selectionStyle = new GUIStyle(GUI.skin.box);
			selectionStyle.border = new RectOffset(0,0,0,0);

			GUI.backgroundColor = selectionColor;
			GUI.Box(new Rect(selectedNode.windowRect.position + selectionBorderSize*2, selectedNode.windowRect.size + selectionBorderSize*2),"");
			GUI.backgroundColor = Color.white;
		}
	}

	void DrawConnectionBoxes(){
		foreach(BeeHiveNode b in nodes){
			foreach (BeeHiveNode c in b.children) {
//				Rect topRect = c.GetTopRect();
				if(GUI.Button( c.GetTopRect(),"▼", GUI.skin.FindStyle("Box")) ){
					b.RemoveChild(c);
					return;
				}
			}
		}
	}

	void DrawAllNodes(){
		if(skipNodeDrawing){
			skipNodeDrawing = false;
			return;
		}

		BeginWindows();

		for (int i = 0; i < nodes.Count; i++) {
			nodes[i].windowRect = GUI.Window(i, nodes[i].windowRect, DrawNodeWindow, nodes[i].windowTitle, GUI.skin.box);
			if(dragMode){				
				nodes[i].windowRect.position += mousePosition - dragDeltaPos;
			}
		}

		if(dragMode){
			dragDeltaPos = mousePosition;
			Repaint();
		}

		EndWindows();
	}

	void DrawSelectionGUI(){
		if(selectedNode != null){
			GUI.backgroundColor = deleteColor;
			deleteStyle = new GUIStyle(GUI.skin.box);
			deleteStyle.normal.textColor = Color.white;
			deleteStyle.fontStyle = FontStyle.Bold;

			if ( GUI.Button(new Rect(selectedNode.windowRect.position + Vector2.right*selectedNode.windowRect.width + deleteBtnOffset, deleteBtnSize), "X", deleteStyle) ){
				RemoveNode(selectedNode);
			}
			GUI.backgroundColor = Color.white;
		}
	}

	public void Callback (object obj) {
		string callback = obj.ToString();

		switch(callback){
			case "Selector":
			CreateNewNode(new BH_Selector(), mousePosition);
				break;
			case "Sequencer":
			CreateNewNode(new BH_Sequence(), mousePosition);
				break;
			case "Inverter":
			CreateNewNode(new BH_Inverter(), mousePosition);
				break;
			case "Succeder":
			CreateNewNode(new BH_Succeeder(), mousePosition);
				break;
			case "Repeater":
			CreateNewNode(new BH_Repeater(), mousePosition);
				break;
			case "RepeatTilFail":
			CreateNewNode(new BH_RepeatUntilFail(), mousePosition);
				break;
			case "Leaf":
			CreateNewNode(new BH_Leaf(), mousePosition);
				break;
			case "makeLink":
				makeTransitionMode = true;
				break;
			case "delete":
				RemoveNode(selectedNode);
				selectedNode = null;
				break;
		}
	}

	void DrawNodeWindow(int index){
		nodes[index].DrawWindow();
		GUI.DragWindow();
	}

	public static void DrawNodeCurve(Rect from, Rect to){
		Vector3 startPos = new Vector3(from.x + from.width/2, from.y + from.height/2,0);
		Vector3 endPos = new Vector3(to.x + to.width/2, to.y + to.height/2,0);

		Vector3 startTang = startPos + Vector3.down * 50;
		Vector3 endTang = endPos + Vector3.up*50;

		Color shadowColor = new Color(0,0,0, 0.06f);

		for (int i = 0; i < 3; i++) {
			Handles.DrawBezier(startPos,endPos,startTang,endTang, shadowColor, null, (i+1) *5);
		}
		Handles.DrawBezier(startPos,endPos,startTang,endTang, Color.black, null, 1);
	}

	void CreateNewNode(BH_Node nodeClass, Vector2 position){
		BeeHiveNode node = ScriptableObject.CreateInstance<BeeHiveNode>();
		node.BuildNode(nodeClass, position);
		nodes.Add(node);

		node.bottonConnectorClick += OnBottonConnectorClicked;
	}

	void CreateNewNode(BlueprintNode bpNode){
		BeeHiveNode node = ScriptableObject.CreateInstance<BeeHiveNode>();

		Vector2 position = new Vector2( bpNode.xPos, bpNode.yPos );

		node.methodName = string.Copy( bpNode.methodName );

		if(bpNode.nodeType == E_NodeType.selector){
			node.BuildNode(new BH_Selector(), position);
		}
		else if(bpNode.nodeType == E_NodeType.sequencer){
			node.BuildNode(new BH_Sequence(), position);
		}
		else if(bpNode.nodeType == E_NodeType.inverter){
			node.BuildNode(new BH_Inverter(), position);
		}
		else if(bpNode.nodeType == E_NodeType.succeder){
			node.BuildNode(new BH_Succeeder(), position);
		}
		else if(bpNode.nodeType == E_NodeType.repeater){
			node.BuildNode(new BH_Repeater(), position);
		}
		else if(bpNode.nodeType == E_NodeType.repeatTilFail){
			node.BuildNode(new BH_RepeatUntilFail(), position);
		}
		else if(bpNode.nodeType == E_NodeType.leaf){
			node.BuildNode(new BH_Leaf(), position);
		}



		nodes.Add(node);

		node.bottonConnectorClick += OnBottonConnectorClicked;
	}

	void RemoveNode(BeeHiveNode node){

		node.bottonConnectorClick -= OnBottonConnectorClicked;

		node.NodeDeleted();
		nodes.Remove(node);

		selectedNode = null;
	}

	void NewTree(){
		for (int i = 0; i < nodes.Count; i++) {
			RemoveNode(nodes[i]);
		}
		nodes.Clear();
		btName = "";
	}

	void BuildTree(string treeName){
		BH_BehaviorTree tree = new BH_BehaviorTree();

		for (int i = 0; i < nodes.Count; i++) {
			if(nodes[i].myNode is BH_Composite){

				if(nodes[i].amountOfChildren == 0){
					continue;
				}

				for (int j = 0; j < nodes[i].amountOfChildren; j++) {
					(nodes[i].myNode as BH_Composite).AddChild(nodes[i].children[j].myNode);
				}
			}
			else if(nodes[i].myNode is BH_Decorator){
				if(nodes[i].amountOfChildren == 0){
					continue;
				}

				(nodes[i].myNode as BH_Decorator).SetChild(nodes[i].children[0].myNode);
			}
			else{
				(nodes[i].myNode as BH_Leaf).SetMethodName(nodes[i].methodName);
			}

			tree.nodes.Add(nodes[i].myNode);
		}

		SaveTree(tree, treeName);
	}



	void SaveTree(BH_BehaviorTree tree, string name){

		List<BeeHIVEAgent> agents = GetReferencesThatUse(name);

		//string filepath = "Assets/BeeHIVE/Trees/" + name + ".asset";
		string filepath = "Assets/3 Decision Making/Behavior Trees/Trees/" + name + ".asset";
		ScriptableTree sTree = ScriptableObject.CreateInstance<ScriptableTree> ();

		byte [] buffer = new byte[5122];
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream s = new MemoryStream(buffer);
		bf.Serialize(s, tree);

		sTree.behaviorTreeData = buffer;
		AssetDatabase.CreateAsset(sTree, filepath);
		AssetDatabase.SaveAssets();

		if(agents.Count>0){
			UpdateReferences(agents, name);
		}
	} 

	void SaveBlueprint(string name){
		//string filepath = "Assets/BeeHIVE/Blueprints/"+name+".asset";	
		string filepath = "Assets/3 Decision Making/Behavior Trees/Trees/" + name + ".asset";

		TreeBlueprint bluePrint = ScriptableObject.CreateInstance<TreeBlueprint> ();

		bluePrint.sourceName = typeOptions[typeIndex];

		bluePrint.nodes = new List<BlueprintNode>();
		for (int i = 0; i < nodes.Count; i++) {
			bluePrint.nodes.Add(new BlueprintNode(nodes[i]));
		}
		CreateConnectionMatrix(bluePrint);

		AssetDatabase.CreateAsset(bluePrint, filepath);
		AssetDatabase.SaveAssets();
	}

	void LoadBlueprint(TreeBlueprint blueprint){		
		if(blueprint == null)
			return;

		typeIndex=0;
		for (int i = 0; i < typeOptions.Count; i++) {
			if(typeOptions[i] == blueprint.sourceName){
				typeIndex=i;
				break;
			}
		}
		
		for (int i = 0; i < blueprint.nodes.Count; i++) {
			CreateNewNode( blueprint.nodes[i] );
		}

		for (int i = 0; i < nodes.Count; i++) {
			for (int j = 0; j < nodes.Count; j++) {
				if( blueprint.connectionMatrix[i * nodes.Count + j] == true ){
					nodes[i].AddChild(nodes[j]);
				}
			}
		}

		btName = blueprint.name;

		UpdateMethodOptions();

	}

	void DeleteBlueprint(){
		if(blueprintToLoad != null){
			AssetDatabase.DeleteAsset("Assets/BeeHIVE/Trees/" + blueprintToLoad.name + "_tree.asset");
			AssetDatabase.DeleteAsset("Assets/BeeHIVE/Blueprints/" + blueprintToLoad.name + ".asset");

			blueprintToLoad = null;
			skipNodeDrawing = true;
			AssetDatabase.Refresh();
			NewTree();

		}
	}

	void CreateConnectionMatrix(TreeBlueprint bluePrint){
		bluePrint.connectionMatrix = new bool[ nodes.Count * nodes.Count];
		for (int i = 0; i < nodes.Count; i++) {
			for (int j = 0; j < nodes.Count; j++) {
				if(nodes[i].children.Contains(nodes[j])){
					bluePrint.connectionMatrix[i * nodes.Count + j] = true;
				}
				else{
					bluePrint.connectionMatrix[i * nodes.Count + j] = false;
				}
			}
		}
	}


	bool CheckForClickOnNode(){	 
		
		for (int i = 0; i < nodes.Count; i++) {
			if(nodes[i].windowRect.Contains(mousePosition)){
				selectedNode = nodes[i];
				return true;
			}
		}

		return false;
	}

	public void OnBottonConnectorClicked(object source, EventArgs args){
		selectedNode = source as BeeHiveNode;
		makeTransitionMode = true;
	}

	public IEnumerable<Type> FindDerivedTypes()
	{
		Type baseType = typeof(BeeHIVEAgent); 
		return baseType.Assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && !t.Equals(baseType));
	}

	void UpdateMethodOptions(){
		methodOptions = new List<string>();
		if(sourceType==null){
			foreach (Type t in derivedTypes) {
				foreach (MethodInfo m in t.GetMethods()) {	
					if(m.ReturnType.Equals(typeof(BH_Status))){
						methodOptions.Add(m.Name);
					}
				}	
			}
		}
		else{
			foreach (MethodInfo m in sourceType.GetMethods()) {	
				if(m.ReturnType.Equals(typeof(BH_Status))){
					methodOptions.Add(m.Name);
				}
			}	
		}
	}

	void UpdateNodesMethodsIndexes(){
		foreach(BeeHiveNode b in nodes){
			b.UpdateMethodIndex();
		}
	}

	List<BeeHIVEAgent> GetReferencesThatUse(string treeName){
		List<BeeHIVEAgent> matches = new List<BeeHIVEAgent>();

		BeeHIVEAgent[] agents = Resources.FindObjectsOfTypeAll(typeof(BeeHIVEAgent)) as BeeHIVEAgent[];

		foreach (BeeHIVEAgent b in agents)
		{
			if (b.behaviorTreeObj != null && string.Compare(b.behaviorTreeObj.name, treeName) == 0){
				matches.Add(b);
			}
		}

		return matches;
	}

	void UpdateReferences(List<BeeHIVEAgent> agents, string treeName){
		ScriptableTree tree = AssetDatabase.LoadAssetAtPath<ScriptableTree>("Assets/BeeHIVE/Trees/" + treeName + ".asset");
		foreach(BeeHIVEAgent a in agents){			
			a.behaviorTreeObj = tree;
			EditorUtility.SetDirty(a);
		}
	}
}

