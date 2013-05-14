using UnityEngine;
using System.Collections;

public class Dashboard : MonoBehaviour {
	
	public int iScore;
	
	GameObject oScore;
	GameObject oOxygen;
	float timeCount;
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
		
		iScore = 0;
		
		oScore.guiText.text = "30";
		timeCount = 30;
		
		height = oOxygen.guiTexture.pixelInset.height;
		width = oOxygen.guiTexture.pixelInset.width;
		x = oOxygen.guiTexture.pixelInset.x;
		y = oOxygen.guiTexture.pixelInset.y;
	}
	
	void Update () {
		
		timeCount -= Time.deltaTime;
		oScore.guiText.text = iScore.ToString("0");
		delta=Time.deltaTime*height*3.33F/100;
		currentx=oOxygen.transform.localPosition.x;
		currenty=oOxygen.transform.localPosition.y+0.000035f;
		currentz=oOxygen.transform.localPosition.z;
		oOxygen.transform.localPosition=new Vector3(currentx,currenty,currentz);
		
		oOxygen.guiTexture.pixelInset = new Rect(x,y,width, height*timeCount*3.33F/100);
	}
}
