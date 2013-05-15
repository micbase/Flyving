using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	GameObject oPlayer;
	GameObject oCamera;
	GameObject oWater;
	GameObject oBubble;
	GameObject oBackWater1;
	GameObject oBackWater2;

	public GameObject projectilePrefab;
	public GameObject Bomb;
	public GameObject Spear;
	
	public int iHealth;
	public float playerspeed = -0.2f;
	
	float colorspeed = 0.1f;
	float Acolor = 0.0f;
	Color mycolor = new Color(15.0f,17.0f,29.0f,0.0f);
	int n=2;


	void Start () {
		
		iHealth = 100;
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		oBubble = GameObject.Find("Bubbles");
		oWater = GameObject.Find ("backPlane");
		oBackWater1 = GameObject.Find ("Water2");
		oBackWater2 = GameObject.Find ("Water3");
	}
	
	void Update () {
				
		if (Input.GetKey(KeyCode.LeftArrow) && oPlayer.transform.localPosition.x > -15) {
			oPlayer.transform.Translate(-0.5F, 0, 0);
			oBubble.transform.Translate(-0.5F, 0, 0);
		}
		
		else if (Input.GetKey(KeyCode.RightArrow) && oPlayer.transform.localPosition.x < 15) {
			oPlayer.transform.Translate(0.5F, 0, 0);
			oBubble.transform.Translate(0.5F, 0, 0);
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
			playerspeed=playerspeed*(-1);
			oPlayer.transform.Rotate(0,180.0f,0);
		}
			
	
		
		oCamera.transform.Translate(0, playerspeed, 0);
		oPlayer.transform.Translate(0, playerspeed, 0);
		oBubble.transform.Translate(0, playerspeed, 0);
		oWater.transform.Translate(0,0, -playerspeed);
		if(oWater.transform.localPosition.y>=-100)
		{
			oBackWater1.transform.localPosition=new Vector3(0,-50,2);
			oBackWater2.transform.localPosition=new Vector3(0,-100,2);
		}
		if(playerspeed<0)
		{
			if(oWater.transform.localPosition.y<-50*n)
			{
				if(n%2==1)
				{
					oBackWater2.transform.localPosition=new Vector3(0,-50*(n+1),2);
					n++;
				}
				else
				{
					oBackWater1.transform.localPosition=new Vector3(0,-50*(n+1),2);
					n++;
				}
				
			}
		}
		else
		{
			if(oWater.transform.localPosition.y>-50*(n-1))
			{
				if(n%2==1)
				{
					oBackWater1.transform.localPosition=new Vector3(0,-50*(n-2),2);
					n--;
				}
				else
				{
					oBackWater2.transform.localPosition=new Vector3(0,-50*(n-2),2);
					n--;
				}
				
			}
		}


		
		if (Acolor<=200.0f)
		{	
			Acolor = Acolor+colorspeed;
			mycolor = new Color(15.0f/255,17.0f/255,29.0f/255,Acolor/255);
			oWater.renderer.material.color = mycolor;
		}

		
		if (Input.GetKeyDown(KeyCode.Space)) {
			Instantiate(projectilePrefab, oPlayer.transform.localPosition, Quaternion.identity);
		}
		
		if (Input.GetKeyDown(KeyCode.B)) {
			Instantiate(Bomb, oPlayer.transform.localPosition, Quaternion.identity);
		}
		
		if (Input.GetKeyDown(KeyCode.V)) {
			Instantiate(Spear, oPlayer.transform.localPosition, Quaternion.identity);
		}
		
		GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon") as GameObject[];
		foreach (GameObject w in weapons) {
			w.transform.Translate(0, -0.2F, 0);
		}
		
	}

	void OnTriggerEnter(Collider collider) {
		
		if (collider.gameObject.tag == "Creature") {
			
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			screenGrid.whenCollide(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, 1);
		}
	}	
	
}
