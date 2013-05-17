using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	GameObject oPlayer;
	GameObject oCamera;
	GameObject oBubble;	
	
	Grid grid;

	public GameObject projectilePrefab;
	public GameObject Bomb;
	public GameObject Spear;
	
	public int iLife;
	public float iOxygen;
	public float iFuel;

	void Start () {
		
		iLife = 3;
		iOxygen = 30;
		iFuel = 15;
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		oBubble = GameObject.Find("Bubbles");
		
		grid = oCamera.GetComponent("Grid") as Grid;
	}
	
	void Update () {
		
		oPlayer.transform.Translate(0, grid.gameSpeed, 0);
		oBubble.transform.Translate(0, grid.gameSpeed, 0);
		
		if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.DivingUp) {
			iOxygen -= Time.deltaTime;
			
			if (iOxygen <= 0) {
				Application.LoadLevel("GameOver");
				Debug.Log("run out of oxygen");
			}
		}
		
		if (grid.CurrentDirection == GameDirection.FlyingUp)
			iFuel -= Time.deltaTime;
		
		if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -17) {
			oPlayer.transform.Translate(-0.5F, 0, 0);
			oBubble.transform.Translate(-0.5F, 0, 0);
		}
		
		else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 17) {
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
		
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (grid.CurrentDirection == GameDirection.DivingDown) {
				grid.CurrentDirection = GameDirection.DivingUp;
				//oPlayer.transform.Rotate(0,180.0f,0);
			}
		}
		
		if (grid.CurrentDirection == GameDirection.DivingUp && oPlayer.transform.localPosition.y >= 0) {
			grid.CurrentDirection = GameDirection.FlyingUp;
			oBubble.renderer.enabled = false;
			Debug.Log("change to flying up");
		}
		
		if (grid.CurrentDirection == GameDirection.FlyingUp && iFuel <= 0) {
			grid.CurrentDirection = GameDirection.FlyingDown;
			//open parachute
			Debug.Log("change to flying down");
		}
		
		if (grid.CurrentDirection == GameDirection.FlyingDown && oPlayer.transform.localPosition.y <= 0) {
			grid.CurrentDirection = GameDirection.GameOver;
			Debug.Log("return to surface");
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
