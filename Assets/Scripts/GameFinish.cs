using UnityEngine;
using System.Collections;

public class GameFinish : MonoBehaviour {
	
	int iGenerate;
	float iGenerate2;
	bool Generate_finish=false;
	int itypeofplayer;
	float timer = 0.0f;
	float timer_player = 0.0f;
	float timer_developer = 0.0f;
	int count=0;
	GameObject oTransfer;
	// Use this for initialization
	void Start () {
		oTransfer = GameObject.Find ("Transfer");
        oTransfer.guiText.material.color = Color.black;
		oTransfer.guiText.fontSize = 1;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		timer_player += Time.deltaTime;
		timer_developer += Time.deltaTime;
		if (timer_player > 0.1f) {
			GenerateCube();
			count++;
			timer_player = 0.0f;
		}
		
		if (timer_developer > 2.0f) {
			
			if (!Generate_finish) {
				GenerateCubefordeveloper(1);
				Generate_finish = true;
			}
			else {
				GenerateCubefordeveloper(2);
				Generate_finish = false;
			}
			count++;
			timer_developer = 0.0f;
			
		}
		if (timer > 5.0f) {
			oTransfer.guiText.material.color = Color.white;
			oTransfer.guiText.fontSize=0;
			if (Input.GetKeyDown(KeyCode.Escape)) 
				Application.LoadLevel("MainMenu");
		}

	}
	void GenerateCubefordeveloper(int sequence) {
		GameObject obj2;
		obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj2.transform.localScale = new Vector3(6,6,6);
		iGenerate2 = Random.Range(-1.0f,1.0f);
        obj2.transform.localPosition = new Vector3(iGenerate2, 7, 4);
		obj2.AddComponent("Rigidbody");
		obj2.rigidbody.mass = 100;
		if (sequence == 1)
			obj2.renderer.material=Resources.Load("Materials/finish1",typeof(Material)) as Material;
		if (sequence == 2)
			obj2.renderer.material=Resources.Load("Materials/finish2",typeof(Material)) as Material;
	}
	void GenerateCube() {
        GameObject obj;
        
		iGenerate = Random.Range(-10,10);
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localPosition = new Vector3(iGenerate, 7, 4);
		obj.AddComponent("Rigidbody");

		itypeofplayer = Random.Range(1,16);
		switch(itypeofplayer) {
		case 1:
			obj.renderer.material=Resources.Load("Materials/player_forFinal/player_divingdown1",typeof(Material)) as Material;
			break;
		case 2:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_divingdown2",typeof(Material)) as Material;
			break;
		case 3:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_divingup",typeof(Material)) as Material;
			break;
		case 4:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_divingupleft",typeof(Material)) as Material;
			break;
		case 5:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_divingupright",typeof(Material)) as Material;
			break;
		case 6:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flydownback",typeof(Material)) as Material;
			break;
		case 7:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flydownfront",typeof(Material)) as Material;
			break;
		case 8:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flydownleft",typeof(Material)) as Material;
			break;
		case 9:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flydownright",typeof(Material)) as Material;
			break;
		case 10:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flydownmidleft",typeof(Material)) as Material;
			break;
		case 11:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flydownmidright",typeof(Material)) as Material;
			break;
		case 12:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flyupback",typeof(Material)) as Material;
			break;
		case 13:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flyupfront",typeof(Material)) as Material;
			break;
		case 14:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flyupleft",typeof(Material)) as Material;
			break;
		case 15:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_flyupright",typeof(Material)) as Material;
			break;
		case 16:
			obj.renderer.material=Resources.Load ("Materials/player_forFinal/player_standing",typeof(Material)) as Material;
			break;
		}
					

       
    }
}
