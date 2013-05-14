using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour {
	
	public float SpearSpeed;
	private Transform myTransform;
	GameObject oCamera;
	GameObject oPlayer;
	
	// Use this for initialization
	void Start () {
		oCamera = GameObject.Find ("Main Camera");
		oPlayer = GameObject.Find ("Player");
		myTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		float amtToMove = SpearSpeed*Time.deltaTime;
		myTransform.Translate(0.0f,-amtToMove,0.0f);
		if (transform.position.y < oCamera.transform.position.y-8.70f) {
			Destroy(this.gameObject);	
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject != oPlayer) {
			//Instantiate(blood, other.gameObject.transform.localPosition, Quaternion.identity);
			Debug.Log ("Spear kill");
		}
	}
}

