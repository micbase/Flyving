using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {
	
	CreatureObject[] cObj = new CreatureObject[30];

	void Start () {

		for (int i = 0; i < cObj.Length; i++) {
			cObj[i] = new CreatureObject();
		}
	}
	
	void Update () {
			
		for (int i = 0; i < cObj.Length; i++) {
			cObj[i].Update();
		}
	}
}

public class CreatureObject {
		
	GameObject obj;
	Rigidbody ridgeBody;
	int iType = 1;
	float iSpeed = 1;
	int iDirection = 0;
	
	public CreatureObject() {
		
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);	
		obj.transform.localScale = new Vector3(2,2,2);
		
		ridgeBody = obj.AddComponent("Rigidbody") as Rigidbody;
		ridgeBody.useGravity = false;
		ridgeBody.constraints = RigidbodyConstraints.FreezePositionY |
		RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		
		iDirection = Random.Range(0,2);
		iSpeed = Random.Range(0.01F, 0.20F);
		
		if (iDirection == 0)
			obj.transform.position = new Vector3(-17,Random.Range (-100F,-3F),0);
		else
			obj.transform.position = new Vector3(17,Random.Range (-100F,-3F),0);	
	}
	
	public void Update() {
		
		if (iDirection == 0)
			obj.transform.Translate(iSpeed,0,0);
		else
			obj.transform.Translate((-1)*iSpeed,0,0);
		
		if (obj.transform.localPosition.x > 16) {
			iDirection = 1;
		}
		else if (obj.transform.localPosition.x < -16) {
			iDirection = 0;
		}
	}
}