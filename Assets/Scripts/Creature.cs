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
		
		//direction 1: diving down
		//direction 2: diving up
		//direction 3: flying up
		//direction 4: flying down
		screenGrid.updateGrid(oPlayer.transform.localPosition, 1);
	}

}

public class CreatureObject {
		
	GameObject obj;
	int iType = 1;
	int iStatus = 1;
	float iSpeed = 1;
	int iDirection = 0;
	float leftInitial = -17;
	float rightInitial = 17;
	float screenLeft = -16;
	float screenRight = 16;
	
	public CreatureObject(float top, float bottom) {
		
		iType = generateType(top, bottom);
		iStatus = 1;
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//obj.AddComponent<CreatureObject>
		
		if (iType == 1) {
			Material mDolphin = Resources.LoadAssetAtPath("Assets/Materials/mDolphin.mat", typeof(Material)) as Material;
			obj.renderer.material = mDolphin;
			obj.transform.localScale = new Vector3(3, 1.5F, 1);
			obj.transform.Rotate(0, 180, 0);
		}
		else {
			
		}
		
		iDirection = Random.Range(0, 2);
		iSpeed = Random.Range(0.01F, 0.20F);
		
		if (iDirection == 0)
			obj.transform.position = new Vector3(Random.Range(leftInitial, rightInitial), Random.Range (top, bottom), 0);
		else
			obj.transform.position = new Vector3(Random.Range(leftInitial, rightInitial), Random.Range (top, bottom), 0);	
	}
	
	public void setStatus(int status) {
		
		iStatus = status;
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
		
		currentHeight = -10;
		
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
			gridArray[i].Update();
		}
	}
	
	public class GridCell {
		
		float topPosition;
		float bottomPosition;
		bool hasCreature = false;
		CreatureObject oCreature = null;
		
		public GridCell(float top, float bottom) {
			topPosition = top;
			bottomPosition = bottom;
			
			hasCreature = isGenerate(topPosition, bottomPosition);
			if (hasCreature)
				oCreature = new CreatureObject(topPosition, bottomPosition);
		}
		
		public void Update() {
			
			if (hasCreature)
				oCreature.Update();
		}
		
		bool isGenerate(float top, float bottom) {
			return (Random.Range (0.0F, 1.0F) < 0.3);
		}

	}
}