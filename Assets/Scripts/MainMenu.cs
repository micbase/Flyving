using UnityEngine;
using System.Collections;
public class MainMenu : MonoBehaviour
{

	public Texture2D MainBgPic;
	public GUISkin MenuGUIskins;
	GameObject oback;
	GameObject oback2;
	//GameObject oaudio;

	void OnGUI ()
	{
		GUI.skin = MenuGUIskins;
		//audio.Play();
		//oaudio.audio.Play();
		if (MainBgPic != null) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
		}
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 260) / 2, 150, 80), "Start!")) {
			Application.LoadLevel ("main");
		}
		
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height - 80) / 2, 150, 80), "Same to above one!")) {
			Application.LoadLevel ("main");
		}
		;
		if (GUI.Button (new Rect ((Screen.width - 150) / 2, (Screen.height + 120) / 2, 150, 80), "Exit")) {
			Application.Quit();
		}
	}
	void Start()
	{
		oback=GameObject.Find("Plane");
		oback2=GameObject.Find("Plane2");
		//oaudio=GameObject.Find("audio");
	}
	void Update()
	{
		float speed=-0.1f;
		oback.transform.localPosition=new Vector3(oback.transform.position.x+speed*Time.deltaTime,oback.transform.position.y,oback.transform.position.z);
		oback2.transform.localPosition=new Vector3(oback2.transform.position.x+speed*Time.deltaTime,oback2.transform.position.y,oback2.transform.position.z);
		if(oback.transform.localPosition.x<-10)
			oback.transform.localPosition=new Vector3(10,oback.transform.position.y,oback.transform.position.z);
		if(oback2.transform.localPosition.x<-10)
			oback2.transform.localPosition=new Vector3(10,oback2.transform.position.y,oback2.transform.position.z);
		//oback.transform.Translate(speed,0,0);
	}
}
