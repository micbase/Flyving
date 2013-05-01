using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	GameObject oPlayer;
	GameObject oCamera;

	void Start () {
		
		oPlayer = GameObject.Find ("Player");
		oCamera = GameObject.Find("Main Camera");
	}
	
	void Update () {
				
		if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -15)
			oPlayer.transform.Translate(-0.5F,0,0);
		
		else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 15)
			oPlayer.transform.Translate(0.5F,0,0);
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			oPlayer.transform.Translate(0,0.3F,0);
			oCamera.transform.Translate(0,0.3F,0);
			
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			oPlayer.transform.Translate(0, -0.3F,0);
			oCamera.transform.Translate(0, -0.3F,0);
		}
		
		oCamera.transform.Translate(0, -0.2F, 0);
		oPlayer.transform.Translate(0, -0.2F, 0);
	}

}