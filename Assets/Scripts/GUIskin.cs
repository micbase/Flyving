using UnityEngine;
using System.Collections;

public class GUIskin : MonoBehaviour {
	
	public GUISkin MenuGUIskins;
	Texture image;
	//float transparency;
	// Use this for initialization
	void Start () {
		image = Resources.Load("Textures/arrow",typeof(Texture)) as Texture;
	
	}
	void OnGUI () {
		GUI.skin = MenuGUIskins;
		GUI.Button(new Rect(10, 10, 80, 48),image);
		//guiTexture.color.a = 0.6;
		
	}

}
