using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	GameObject oPlayer;
	GameObject oCamera;
	GameObject[] oFish;
	Rigidbody[] ridgeBody;
	int[] direction;
	float[] speed;
	
	// Use this for initialization
	void Start () {
		oPlayer = GameObject.Find ("Player");
		oCamera = GameObject.Find("Main Camera");
		oFish = new GameObject[30];
		direction = new int[30];
		speed = new float[30];
		
		for (int i = 0; i< oFish.Length; i++) {
			oFish[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);	
			oFish[i].transform.localScale = new Vector3(2,2,2);
			
			//ridgeBody[i] = oFish[i].AddComponent<Rigidbody>();
			//ridgeBody[i].useGravity = false;
			//ridgeBody[i].constraints = RigidbodyConstraints.FreezePositionY && RigidbodyConstraints.FreezePositionZ;
			
			direction[i] = Random.Range(0,2);
			speed[i] = Random.Range(0.01F, 0.20F);
			if (direction[i] == 0)
				oFish[i].transform.position = new Vector3(-17,Random.Range (-100F,-3F),0);
			else
				oFish[i].transform.position = new Vector3(17,Random.Range (-100F,-3F),0);
		}
				
	}
	
	// Update is called once per frame
	void Update () {
				
		if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -15)
			oPlayer.transform.Translate(-0.5F,0,0);
		
		else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 15)
			oPlayer.transform.Translate(0.5F,0,0);
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			oPlayer.transform.Translate(0,0.3F,0);
			oCamera.transform.Translate(0,0.3F,0);
			
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			oPlayer.transform.Translate(0, -0.3F,0);
			oCamera.transform.Translate(0, -0.3F,0);
		}
		
		//oCamera.transform.Translate(0, -0.2F, 0);
		//oPlayer.transform.Translate(0, -0.2F, 0);
		
		for (int i = 0; i < oFish.Length; i++) {
			if (direction[i] == 0)
				oFish[i].transform.Translate(speed[i],0,0);
			else
				oFish[i].transform.Translate((-1)*speed[i],0,0);
			
			if (oFish[i].transform.localPosition.x > 16) {
				direction[i] = 1;
				//NewFish();
			}
			else if (oFish[i].transform.localPosition.x < -16) {
				direction[i] = 0;
				//NewFish();
			}
		}
	}
	
	void NewFish() {
		
	}
}