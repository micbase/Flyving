using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	GameObject oPlayer;
	// Use this for initialization
	void Start () {
		oPlayer = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
				
		if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -15)
			oPlayer.transform.Translate(-0.5F,0,0);
		
		else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 15)
			oPlayer.transform.Translate(0.5F,0,0);
	}
}