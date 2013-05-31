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
	
	private int getTreasure = 0;  // 0: No Treasure ; 1: Weapon ; 2: Effect
	private float timer_getTreasure = 0.0f;
	private int AnimationTimes = 0;
	private bool beginAnimation = false;
	float IconWidth;
	float IconHeight;
	
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
		
		oEffectIcon.guiTexture.texture = null;
		oWeaponIcon.guiTexture.texture = null;
    }

    void Update () {

        if (!player.isPaused) {
			
			if (!beginAnimation) {
				timer_getTreasure = 0.0f;
				AnimationTimes = 0;
				oWeaponIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,32.0f,32.0f);
				oWeaponIcon.guiTexture.transform.localPosition = new Vector3(0.02f,0.03f,0.0f);
				oEffectIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,32.0f,32.0f);
				oEffectIcon.guiTexture.transform.localPosition = new Vector3(0.02f,0.1f,0.0f);
			}
			else {
			
				if (getTreasure == 1) {
					timer_getTreasure += Time.deltaTime;
					oWeaponIcon.guiTexture.transform.localPosition = new Vector3(0.5f,0.5f,0.0f);
					IconWidth = oWeaponIcon.guiTexture.pixelInset.width; 
					IconHeight = oWeaponIcon.guiTexture.pixelInset.height; 
					if (timer_getTreasure < 0.2f) {
						oWeaponIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,IconWidth+20.0f,IconHeight+20.0f);
						AnimationTimes++;
					}
					else if (timer_getTreasure >= 0.3f &&	AnimationTimes > 0) {
						oWeaponIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,IconWidth-20.0f,IconHeight-20.0f);
						AnimationTimes--;
					}
					else if (timer_getTreasure >= 0.5f) {
						timer_getTreasure = 0.0f;
						getTreasure = 0;
						oWeaponIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,32.0f,32.0f);
						oWeaponIcon.guiTexture.transform.localPosition = new Vector3(0.02f,0.03f,0.0f);
						beginAnimation = false;
					}
						
				}
				
				if (getTreasure == 2) {
					timer_getTreasure += Time.deltaTime;
					oEffectIcon.guiTexture.transform.localPosition = new Vector3(0.5f,0.5f,0.0f);
					IconWidth = oEffectIcon.guiTexture.pixelInset.width; 
					IconHeight = oEffectIcon.guiTexture.pixelInset.height; 
					if (timer_getTreasure < 0.2f) {
						oEffectIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,IconWidth+20.0f,IconHeight+20.0f);
						AnimationTimes++;
					}
					else if (timer_getTreasure >= 0.4f &&	AnimationTimes > 0) {
						oEffectIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,IconWidth-20.0f,IconHeight-20.0f);
						AnimationTimes--;
					}
					else if (timer_getTreasure >= 0.6f) {
						timer_getTreasure = 0.0f;
						getTreasure = 0;
						oEffectIcon.guiTexture.pixelInset = new Rect(0.0f,0.0f,32.0f,32.0f);
						oEffectIcon.guiTexture.transform.localPosition = new Vector3(0.02f,0.1f,0.0f);
						beginAnimation = false;
					}
				}
			}
			
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
                oProgress.guiTexture.pixelInset = new Rect (0, 0, 38, player.fFuel / 10 * 200);
                oProgress.guiTexture.texture = texture_yellow;
            }

            //update alert
            if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.DivingUp) {
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
		
		if (player.currentWeapon != WeaponType.noWeapon) {
			beginAnimation = false;
		}
		
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
		
		if (player.currentWeapon != WeaponType.noWeapon) {
			getTreasure = 1;
			beginAnimation = true;
		}
    }

    public void updateEffectIcon() {
		
		if (player.currentEffect != PlayerEffect.noEffect) {
			beginAnimation = false;
		}
		
        switch (player.currentEffect) {

            case PlayerEffect.Inverse:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/effect/inverse");
                break;

            case PlayerEffect.Undefeat:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/effect/undefeat");
                break;

            case PlayerEffect.SlowDown:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/effect/slowdown");
                break;

            case PlayerEffect.SpeedUp:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/effect/speedup");
                break;
			
			case PlayerEffect.Dark:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/effect/dark");
                break;
			
			case PlayerEffect.Bigger:
                oEffectIcon.guiTexture.texture = (Texture2D)Resources.Load ("Textures/effect/bigger");
                break;
			
            default:
                oEffectIcon.guiTexture.texture = null;
                break;
        }
		
		if (player.currentEffect != PlayerEffect.noEffect) {
			getTreasure = 2;
			beginAnimation = true;
		}
    }

}
