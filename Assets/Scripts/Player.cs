using UnityEngine;
using System.Collections;
using System.Timers;

public class Player : MonoBehaviour
{
	
	GameObject oPlayer;
	GameObject oCamera;
	GameObject oBubble;
	GameObject oBlackPlane;
	
	Grid grid;
	Dashboard dashboard;
	
	public Texture2D MainBgPic;
	public GUISkin MenuGUIskins;
	public GameObject projectilePrefab;
	public GameObject Bomb;
	public GameObject Spear;
	public WeaponType currentWeapon;
	public int bomb_num = 0;
	public int spear_num = 0;
	int iLife;
	public float iOxygen;
	public float iFuel;
	
	bool blink = false;
	float timeCount = 3.0f;

	public bool isPaused = false;
	private float nativeVerticalResolution = 1200.0f;
	private float pause_background;
		
	void Start () {
		
		iLife = 3;
		iOxygen = 30;
		iFuel = 15;
		
		oPlayer = GameObject.Find ("Player");
		oCamera = GameObject.Find ("Main Camera");
		oBubble = GameObject.Find ("Bubbles");
		oBlackPlane = GameObject.Find ("BlackPlane");
		currentWeapon = WeaponType.noWeapon;
		
		grid = oCamera.GetComponent("Grid") as Grid;
		dashboard = oCamera.GetComponent("Dashboard") as Dashboard;
	}
	
