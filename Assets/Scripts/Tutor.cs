using UnityEngine;
using System.Collections;

public class Tutor : MonoBehaviour {

    public Texture2D MainBgPic;
    public GUISkin MenuGUIskins;

    void Start () {

    }

    void OnGUI ()
    {
        GUI.skin = MenuGUIskins;
        if (MainBgPic != null) {
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
        }
        if (GUI.Button (new Rect ((Screen.width) * 0.15f, (Screen.height) * 0.8f, 150, 80), "Go Back")) {
            Application.LoadLevel ("MainMenu");
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) 
            Application.LoadLevel("MainMenu");

    }
}
