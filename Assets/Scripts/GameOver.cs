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
		
		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);

        // Load and set Font
        Font myFont = (Font)Resources.Load("Font/HoboStd", typeof(Font));
        myButtonStyle.font = myFont;
		myButtonStyle.fontSize = 40;
        // Set color for selected and unselected buttons
        myButtonStyle.normal.textColor = Color.gray;
        myButtonStyle.hover.textColor = Color.white;
		//audio.Play();
		//oaudio.audio.Play();
		if (MainBgPic != null) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
		}
		if (GUI.Button (new Rect ((Screen.width - 300) / 2, (Screen.height - 300) / 2, 300, 100), "Replay!", myButtonStyle)) {
			Application.LoadLevel ("Main");
		}
		
		if (GUI.Button (new Rect ((Screen.width - 300) / 2, (Screen.height - 50) / 2, 300,100), "Mainmenu", myButtonStyle)) {
			Application.LoadLevel ("MainMenu");
		}
		;
		if (GUI.Button (new Rect ((Screen.width - 300) / 2, (Screen.height + 200) / 2, 300, 100), "Exit", myButtonStyle)) {
			Application.Quit();
		}
	}
}
