using UnityEngine;
using System.Collections;

public class Dashboard : MonoBehaviour {

    public int iScore;
    GameObject oScore;
    GameObject oOxygen;

    GameObject[] oLife;
    GameObject oAlert;

    Player player;
    int Max_Lives = 3;

    private float height;
    private float width;
    private float x;
    private float y;
    private float currentx;
    private float currenty;
    private float currentz;
    private float currentheight;
    private int checkalert = 0;


    void Start () {

        oScore = GameObject.Find("Score");
        oOxygen = GameObject.Find("Oxygenbar");

        player = GameObject.Find("Player").GetComponent("Player") as Player;

        oAlert = GameObject.Find ("Alert");
        oAlert.guiText.material.color = Color.grey;
        oAlert.guiText.fontSize = 1;

        Texture2D texture = (Texture2D)Resources.Load ("Textures/heart-icon");
        oLife = new GameObject[Max_Lives];
        for (int i = 0; i < Max_Lives; i++) {

            oLife [i] = new GameObject ("Life");
            oLife [i].AddComponent ("GUITexture");
            oLife [i].guiTexture.texture = texture;
            oLife [i].gameObject.guiTexture.pixelInset = new Rect (0, 0, 32, 32);
            oLife [i].transform.localPosition = new Vector3 (0.02f + i * 0.05f, 0.87f, 1);
            oLife [i].transform.localScale = new Vector3 (0, 0, 1);
        }

        iScore = 0;
        oScore.guiText.text = "30";

        height = oOxygen.guiTexture.pixelInset.height;
        width = oOxygen.guiTexture.pixelInset.width;
        x = oOxygen.guiTexture.pixelInset.x;
        y = oOxygen.guiTexture.pixelInset.y;
    }

    void Update () {

        if (!player.isPaused) {

            oScore.guiText.text = "Score: " + iScore.ToString ("0");
            currentx = oOxygen.transform.localPosition.x;
            currenty = oOxygen.transform.localPosition.y + 0.000035f;
            currentz = oOxygen.transform.localPosition.z;
            oOxygen.transform.localPosition = new Vector3 (currentx, currenty, currentz);

            currentheight = height * player.fOxygen * 3.33F / 100;
            oOxygen.guiTexture.pixelInset = new Rect (x, y, width, currentheight);
            if ((currentheight / height) <= 0.7f) {
                oAlert.guiText.fontSize = 25;
                if (checkalert % 2 == 0) {
                    oAlert.guiText.material.color = Color.red;				
                } else {
                    oAlert.guiText.material.color = Color.yellow;				
                }
                checkalert++;
            } else {
                oAlert.guiText.material.color = Color.grey;
                oAlert.guiText.fontSize = 1;
                checkalert = 0;

            }	
        }
    }

    public void updateLife (int life) {

        for (int i = 0; i < life; i++) {
            oLife [i].guiTexture.enabled = true;
        }

        for (int i = life; i < Max_Lives; i++) {
            oLife [i].guiTexture.enabled = false;
        }	
    }

}
