using UnityEngine;
using System.Collections;

public class Dashboard : MonoBehaviour {

    public int iScore;
    GameObject oScore;
    GameObject oProgress;

    GameObject[] oLife;
    GameObject oWeaponIcon;
    GameObject oWeaponCount;
    GameObject oEffectIcon;
    GameObject oEffectCount;
    GameObject oAlert;

    Player player;
	Grid grid;
    int Max_Lives = 3;

    Texture2D texture_blue;
    Texture2D texture_yellow;
    Texture2D texture_red;

    private int checkalert = 0;

    void Start () {

        oScore = GameObject.Find("Score");
        oProgress = GameObject.Find("Progressbar");
        oWeaponIcon = GameObject.Find("WeaponIcon");
        oWeaponCount = GameObject.Find("WeaponCount");
        oEffectIcon = GameObject.Find("EffectIcon");
        oEffectCount = GameObject.Find("EffectCount");

        player = GameObject.Find("Player").GetComponent("Player") as Player;
		grid = GameObject.Find("Main Camera").GetComponent("Grid") as Grid;

        oAlert = GameObject.Find ("Alert");
        oAlert.guiText.material.color = Color.grey;
        oAlert.guiText.fontSize = 1;

        Texture2D texture = (Texture2D)Resources.Load ("Textures/heart-icon");
        oLife = new GameObject[Max_Lives];
        for (int i = 0; i < Max_Lives; i++) {

            oLife[i] = new GameObject ("Life");
            oLife[i].AddComponent ("GUITexture");
            oLife[i].guiTexture.texture = texture;
            oLife[i].gameObject.guiTexture.pixelInset = new Rect (0, 0, 32, 32);
            oLife[i].transform.localPosition = new Vector3 (0.02f + i * 0.05f, 0.87f, 1);
            oLife[i].transform.localScale = new Vector3 (0, 0, 1);
        }

        iScore = 0;
        oScore.guiText.text = "30";

        texture_blue = (Texture2D)Resources.Load ("Textures/oxygen_blue");
        texture_yellow = (Texture2D)Resources.Load ("Textures/oxygen_yellow");
        texture_red = (Texture2D)Resources.Load ("Textures/oxygen_red");

    }

    void Update () {

        if (!player.isPaused) {

            //update score
            oScore.guiText.text = "Score: " + iScore.ToString ("0");

            //update progress bar
            if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.DivingUp) {
                oProgress.guiTexture.pixelInset = new Rect (0, 0, 38, player.fOxygen / 30 * 200);

                if (player.fOxygen / 30 < 0.5f)
                    oProgress.guiTexture.texture = texture_red;
                else
                    oProgress.guiTexture.texture = texture_blue;
            }
            else {
                oProgress.guiTexture.pixelInset = new Rect (0, 0, 38, player.fFuel / 30 * 200);
                oProgress.guiTexture.texture = texture_yellow;
            }

            //update alert
            if (player.fOxygen / 30 <= 0.5f) {
				
				oAlert.guiText.enabled = true;
				
                if (checkalert % 2 == 0) {
                    oAlert.guiText.material.color = Color.red;				
                }
                else {
                    oAlert.guiText.material.color = Color.yellow;				
                }
                checkalert++;
            }
            else {
                oAlert.guiText.enabled = false;
                checkalert = 0;
            }

            //update weapon count
            if (player.currentWeapon != WeaponType.noWeapon) {
            	
				if (player.currentWeapon == WeaponType.Gun)
                    oWeaponCount.guiText.text = player.weaponCount.ToString ("0");
                else
                    oWeaponCount.guiText.text = "X " + player.weaponCount.ToString ("0");
            }
            else
                oWeaponCount.guiText.text = "";

            //update effect count
            if (player.currentEffect != PlayerEffect.noEffect) {
                oEffectCount.guiText.text = player.effectCount.ToString ("0");
            }
            else
                oEffectCount.guiText.text = "";
        }
    }

    public void updateLife (int life) {

        for (int i = 0; i < life; i++) {
            oLife[i].guiTexture.enabled = true;
        }

        for (int i = life; i < Max_Lives; i++) {
            oLife[i].guiTexture.enabled = false;
        }	
    }

    public void updateWeaponIcon() {

        switch (player.currentWeapon) {

            case WeaponType.Gun:
                oWeaponIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/gun");
                break;

            case WeaponType.Bomb:
                oWeaponIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/bomb");
                break;

            case WeaponType.Spear:
                oWeaponIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/spear");
                break;

            default:
                oWeaponIcon.guiTexture.texture = null;
                break;
        }
    }

    public void updateEffectIcon() {

        switch (player.currentEffect) {

            case PlayerEffect.Inverse:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/inverse");
                break;

            case PlayerEffect.Undefeat:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/undefeat");
                break;

            case PlayerEffect.SlowDown:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/slowdown");
                break;

            case PlayerEffect.SpeedUp:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/speedup");
                break;

            default:
                oEffectIcon.guiTexture.texture = null;
                break;
        }
    }

}
