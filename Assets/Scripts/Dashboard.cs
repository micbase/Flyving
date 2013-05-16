using UnityEngine;
using System.Collections;

public class Dashboard : MonoBehaviour {
	
	public int iScore;
	
	GameObject oScore;
	GameObject oOxygen;
	
	Player player;

	private float height;
	private float width;
	private float x;
	private float y;
	private float delta;
	private float currentx;
	private float currenty;
	private float currentz;
	
	void Start () {
		
		oScore = GameObject.Find("Score");
		oOxygen = GameObject.Find("Oxygenbar");
		
		player = GameObject.Find("Player").GetComponent("Player") as Player;
		
		iScore = 0;
		
		oScore.guiText.text = "30";
		
		height = oOxygen.guiTexture.pixelInset.height;
		width = oOxygen.guiTexture.pixelInset.width;
		x = oOxygen.guiTexture.pixelInset.x;
		y = oOxygen.guiTexture.pixelInset.y;
	}
	
	void Update () {
		
		oScore.guiText.text = "Score: " + iScore.ToString("0");
		delta=Time.deltaTime*height*3.33F/100;
		currentx=oOxygen.transform.localPosition.x;
		currenty=oOxygen.transform.localPosition.y+0.000035f;
		currentz=oOxygen.transform.localPosition.z;
		oOxygen.transform.localPosition=new Vector3(currentx,currenty,currentz);

		
		oOxygen.guiTexture.pixelInset = new Rect(x,y,width, height*player.iOxygen*3.33F/100);
	}
}
