using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	GameObject oPlayer;
	GameObject oCamera;
	GameObject oWater;
	
	public GameObject projectilePrefab;
	GameObject oBubble;

	void Start () {
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		oBubble = GameObject.Find("Bubbles");
		oWater = GameObject.Find ("Water");
	}
	
	void Update () {
				
		if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -15) {
			oPlayer.transform.Translate(-0.5F, 0, 0);
			oBubble.transform.Translate(-0.5F, 0, 0);
		}
		
		else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 15) {
			oPlayer.transform.Translate(0.5F, 0, 0);
			oBubble.transform.Translate(0.5F, 0, 0);
		}
		
		if (Input.GetKey(KeyCode.UpArrow) && (oPlayer.transform.localPosition.y - oCamera.transform.localPosition.y) < 7.5)
		{
			oPlayer.transform.Translate(0, 0.3F, 0);
			oBubble.transform.Translate(0, 0.3F, 0);
		}
		else if (Input.GetKey(KeyCode.DownArrow) && (oPlayer.transform.localPosition.y - oCamera.transform.localPosition.y) > -7.5)
		{
			oPlayer.transform.Translate(0, -0.3F, 0);
			oBubble.transform.Translate(0, -0.3F, 0);
		}
		
		oCamera.transform.Translate(0, -0.2F, 0);
		oPlayer.transform.Translate(0, -0.2F, 0);
		oBubble.transform.Translate(0, -0.2F, 0);
		oWater.transform.Translate(0,0,-0.2F); //	
			
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			Instantiate(projectilePrefab, oPlayer.transform.localPosition, Quaternion.identity);
		}
	}

	void OnTriggerEnter(Collider colider) {
		if (colider.gameObject.name == "Cube") {
			Application.LoadLevel("GameOver");
			Debug.Log("die");
		}
	}	
	
}
