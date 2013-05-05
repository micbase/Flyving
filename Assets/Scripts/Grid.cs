using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	
	GridObject screenGrid;
	GameObject oPlayer;
	
	void Start () {
		
		oPlayer = GameObject.Find("Player");
		screenGrid = new GridObject(20, 1, 1);
	}
	
	void Update () {
		
		//direction 1: diving down
		//direction 2: diving up
		//direction 3: flying up
		//direction 4: flying down
		screenGrid.updateGrid(oPlayer.transform.localPosition, 1);
	}
	
	public void updateStatus(int objID, float top, int newStatus) {
		
		screenGrid.updateStatus(objID, top, newStatus);
	}
	
	public void whenCollide(int objID, float top, int type) {
	
		screenGrid.whenCollide(objID, top, type);
	}

}

public class CreatureObject {
			
	int objID;
	GameObject obj;
	int iType = 1;
	int iStatus = 1;
	float fSpeed = 1;
	int iDirection = 0;
	float leftInitial = -13;
	float rightInitial = 13;
	float screenLeft = -14;
	float screenRight = 14;
	
	public CreatureObject(float top, float bottom) {
		
		iType = generateType(top, bottom);
		iStatus = 1;
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		objID = this.GetHashCode();
		obj.tag = "Creature";
		obj.name = objID.ToString();
		
		if (iType == 1) {
			Material mDolphin = Resources.LoadAssetAtPath("Assets/Materials/mDolphin.mat", typeof(Material)) as Material;
			obj.renderer.material = mDolphin;
			obj.transform.localScale = new Vector3(3, 1.5F, 1);
			obj.transform.Rotate(0, 180, 0);
		}
		else {
			
		}

		iDirection = Random.Range(0, 2);
		fSpeed = Random.Range(0.01F, 0.20F);
		obj.transform.position = new Vector3(Random.Range(leftInitial, rightInitial), Random.Range (top, bottom), 0);	
	}
	
	public void setStatus(int status) {
		
		iStatus = status;
		obj.renderer.enabled = false;
	}
	
	public int getStatus() {
		
		return iStatus;
	}
	
	public int getObjID() {
		
		return objID;
	}
	
	public void Update() {
		
		if (iStatus == 1) {
			if (iDirection == 1)
				obj.transform.Translate(fSpeed, 0, 0);
			else
				obj.transform.Translate((-1) * fSpeed, 0, 0);
			
			if (obj.transform.localPosition.x > screenRight) {
				iDirection = 1;
			}
			else if (obj.transform.localPosition.x < screenLeft) {
				iDirection = 0;
			}
		}
	}
	
	int generateType(float top, float bottom) {
		return 1;
	}
}

public class GridObject {
	
	int gridSize;
	float gridMargin;
	float gridHeight;
	float currentHeight;
	float initialHeight = -10;
	List<GridCell> gridArray;
	
	public GridObject (int size, float margin, float height) {
		
		gridSize = size;
		gridMargin = margin;
		gridHeight = height;
		
		currentHeight = initialHeight;
		
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
	
	public void updateStatus(int objID, float top, int newStatus) {
		
		int index = Mathf.CeilToInt((initialHeight - top) / (gridMargin + gridHeight)) - 1;
		gridArray[index].updateStatus(objID, newStatus);
	}
		
	public void whenCollide(int objID, float top, int type) {
		
		int index = Mathf.CeilToInt((initialHeight - top) / (gridMargin + gridHeight)) - 1;
		gridArray[index].whenCollide(objID, type);
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
		
		public void updateStatus(int objID, int newStatus) {
		
			if (hasCreature && oCreature.getObjID() == objID)
				oCreature.setStatus(newStatus);
		}
		
		public void whenCollide(int objID, int type) {
		
			if (type == 1) {
				
				if (oCreature.getStatus() == 1) {
					Application.LoadLevel("GameOver");
					Debug.Log("die");
				}
			}
		}
		
		bool isGenerate(float top, float bottom) {
			
			return (Random.Range (0.0F, 1.0F) < 0.7);
		}

	}
}