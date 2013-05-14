using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour {
	
	public float SpearSpeed;
	private Transform myTransform;
	GameObject oCamera;
	
	// Use this for initialization
	void Start () {
		oCamera = GameObject.Find ("Main Camera");
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
	
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Creature") {
			//Instantiate(blood, other.gameObject.transform.localPosition, Quaternion.identity);
			Destroy(this.gameObject);
			Grid screenGrid =  oCamera.GetComponent("Grid") as Grid;
			screenGrid.applyWeapon(int.Parse(collider.gameObject.name), collider.gameObject.transform.localPosition.y, WeaponType.Spear);
		}
	}
}

