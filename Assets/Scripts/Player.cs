using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	GameObject oPlayer;
	GameObject oCamera;
	GameObject oWater;
	GameObject oBubble;

	public GameObject projectilePrefab;
	public GameObject Bomb;
	public GameObject Spear;
	
	public int iHealth;
	
	float colorspeed = 0.1f;
	float Acolor = 0.0f;
	Color mycolor = new Color(15.0f,17.0f,29.0f,0.0f);


	void Start () {
		
		iHealth = 100;
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		oBubble = GameObject.Find("Bubbles");
		oWater = GameObject.Find ("backPlane");
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
		
		oCamera.transform.Translate(0, -0.1F, 0);
		oPlayer.transform.Translate(0, -0.1F, 0);
		oBubble.transform.Translate(0, -0.1F, 0);
		oWater.transform.Translate(0,0, 0.1F);
		
		if (Acolor<=200.0f)
		{	
			Acolor = Acolor+colorspeed;
			mycolor = new Color(15.0f/255,17.0f/255,29.0f/255,Acolor/255);
			oWater.renderer.material.color = mycolor;
		}

		
		if (Input.GetKeyDown(KeyCode.Space)) {
			Instantiate(projectilePrefab, oPlayer.transform.localPosition, Quaternion.identity);
		}
		
		if (Input.GetKeyDown(KeyCode.B)) {
			Instantiate(Bomb, oPlayer.transform.localPosition, Quaternion.identity);
		}
		
		if (Input.GetKeyDown(KeyCode.V)) {
			Instantiate(Spear, oPlayer.transform.localPosition, Quaternion.identity);
		}
		
		GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon") as GameObject[];
		foreach (GameObject w in weapons) {
			w.transform.Translate(0, -0.2F, 0);
		}
		
	}

	void OnTriggerEnter(Collider collider) {
		
		if (collider.gameObject.tag == "Creature") {
			
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, 1);
		}
	}	
	
}
