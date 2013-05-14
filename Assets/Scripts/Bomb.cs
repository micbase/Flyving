using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public float BombSpeed;
	private Transform myTransform;
	public GameObject Explosion1;
	public GameObject Explosion2;
	public GameObject Explosion3;
	GameObject oCamera;
	GameObject oPlayer;
	GameObject oWater;
	
	void Start () {
		oCamera = GameObject.Find ("Main Camera");
		oPlayer = GameObject.Find ("Player");
		oWater = GameObject.Find ("backPlane");
		myTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		float amtToMove = BombSpeed*Time.deltaTime;
		myTransform.Translate(0.0f,-amtToMove,0.0f);
		if (transform.position.y < oCamera.transform.position.y-8.20f) {
			Destroy(this.gameObject);	
		}
	}
		
	void OnTriggerEnter(Collider other) {

        Debug.Log(other);
		
		Vector3 exp_pos_1 = new Vector3(this.gameObject.transform.localPosition.x,this.gameObject.transform.localPosition.y,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_2 = new Vector3(this.gameObject.transform.localPosition.x+2.0f,this.gameObject.transform.localPosition.y+2.0f,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_3 = new Vector3(this.gameObject.transform.localPosition.x-2.0f,this.gameObject.transform.localPosition.y+2.0f,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_4 = new Vector3(this.gameObject.transform.localPosition.x+2.0f,this.gameObject.transform.localPosition.y-2.0f,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_5 = new Vector3(this.gameObject.transform.localPosition.x-2.0f,this.gameObject.transform.localPosition.y-2.0f,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_6 = new Vector3(this.gameObject.transform.localPosition.x,this.gameObject.transform.localPosition.y-2.5f,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_7 = new Vector3(this.gameObject.transform.localPosition.x,this.gameObject.transform.localPosition.y+2.5f,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_8 = new Vector3(this.gameObject.transform.localPosition.x-2.5f,this.gameObject.transform.localPosition.y,this.gameObject.transform.localPosition.z);
		Vector3 exp_pos_9 = new Vector3(this.gameObject.transform.localPosition.x+2.5f,this.gameObject.transform.localPosition.y,this.gameObject.transform.localPosition.z);
		
		if (other.gameObject != oPlayer && other.gameObject != oWater) {
			Destroy(this.gameObject);
			Instantiate(Explosion1, exp_pos_1, Quaternion.identity);
			Instantiate(Explosion2, exp_pos_1, Quaternion.identity);
			Instantiate(Explosion2, exp_pos_1, Quaternion.identity);
			Instantiate(Explosion2, exp_pos_2, Quaternion.identity);
			Instantiate(Explosion3, exp_pos_3, Quaternion.identity);
			Instantiate(Explosion1, exp_pos_4, Quaternion.identity);
			Instantiate(Explosion2, exp_pos_5, Quaternion.identity);
			Instantiate(Explosion2, exp_pos_6, Quaternion.identity);
			Instantiate(Explosion3, exp_pos_7, Quaternion.identity);
			Instantiate(Explosion1, exp_pos_8, Quaternion.identity);
			Instantiate(Explosion3, exp_pos_9, Quaternion.identity);
		}
	}
}
