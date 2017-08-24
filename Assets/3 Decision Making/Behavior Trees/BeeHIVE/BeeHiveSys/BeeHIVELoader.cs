using UnityEngine;
using System.Collections;
using BeeHive;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class BeeHIVELoader  {
	

	public static BH_BehaviorTree LoadTree(ScriptableTree tree){
		BH_BehaviorTree t;

		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream s = new MemoryStream(tree.behaviorTreeData);

		t = (BH_BehaviorTree)bf.Deserialize(s);
		s.Close();

		return t;
	}

}

public static class BTGuiLoader{
	public static void initGUILoader(){
		textures[0] = Resources.Load ("BTBackg") as Texture;
		textures[1] = Resources.Load ("BeeHiveLogo") as Texture;
		textures[2] = Resources.Load ("blueprint") as Texture;
		textures[3] = Resources.Load ("selector") as Texture;
		textures[4] = Resources.Load ("sequence") as Texture;
		textures[5] = Resources.Load ("inverter") as Texture;
		textures[6] = Resources.Load ("succeder") as Texture;
		textures[7] = Resources.Load ("repeater") as Texture;
		textures[8] = Resources.Load ("repeatUntilFail") as Texture;
		textures[9]  = Resources.Load ("leaf") as Texture;
		textures[10] = Resources.Load ("delete") as Texture; 
	}
	public static Texture[] textures = new Texture[11];

	public static Texture GetTexture(E_TextureNames texName){
		return textures[(int)texName];
	}

}
public enum E_TextureNames{backTex, logo, blueprintIcon, selectorIcon, sequenceIcon, inverterIcon, succederIcon, repeaterIcon, repeatTilFailIcon, leafIcon, deleteIcon};