	void Update () {
		
		if (Input.GetKeyDown("escape")) {
			
			if (!isPaused) {
				pause_background = oBlackPlane.renderer.material.color.a;
				oBlackPlane.renderer.material.color = new Color (0, 0, 0, 0.8f);
				isPaused = true;
			}
			else {
				oBlackPlane.renderer.material.color = new Color (0, 0, 0, pause_background);
				isPaused = false;    
			}
		}
		
		if (!isPaused) {
		
			oPlayer.transform.Translate (0, grid.gameSpeed, 0);
			oBubble.transform.Translate (0, grid.gameSpeed, 0);		
		
			if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.DivingUp) {
				iOxygen -= Time.deltaTime;
			
				if (iOxygen <= 0) {
					Application.LoadLevel ("GameOver");
					Debug.Log ("run out of oxygen");
				}
			}
		
			if (grid.CurrentDirection == GameDirection.FlyingUp)
				iFuel -= Time.deltaTime;
		
			if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.FlyingDown) {
				if (Input.GetKey (KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -17) {
					oPlayer.transform.Translate (-0.5F, 0, 0);
					oBubble.transform.Translate (-0.5F, 0, 0);
				} else if (Input.GetKey (KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 17) {
					oPlayer.transform.Translate (0.5F, 0, 0);
					oBubble.transform.Translate (0.5F, 0, 0);
				}
			} else if (grid.CurrentDirection == GameDirection.DivingUp || grid.CurrentDirection == GameDirection.FlyingUp) {
				if (Input.GetKey (KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -17) {
					oPlayer.transform.Translate (0.5F, 0, 0);
					oBubble.transform.Translate (0.5F, 0, 0);
				} else if (Input.GetKey (KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 17) {
					oPlayer.transform.Translate (-0.5F, 0, 0);
					oBubble.transform.Translate (-0.5F, 0, 0);
				}			
			}
		
			if (Input.GetKey (KeyCode.UpArrow) && (oPlayer.transform.localPosition.y - oCamera.transform.localPosition.y) < 7.5) {
				oPlayer.transform.Translate (0, 0.3F, 0);
				oBubble.transform.Translate (0, 0.3F, 0);
			} else if (Input.GetKey (KeyCode.DownArrow) && (oPlayer.transform.localPosition.y - oCamera.transform.localPosition.y) > -7.5) {
				oPlayer.transform.Translate (0, -0.3F, 0);
				oBubble.transform.Translate (0, -0.3F, 0);
			}
		
			if (Input.GetKeyDown (KeyCode.Tab)) {
				if (grid.CurrentDirection == GameDirection.DivingDown) {
					grid.CurrentDirection = GameDirection.DivingUp;
					oPlayer.transform.Rotate (0, 180.0f, 0);
				}
			}
		
			if (grid.CurrentDirection == GameDirection.DivingUp && oPlayer.transform.localPosition.y >= 0) {
				grid.CurrentDirection = GameDirection.FlyingUp;
				oBubble.renderer.enabled = false;
				Debug.Log ("change to flying up");
			}
		
			if (grid.CurrentDirection == GameDirection.FlyingUp && iFuel <= 0) {
				grid.CurrentDirection = GameDirection.FlyingDown;
				oPlayer.transform.Rotate (0, 180.0f, 0);
				//open parachute
				Debug.Log ("change to flying down");
			}
		
			if (grid.CurrentDirection == GameDirection.FlyingDown && oPlayer.transform.localPosition.y <= 0) {
				grid.CurrentDirection = GameDirection.GameOver;
				Debug.Log ("return to surface");
			}
		
			//using weapon
			if (Input.GetKeyDown (KeyCode.Space)) {
			
				if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.FlyingDown) {
					if (currentWeapon == WeaponType.Gun) {
						Instantiate (projectilePrefab, oPlayer.transform.localPosition, Quaternion.identity);
					}
				
					if (currentWeapon == WeaponType.Bomb && bomb_num > 0) {
						Instantiate (Bomb, oPlayer.transform.localPosition, Quaternion.identity);
						bomb_num --;
					}
	
					if (currentWeapon == WeaponType.Spear && spear_num > 0) {
						Instantiate (Spear, oPlayer.transform.localPosition, Quaternion.identity);
						spear_num --;
					}
				}
			}
		
			GameObject[] weapons = GameObject.FindGameObjectsWithTag ("Weapon") as GameObject[];
			foreach (GameObject w in weapons) {
				w.transform.Translate (0, -0.2F, 0);
			}	
					
			if (Blink) {
				timeCount -= Time.deltaTime;
				
				if (timeCount <= 0)
					Blink = false;
			}
		}
	}

	void OnGUI () {
		// Set up gui skin
		GUI.skin = MenuGUIskins;
		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
		myButtonStyle.fontSize = 50;
		
		// Load and set Font
		Font myFont = (Font)Resources.Load("Font/HoboStd", typeof(Font));
		myButtonStyle.font = myFont;
		
		// Set color for selected and unselected buttons
		myButtonStyle.normal.textColor = Color.white;
		myButtonStyle.hover.textColor = Color.red;
		
		
		// Our GUI is laid out for a 1920 x 1200 pixel display (16:10 aspect). The next line makes sure it rescales nicely to other resolutions.
		GUI.matrix = Matrix4x4.TRS (new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity, new Vector3 (Screen.height / nativeVerticalResolution, Screen.height / nativeVerticalResolution, 1)); 
 
 
		if (isPaused) {
			// RenderSettings.fogDensity = 1;
			if (MainBgPic != null) {
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), MainBgPic);
			}
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height - 260) / 2, 400, 200), "Quit",myButtonStyle)) {
				//print ("Quit!");
				Application.Quit ();
			}
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height +140) / 2, 400, 200), "Restart",myButtonStyle)) {
				//print ("Restart");
				Application.LoadLevel ("main");
				Time.timeScale = 1.0f;
				isPaused = false;
			}
			if (GUI.Button (new Rect ((Screen.width + 200) / 2, (Screen.height + 540) / 2, 400, 200), "Continue",myButtonStyle)) {
				//print ("Continue");
				Time.timeScale = 1.0f;
				isPaused = false;   
			}
		} 
 
 
	}

	void OnTriggerEnter (Collider collider) {
		
		if (collider.gameObject.tag == "Creature") {
			
			Grid screenGrid = oCamera.GetComponent ("Grid") as Grid;
			screenGrid.whenCollide (int.Parse (collider.gameObject.name), collider.gameObject.transform.localPosition.y, 1);
		}
		if (collider.gameObject.tag == "TreasureBox") {
			Grid screenGrid = oCamera.GetComponent ("Grid") as Grid;
			int weapon = screenGrid.whenCollide (int.Parse (collider.gameObject.name), collider.gameObject.transform.localPosition.y, 0);
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
			Grid screenGrid = oCamera.GetComponent ("Grid") as Grid;
			screenGrid.whenCollide (int.Parse (collider.gameObject.name), collider.gameObject.transform.localPosition.y, 2);
			iOxygen = 30.0f;
		}
	}
	
	public int Life {
		get {
			return iLife;
		}
		
		set {
			if (iLife > value) {
				iLife--;
				Blink = true;
				dashboard.updateLife(iLife);
				currentWeapon = WeaponType.noWeapon;
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
					timeCount = 3.0f;
					animation.Play();
				}
			}
		}
	}
	
}
