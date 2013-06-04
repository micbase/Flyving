using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

	public Texture2D MainBgPic;
	public GUISkin MenuGUIskins;
	GameObject oback;
	GameObject oback2;
	bool Button2;
	//GameObject oaudio;

	void OnGUI () {
		GUI.skin = MenuGUIskins;

		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);

        // Load and set Font
        Font myFont = (Font)Resources.Load("Font/HoboStd", typeof(Font));
        myButtonStyle.font = myFont;

        // Set color for selected and unselected buttons
        myButtonStyle.normal.textColor = Color.white;
        myButtonStyle.hover.textColor = Color.red;
		
		if (MainBgPic != null) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
		}
		if (!Button2) {
			if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 260) / 2, 150, 80), "Quick Start!", myButtonStyle)) {
				Application.LoadLevel ("main");
			}
		
		//if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 80) / 2, 150, 80), "View Scores")) {
		//Application.LoadLevel ("score");
			if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 80) / 2, 150, 80), "Game Facts", myButtonStyle)) {
				Application.LoadLevel ("tutor");
			}
		}
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height + 100) / 2, 150, 80), "Different Level", myButtonStyle)) {
			Button2 = true;
		}
		if (!Button2) {
			if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height + 280) / 2, 150, 80), "Exit", myButtonStyle)) {
				Application.Quit ();
			}
		}
		
		if (Button2) {
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height - 260) / 2, 150, 80), "Level1", myButtonStyle)) {
			}
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height - 80) / 2, 150, 80), "Level2", myButtonStyle)) {				
			}
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height + 100) / 2, 150, 80), "Level3", myButtonStyle)) {				
			}
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height + 280) / 2, 150, 80), "Back", myButtonStyle)) {				
				Button2 = false;
			}
			
		}
	}

	void Start () {
		oback = GameObject.Find ("Plane");
		oback2 = GameObject.Find ("Plane2");
		//oaudio=GameObject.Find("audio");
	}

	void Update () {
		float speed = -0.1f;
		oback.transform.localPosition = new Vector3 (oback.transform.position.x + speed * Time.deltaTime, oback.transform.position.y, oback.transform.position.z);
		oback2.transform.localPosition = new Vector3 (oback2.transform.position.x + speed * Time.deltaTime, oback2.transform.position.y, oback2.transform.position.z);
		if (oback.transform.localPosition.x < -10)
			oback.transform.localPosition = new Vector3 (10, oback.transform.position.y, oback.transform.position.z);
		if (oback2.transform.localPosition.x < -10)
			oback2.transform.localPosition = new Vector3 (10, oback2.transform.position.y, oback2.transform.position.z);
		//oback.transform.Translate(speed,0,0);
	}
}
