using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	public Texture2D MainBgPic;
	public GUISkin MenuGUIskins;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI ()
	{
		GUI.skin = MenuGUIskins;
		//audio.Play();
		//oaudio.audio.Play();
		if (MainBgPic != null) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
		}
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 260) / 2, 150, 80), "Replay!")) {
			Application.LoadLevel ("Main");
		}
		
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 80) / 2, 150, 80), "Mainmenu")) {
			Application.LoadLevel ("MainMenu");
		}
		;
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height + 100) / 2, 150, 80), "Exit")) {
			Application.Quit();
		}
	}
}
