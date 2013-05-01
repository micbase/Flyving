using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {
	
	Grid screenGrid;
	GameObject oPlayer;

	void Start () {
		
		oPlayer = GameObject.Find ("Player");
		screenGrid = new Grid(100, 1, 1);
	}
	
	void Update () {
		screenGrid.updateGrid(oPlayer.transform.localPosition, 1);
	}
}

public class CreatureObject {
		
	GameObject obj;
	Rigidbody ridgeBody;
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
		obj.transform.localScale = new Vector3(1, 1, 1);
		
		//ridgeBody = obj.AddComponent("Rigidbody") as Rigidbody;
		//ridgeBody.useGravity = false;
		//ridgeBody.constraints = RigidbodyConstraints.FreezePositionY |
		//RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		
		iDirection = Random.Range(0, 2);
		iSpeed = Random.Range(0.01F, 0.20F);
		
		if (iDirection == 0)
			obj.transform.position = new Vector3(leftInitial, Random.Range (top, bottom), 0);
		else
			obj.transform.position = new Vector3(rightInitial, Random.Range (top, bottom), 0);	
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
	
	int generateType(float top, float bottom) {
		return 0;	
	}
}

public class Grid {
	
	int gridSize;
	float gridMargin;
	float gridHeight;
	GridCell[] gridArray;
	
	public Grid (int size, float margin, float height) {
		gridSize = size;
		gridMargin = margin;
		gridHeight = height;
		
		float currentHeight = 0;
		
		gridArray = new GridCell[gridSize];
		for (int i = 0; i < gridSize; i++) {
			gridArray[i] = new GridCell(currentHeight, currentHeight - gridHeight);
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
			
			if (iEnd > gridSize)
				iEnd = gridSize;
		}
		else {
			iStart = 0;
			iEnd = gridSize;
		}
				
		for (int i = iStart; i < iEnd; i++ ) {
			if (gridArray[i].getObj() != null)
				gridArray[i].getObj().Update();
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
			return true;
			return (Random.Range (0.0F, 1.0F) < 0.5);
		}

	}
}