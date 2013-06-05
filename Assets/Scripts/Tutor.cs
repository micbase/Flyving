using UnityEngine;
using System.Collections;

public class Tutor : MonoBehaviour {

    public Texture2D MainBgPic;
    public GUISkin MenuGUIskins;
	Texture arrow_down;
	Texture arrow_up;
	Texture Back_button;
	bool go_down = false;
	bool go_up = false;
	GameObject oCamera;
	GameObject ofinalPlane;
    void Start () {
		oCamera = GameObject.Find("Main Camera");
		ofinalPlane = GameObject.Find("Plane12");
		arrow_down = Resources.Load("Textures/arrow_down",typeof(Texture)) as Texture;
		arrow_up = Resources.Load("Textures/arrow_up",typeof(Texture)) as Texture;
		Back_button = Resources.Load("Textures/Backbutton",typeof(Texture)) as Texture;
		
    }

    void OnGUI ()
    {
        GUI.skin = MenuGUIskins;
        if (MainBgPic != null) {
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
        }
        if (GUI.Button (new Rect (0.0f, (Screen.height) * 0.9f, 150, 80), Back_button)) {
            Application.LoadLevel ("MainMenu");
        }
		if ((oCamera.transform.localPosition.y - ofinalPlane.transform.localPosition.y) > 1.0f)
		if (GUI.Button(new Rect((Screen.width) * 0.5f, (Screen.height) * 0.9f, 150, 80),arrow_down)) {
			go_down = true;
		}
		if (oCamera.transform.localPosition.y < -5.0f)
		if (GUI.Button(new Rect((Screen.width) * 0.5f, 0.0f, 150, 80),arrow_up)) {
			go_up = true;
		}
		if (go_down) {
			
			  oCamera.transform.localPosition -= new Vector3 (0.0f, 0.1f, 0.0f) ;
			if ((- oCamera.transform.localPosition.y) % 11.5 <= 0.1)
				go_down = false;
		}
		if (go_up) {
			
			  oCamera.transform.localPosition += new Vector3 (0.0f, 0.1f, 0.0f) ;
			if ((- oCamera.transform.localPosition.y % 11.5) <= 0.1)
				go_up = false;
		}
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) 
            Application.LoadLevel("MainMenu");
		if (oCamera.transform.localPosition.y < -5.0f)
			if (Input.GetKeyDown(KeyCode.UpArrow))
				go_up = true;
		if ((oCamera.transform.localPosition.y - ofinalPlane.transform.localPosition.y) > 1.0f)
			if (Input.GetKeyDown(KeyCode.DownArrow))
				go_down = true;
			


    }
}
