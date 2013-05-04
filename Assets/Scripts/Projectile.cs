using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float projectileSpeed;
	private Transform myTransform;
	GameObject oCamera;
	
	// Use this for initialization
	void Start () {
		oCamera = GameObject.Find ("Main Camera");
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
	
	void OnTriggerEnter(Collider collider) {
				
		if (collider.gameObject.name == "Cube") {
			Debug.Log(collider.gameObject.name);
			
			//CreatureObject a = collider.gameObject.GetComponent(typeof(CreatureObject)) as CreatureObject;
			//Debug.Log(a.iStatus);
			Destroy(this.gameObject);
			//Destroy(other.gameObject);
		}
	}
}
