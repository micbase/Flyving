using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour {
	
	Grid screenGrid;
	GameObject oPlayer;
	
	void Start () {
		
		oPlayer = GameObject.Find ("Player");
		screenGrid = new Grid(20, 1, 1);
	}
	
	void Update () {
		screenGrid.updateGrid(oPlayer.transform.localPosition, 1);
	}
	
	void OnCollisionEnter(Collision collision) {
		Debug.Log("Collided with " + collision.gameObject.name);
	}
}

public class CreatureObject {
		
	GameObject obj;
	Rigidbody ridgeBody;
	BoxCollider boxCollider;
	int iType = 0;
	int iStatus = 0;
	float iSpeed = 1;
	int iDirection = 0;
	float leftInitial = -17;
	float rightInitial = 17;
	float screenLeft = -16;
	float screenRight = 16;
	
	public CreatureObject(float top, float bottom) {
		
		iType = generateType(top, bottom);
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
		if (iType == 1) {
			Material mDolphin = Resources.LoadAssetAtPath("Assets/mDolphin.mat", typeof(Material)) as Material;
			obj.renderer.material = mDolphin;
			obj.transform.localScale = new Vector3(3, 1.5F, 1);
		}
		else {
			
		}
		
		//boxCollider = obj.AddComponent("BoxCollider") as BoxCollider;
		boxCollider = obj.GetComponent("BoxCollider") as BoxCollider;
		boxCollider.isTrigger = true;
		//ridgeBody = obj.AddComponent("Rigidbody") as Rigidbody;
		//ridgeBody.useGravity = false;
		//ridgeBody.constraints = RigidbodyConstraints.FreezePositionY |
		//RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		
		iDirection = Random.Range(0, 2);
		iSpeed = Random.Range(0.01F, 0.20F);
		
		if (iDirection == 0)
			obj.transform.position = new Vector3(Random.Range(leftInitial, rightInitial), Random.Range (top, bottom), 0);
		else
			obj.transform.position = new Vector3(Random.Range(leftInitial, rightInitial), Random.Range (top, bottom), 0);	
	}
	
	public void Update() {
		
		if (iDirection == 0)
			obj.transform.Translate(iSpeed, 0, 0);
		else
			obj.transform.Translate((-1) * iSpeed, 0, 0);
		
		if (obj.transform.localPosition.x > screenRight) {
			iDirection = 1;
		}
		else if (obj.transform.localPosition.x < screenLeft) {
			iDirection = 0;
		}
	}
	
	public GameObject getObj() {
		return obj;
	}
	
	int generateType(float top, float bottom) {
		return 1;
	}
}

public class Grid {
	
	int gridSize;
	float gridMargin;
	float gridHeight;
	float currentHeight;
	List<GridCell> gridArray;
	
	public Grid (int size, float margin, float height) {
		gridSize = size;
		gridMargin = margin;
		gridHeight = height;
		
		currentHeight = 0;
		
		gridArray = new List<GridCell>();
		GenerateGrid();
	}
	
	void GenerateGrid() {
		
		for (int i = 0; i < gridSize; i++) {
			gridArray.Add(new GridCell(currentHeight, currentHeight - gridHeight));
			currentHeight -= gridMargin + gridHeight;
		}
	}
	
	public void updateGrid(Vector3 playerPos, int direction) {
		
		float startPoint;
		float endPoint;
		int iStart;
		int iEnd;
		
		if (direction == 1) {
				
			startPoint = playerPos.y + 15;
			endPoint = playerPos.y - 25;
			iStart = (int)Mathf.Abs(startPoint / (gridMargin + gridHeight));
			iEnd = (int)Mathf.Abs(endPoint / (gridMargin + gridHeight));
			
			if (startPoint >= 0)
				iStart = 0;
			
			if (iEnd > gridArray.Count) {
				GenerateGrid();
				iEnd = gridArray.Count;
			}
		}
		else {
			//fake code!!
			iStart = 0;
			iEnd = gridSize;
		}			
				
		for (int i = iStart; i < iEnd; i++ ) {
			if (gridArray[i].getObj() != null) {
				
				if (Mathf.Abs(gridArray[i].getObj().getObj().transform.localPosition.x - playerPos.x) < 1 &&
					Mathf.Abs(gridArray[i].getObj().getObj().transform.localPosition.y - playerPos.y) < 1)
					//Application.LoadLevel("GameOver");
					Debug.Log("die");
				
				gridArray[i].getObj().Update();
			}
		}
	}
	
	public class GridCell {
		
		float topPosition;
		float bottomPosition;
		bool hasCreature = false;
		CreatureObject objCell = null;
		
		public GridCell(float top, float bottom) {
			topPosition = top;
			bottomPosition = bottom;
			
			hasCreature = isGenerate(topPosition, bottomPosition);
			if (hasCreature)
				objCell = new CreatureObject(topPosition, bottomPosition);
		}
		
		public CreatureObject getObj() {
			if (hasCreature)
				return objCell;
			else
				return null;
		}
		
		bool isGenerate(float top, float bottom) {
			return (Random.Range (0.0F, 1.0F) < 0.8);
		}

	}
}