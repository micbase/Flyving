using UnityEngine;
using System.Collections;
using System.Timers;

public class Player : MonoBehaviour {
	
	GameObject oPlayer;
	GameObject oCamera;
	GameObject oBubble;	
	Color thecolor;
	
	Grid grid;
	Dashboard dashboard;
	
	public GameObject projectilePrefab;
	public GameObject Bomb;
	public GameObject Spear;
	
	public WeaponType currentWeapon;
	public PlayerEffect currentEffect;
	public float weaponCount = 0;
	public float effectCount = 0;
	
	int iLife;
	public float fOxygen;
	public float fFuel;
	
	bool blink = false;
	float blinkTimeCount = 0;

	void Start () {
		
		iLife = 3;
		fOxygen = 30;
		fFuel = 15;
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		oBubble = GameObject.Find("Bubbles");
		currentWeapon = WeaponType.noWeapon;
		currentEffect = PlayerEffect.noEffect;
		
		grid = oCamera.GetComponent("Grid") as Grid;
		dashboard = oCamera.GetComponent("Dashboard") as Dashboard;

	}
	
	void Update () {
		
		oPlayer.transform.Translate(0, grid.GameSpeed, 0);
		oBubble.transform.Translate(0, grid.GameSpeed, 0);
		
		//apply player effect
		if (currentEffect != PlayerEffect.noEffect) {
			
			effectCount -= Time.deltaTime;
		}
		
		if (effectCount <= 0) {
			
			if (currentEffect == PlayerEffect.SlowDown || currentEffect == PlayerEffect.SpeedUp) {
				
				grid.speedFactor = 1;
			}
			
			currentEffect = PlayerEffect.noEffect;			
		}
		
		if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.DivingUp) {
			
			fOxygen -= Time.deltaTime;
			
			if (fOxygen <= 0) {
				Application.LoadLevel("GameOver");
				Debug.Log("run out of oxygen");
			}
		}
		
		if (grid.CurrentDirection == GameDirection.FlyingUp)
			fFuel -= Time.deltaTime;
		
		if (currentEffect == PlayerEffect.Inverse) {
			
			if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x < 17) {
				oPlayer.transform.Translate(0.5F, 0, 0);
				oBubble.transform.Translate(0.5F, 0, 0);
				}
			else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x > -17) {
				oPlayer.transform.Translate(-0.5F, 0, 0);
				oBubble.transform.Translate(-0.5F, 0, 0);
			}			
		}
		else {
			if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -17) {
				oPlayer.transform.Translate(-0.5F, 0, 0);
				oBubble.transform.Translate(-0.5F, 0, 0);
			}
			else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 17) {
				oPlayer.transform.Translate(0.5F, 0, 0);
				oBubble.transform.Translate(0.5F, 0, 0);
			}
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
		
		if (grid.CurrentDirection == GameDirection.FlyingUp && fFuel <= 0) {
			grid.CurrentDirection = GameDirection.FlyingDown;
			//oPlayer.transform.Rotate(0,180.0f,0);
			//open parachute
			Debug.Log("change to flying down");
		}
		
		if (grid.CurrentDirection == GameDirection.FlyingDown && oPlayer.transform.localPosition.y <= 0) {
			grid.CurrentDirection = GameDirection.GameOver;
			Debug.Log("return to surface");
		}
		
		//using weapon
		if (currentWeapon == WeaponType.Gun) {
			
			weaponCount -= Time.deltaTime;
		}
		
		if (currentWeapon != WeaponType.noWeapon && weaponCount <= 0) {
			
			currentWeapon = WeaponType.noWeapon;
		}
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			
			if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.FlyingDown) {
				if (currentWeapon == WeaponType.Gun && weaponCount > 0) {
					Instantiate(projectilePrefab, oPlayer.transform.localPosition, Quaternion.identity);
				}
				
				if (currentWeapon == WeaponType.Bomb && weaponCount > 0) {
					Instantiate(Bomb, oPlayer.transform.localPosition, Quaternion.identity);
					weaponCount--;
				}
	
				if (currentWeapon == WeaponType.Spear && weaponCount > 0) {
					Instantiate(Spear, oPlayer.transform.localPosition, Quaternion.identity);
					weaponCount--;
				}
			}
		}
		
		GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon") as GameObject[];
		foreach (GameObject w in weapons) {
			w.transform.Translate(0, -0.2F, 0);
		}
		
		if (Blink) {
			blinkTimeCount -= Time.deltaTime;
			
			if (blinkTimeCount <= 0)
				Blink = false;
		}
	}

	void OnTriggerEnter(Collider collider) {
		
		if (collider.gameObject.tag == "Creature") {
			
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, CellType.Creature);
		}
		if (collider.gameObject.tag == "TreasureBox") {
			
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			TreasureType type = (TreasureType)screenGrid.whenCollide(int.Parse(collider.gameObject.name), 
				collider.gameObject.transform.localPosition.y, CellType.Treasure);
			
			switch (type) {
				
			case TreasureType.Gun:
				currentWeapon = WeaponType.Gun;
				weaponCount = 10;
				break;
				
			case TreasureType.Bomb:
				currentWeapon = WeaponType.Bomb;
				weaponCount = 3;
				break;
				
			case TreasureType.Spear:
				currentWeapon = WeaponType.Spear;
				weaponCount = 5;
				break;
				
			case TreasureType.Inverse:
				currentEffect = PlayerEffect.Inverse;
				effectCount = 10;
				break;
				
			case TreasureType.Undefeat:
				currentEffect = PlayerEffect.Undefeat;
				effectCount = 10;
				break;
				
			case TreasureType.SlowDown:
				currentEffect = PlayerEffect.SlowDown;
				grid.speedFactor = 0.5f;
				effectCount = 10;
				break;
				
			case TreasureType.SpeedUp:
				currentEffect = PlayerEffect.SpeedUp;
				grid.speedFactor = 2;
				effectCount = 10;
				break;
			}
		}
		
		if (collider.gameObject.tag == "OxygenCan") {
			
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, CellType.Oxygen);
			
			fOxygen += 10.0f;
			if (fOxygen > 30)
				fOxygen = 30;
		}
	}
	
	public int Life {
		get {
			return iLife;
		}
		
		set {
			if (iLife > value) {
				
				if (!Blink && currentEffect != PlayerEffect.Undefeat) {
					
					iLife--;
					Blink = true;
					dashboard.updateLife(iLife);
					currentWeapon = WeaponType.noWeapon;
					currentEffect = PlayerEffect.noEffect;
					grid.speedFactor = 1;
				}
			}
		}
	}
	
	public bool Blink {
		get{
			return blink;
		}
		set{
			blink = value;
			if(blink){
				if(!animation.isPlaying){
					blinkTimeCount = 3.0f;
					animation.Play();
				}
			}
		}
	}
	
}
