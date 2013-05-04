using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float projectileSpeed;
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
		float amtToMove = projectileSpeed*Time.deltaTime;
		myTransform.Translate(0.0f,-amtToMove,0.0f);
		if (transform.position.y < oCamera.transform.position.y-8.20f) {
			Destroy(this.gameObject);	
		}
	}
	
	void OnTriggerEnter(Collider other) {
		
		Debug.Log (other.gameObject.name);
		
		if (other.gameObject != oPlayer) {
			//Destroy(other.gameObject);	
		}
	}
}
