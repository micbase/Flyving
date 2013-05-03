using UnityEngine;
using System.Collections;

public class Dashboard : MonoBehaviour {

	GameObject oScore;
	GameObject oOxygen;
	float timeCount;
	
	void Start () {
		
		oScore = GameObject.Find("Score");
		oOxygen = GameObject.Find("Oxygen");
		
		oScore.guiText.text = "30";
		timeCount = 30;
	}
	
	// Update is called once per frame
	void Update () {
		
		timeCount -= Time.deltaTime;
		oScore.guiText.text = timeCount.ToString("0");
		oOxygen.guiTexture.pixelInset = new Rect(0, 0, timeCount*3.33F, 25);
		
		if (timeCount <= 0)
			Application.LoadLevel("GameOver");
	}
}
