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
		myButtonStyle.fontSize = 40;
        // Set color for selected and unselected buttons
        myButtonStyle.normal.textColor = Color.gray;
        myButtonStyle.hover.textColor = Color.white;
		
		if (MainBgPic != null) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
		}
		
		if (GUI.Button (new Rect ((Screen.width - 900) / 2, (Screen.height + 300) / 2, 200, 100), "Start!", myButtonStyle)) {
			Button2 = true;
		}
		
		if (GUI.Button (new Rect ((Screen.width - 300) / 2, (Screen.height + 300) / 2, 300, 100), "Game Facts", myButtonStyle)) {
				Application.LoadLevel ("Tutor");
		}				
		if (GUI.Button (new Rect ((Screen.width + 500) / 2, (Screen.height + 300) / 2, 200, 100), "Exit", myButtonStyle)) {
				Application.Quit ();
		}
		
		if (Button2) {
			if (GUI.Button (new Rect ((Screen.width - 1100) / 2, (Screen.height + 550) / 2, 300, 100), "Easy Level", myButtonStyle)) {				
				ApplicationModel.levelPath = "Assets/Resources/level1.txt";
				Application.LoadLevel ("Main");
			}
			if (GUI.Button (new Rect ((Screen.width - 300) / 2, (Screen.height + 550) / 2, 300, 100), "Mid Level", myButtonStyle)) {				
				
				ApplicationModel.levelPath = "Assets/Resources/level2.txt";
				Application.LoadLevel ("Main");
			}
			if (GUI.Button (new Rect ((Screen.width + 500) / 2, (Screen.height + 550) / 2, 300, 100), "Hard Level", myButtonStyle)) {				
				
				ApplicationModel.levelPath = "Assets/Resources/level3.txt";
				Application.LoadLevel ("Main");
			}			
		}
	}

	void Start () {
		oback = GameObject.Find ("Plane");
		oback2 = GameObject.Find ("Plane2");
		//oaudio=GameObject.Find("audio");
	}
	
	void Awake() {
		QualitySettings.SetQualityLevel(4);
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
