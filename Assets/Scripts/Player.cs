using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

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
    public PlayerEffect currentEffect;
    public float weaponCount = 0;
    public float effectCount = 0;

    int iLife;
    public float fOxygen;
    public float fFuel;
	
    bool blink = false;
    float blinkTimeCount = 0;

    public bool isPaused = false;
    private float nativeVerticalResolution = 1200.0f;
    private float pause_background;

    void Start () {

        iLife = 3;
        fOxygen = 30;
        fFuel = 15;

        oPlayer = GameObject.Find ("Player");
        oCamera = GameObject.Find ("Main Camera");
        oBubble = GameObject.Find ("Bubbles");
        oBlackPlane = GameObject.Find ("BlackPlane");
        currentWeapon = WeaponType.noWeapon;
        currentEffect = PlayerEffect.noEffect;

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
			
			
            oPlayer.transform.Translate(0, grid.GameSpeed, 0);
            oBubble.transform.Translate(0, grid.GameSpeed, 0);

            //apply player effect
            if (currentEffect != PlayerEffect.noEffect) {

                effectCount -= Time.deltaTime;

                if (effectCount <= 0) {

                    if (currentEffect == PlayerEffect.SlowDown || currentEffect == PlayerEffect.SpeedUp) {

                        grid.speedFactor = 1;
                    }

                    if (currentEffect == PlayerEffect.Bigger)
                        oPlayer.transform.localScale = new Vector3(3.0f, 3.0f, 0.0001f);				

                    currentEffect = PlayerEffect.noEffect;
                    dashboard.updateEffectIcon();
                }
            }

            if (currentWeapon != WeaponType.noWeapon) {

                if (weaponCount <= 0) {

                    currentWeapon = WeaponType.noWeapon;
                    dashboard.updateWeaponIcon();
                }
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

            if (Input.GetKey (KeyCode.UpArrow) && (oPlayer.transform.localPosition.y - oCamera.transform.localPosition.y) < 7.5) {
                oPlayer.transform.Translate (0, 0.3F, 0);
                oBubble.transform.Translate (0, 0.3F, 0);
            } else if (Input.GetKey (KeyCode.DownArrow) && (oPlayer.transform.localPosition.y - oCamera.transform.localPosition.y) > -7.5) {
                oPlayer.transform.Translate (0, -0.3F, 0);
                oBubble.transform.Translate (0, -0.3F, 0);
            }

            //changing game direction
            if (Input.GetKeyDown(KeyCode.Tab)) {

                if (grid.CurrentDirection == GameDirection.DivingDown) {
                    grid.CurrentDirection = GameDirection.DivingUp;
                    //oPlayer.transform.Rotate(0,180.0f,0);
                }
            }

            if (grid.CurrentDirection == GameDirection.DivingUp && oPlayer.transform.localPosition.y >= 0) {
                grid.CurrentDirection = GameDirection.FlyingUp;
                oBubble.renderer.enabled = false;
                if (fOxygen > 0) {

                    fFuel += fOxygen;
                    if (fFuel > 30)
                        fFuel = 30;
                }
                Debug.Log ("change to flying up");
            }

            if (grid.CurrentDirection == GameDirection.FlyingUp && fFuel <= 0) {
                grid.CurrentDirection = GameDirection.FlyingDown;
                //oPlayer.transform.Rotate(0,180.0f,0);
                //open parachute
                Debug.Log("change to flying down");
            }

            if (grid.CurrentDirection == GameDirection.FlyingDown && oPlayer.transform.localPosition.y <= 0) {
                grid.CurrentDirection = GameDirection.GameOver;
                Debug.Log ("return to surface");
            }
			if (grid.CurrentDirection == GameDirection.GameOver)
				Application.LoadLevel("GameFinish");

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

            //updating weapons
            GameObject[] weapons = GameObject.FindGameObjectsWithTag ("Weapon") as GameObject[];
            foreach (GameObject w in weapons) {
                w.transform.Translate (0, -0.2F, 0);
            }	

            //blinking effect
            if (Blink) {

                blinkTimeCount -= Time.deltaTime;

                if (blinkTimeCount <= 0)
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

            Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
            screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, CellType.Creature);
        }

        if (collider.gameObject.tag == "TreasureBox") {

            Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
            TreasureType type = (TreasureType)screenGrid.whenCollide(int.Parse(collider.gameObject.name), 
                    collider.gameObject.transform.localPosition.y, CellType.Treasure);

            switch (type) {

                case TreasureType.Gun:
                    resetWeapon();
                    currentWeapon = WeaponType.Gun;
                    dashboard.updateWeaponIcon();
                    weaponCount = 10;
                    break;

                case TreasureType.Bomb:
                    resetWeapon();
                    currentWeapon = WeaponType.Bomb;
                    dashboard.updateWeaponIcon();
                    weaponCount = 3;
                    break;

                case TreasureType.Spear:
                    resetWeapon();
                    currentWeapon = WeaponType.Spear;
                    dashboard.updateWeaponIcon();
                    weaponCount = 5;
                    break;

                case TreasureType.Inverse:
                    resetEffect();
                    currentEffect = PlayerEffect.Inverse;
                    dashboard.updateEffectIcon();
                    effectCount = 10;
                    break;

                case TreasureType.Undefeat:
                    resetEffect();
                    currentEffect = PlayerEffect.Undefeat;
                    dashboard.updateEffectIcon();
                    effectCount = 10;
                    break;

                case TreasureType.SlowDown:
                    resetEffect();
                    currentEffect = PlayerEffect.SlowDown;
                    dashboard.updateEffectIcon();
                    grid.speedFactor = 0.5f;
                    break;

                case TreasureType.SpeedUp:
                    resetEffect();
                    currentEffect = PlayerEffect.SpeedUp;
                    dashboard.updateEffectIcon();
                    grid.speedFactor = 2;
                    effectCount = 10;
                    break;

                case TreasureType.Bigger:
                    resetEffect();
                    oPlayer.transform.localScale += new Vector3(1.0f ,1.0f ,0.001f);
                    currentEffect = PlayerEffect.Bigger;
                    effectCount = 10;
                    break;

                case TreasureType.Dark:
                    resetEffect();
                    oBlackPlane.renderer.material.color = new Color (0, 0, 0, 0.6f);
                    currentEffect = PlayerEffect.Dark;
                    effectCount = 3;
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

    void resetWeapon() {

    }

    void resetEffect() {

        grid.speedFactor = 1;
        oPlayer.transform.localScale = new Vector3(3.0f, 3.0f, 0.0001f);	
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
                    resetWeapon();
                    resetEffect();
                    dashboard.updateWeaponIcon();
                    dashboard.updateEffectIcon();
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
