using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float projectileSpeed;
	Transform myTransform;
	GameObject oCamera;
	
	// Use this for initialization
	void Start () {
		oCamera = GameObject.Find ("Main Camera");
		myTransform = gameObject.transform;
	}
	
	// Update is called oncex per frame
	void Update () {
		float amtToMove = projectileSpeed*Time.deltaTime;
		myTransform.Translate(0.0f,-amtToMove,0.0f);
		if (transform.position.y < oCamera.transform.position.y-8.20f) {
			Destroy(this.gameObject);	
		}
	}
	
	void OnTriggerEnter(Collider collider) {
				
		if (collider.gameObject.tag == "Creature") {
			
			Grid screenGrid = oCamera.GetComponent("Grid") as Grid;
			screenGrid.applyWeapon(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition, WeaponType.Gun);
			Destroy(this.gameObject);
		}
	}
}
