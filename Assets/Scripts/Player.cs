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
	public WeaponType currentWeapon;
	public int bomb_num = 0;
	public int spear_num = 0;
	
	float colorspeed = 0.1f;
	float Acolor = 0.0f;
	Color mycolor = new Color(15.0f,17.0f,29.0f,0.0f);


	void Start () {
		
		iHealth = 100;
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		oBubble = GameObject.Find("Bubbles");
		oWater = GameObject.Find ("backPlane");
		currentWeapon = WeaponType.noWeapon;
	}
	
	void Update () {
				
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

		//using weapon
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (currentWeapon == WeaponType.Gun) {
				Instantiate(projectilePrefab, oPlayer.transform.localPosition, Quaternion.identity);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.B)) {
			if (currentWeapon == WeaponType.Bomb && bomb_num>0) {
				Instantiate(Bomb, oPlayer.transform.localPosition, Quaternion.identity);
				bomb_num --;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.V)) {
			if (currentWeapon == WeaponType.Spear && spear_num>0) {
				Instantiate(Spear, oPlayer.transform.localPosition, Quaternion.identity);
				spear_num --;
			}
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
		if (collider.gameObject.tag == "TreasureBox") {
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			int weapon = screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, 0);
			switch (weapon) {
			case 1:
				currentWeapon = WeaponType.Gun;
				break;
			case 2:
				currentWeapon = WeaponType.Bomb;
				bomb_num = 3;
				break;
			case 3:
				currentWeapon = WeaponType.Spear;
				spear_num = 5;
				break;
			default:
				currentWeapon = WeaponType.noWeapon;
				break;
			}
		}
		
		if (collider.gameObject.tag == "OxygenCan") {
				Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
				screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, 2);
				Dashboard.timeCount = 30.0f;
		}
	}	
	
}
