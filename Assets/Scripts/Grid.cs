using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObjStatus { Normal = 1, Stop, Invisiable };
public enum CreatureType { Dolphin = 1 };
public enum TreasureType { Gun = 1 };
public enum GameDirection { DivingDown = 1, DivingUp, FlyingUp, FlyingDown };
public enum WeaponType { Gun = 1, Bomb };

public class Grid : MonoBehaviour {
	
	int gridSize;
	float gridMargin;
	float gridHeight;
	float currentHeight;
	float initialHeight = -10;
	
	List<GridCell> gridArray;
	GameObject oPlayer;
	GameDirection currentDirection;
	
	void Start () {
		
		oPlayer = GameObject.Find("Player");
		
		gridSize = 20;
		gridMargin = 1;
		gridHeight = 1;
		
		currentHeight = initialHeight;
		currentDirection = GameDirection.DivingDown;
		
		gridArray = new List<GridCell>();
		GenerateGrid();
	}
	
	void Update () {
		
		float startPoint;
		float endPoint;
		int iStart;
		int iEnd;

		if (currentDirection == GameDirection.DivingDown) {
				
			startPoint = oPlayer.transform.localPosition.y + 15;
			endPoint = oPlayer.transform.localPosition.y - 25;
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

	
	void GenerateGrid() {
		
		for (int i = 0; i < gridSize; i++) {
			gridArray.Add(new GridCell(currentHeight, currentHeight - gridHeight));
			currentHeight -= gridMargin + gridHeight;
		}
	}
	
	public void setDirection(GameDirection direction) {
		
		currentDirection = direction;
	}
	
	public void applyWeapon(int objID, float top, WeaponType weaponType) {
		
		int index = Mathf.CeilToInt((initialHeight - top) / (gridMargin + gridHeight)) - 1;
		gridArray[index].applyWeapon(objID, weaponType);
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
		
		public void applyWeapon(int objID, WeaponType weaponType) {
		
			if (hasCreature && oCreature.ObjID == objID) {
				
				if (weaponType == WeaponType.Gun)
					oCreature.setStatus(ObjStatus.Invisiable);
			}
		}
		
		public void whenCollide(int objID, int type) {
		
			oCreature.whenCollide();
		}
		
		bool isGenerate(float top, float bottom) {
			
			return (Random.Range (0.0F, 1.0F) < 0.7);
		}

	}
}


public abstract class BaseObject {
			
	protected int objID;
	protected GameObject obj;
	protected ObjStatus iStatus;
	protected float fSpeed;
	protected int iDirection;
	
	float leftInitial = -13;
	float rightInitial = 13;
	float screenLeft = -14;
	float screenRight = 14;
	
	public BaseObject(float top, float bottom) {
		
		iDirection = Random.Range(0, 2);
		
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		objID = this.GetHashCode();
		obj.name = objID.ToString();
		obj.transform.position = new Vector3(Random.Range(leftInitial, rightInitial), Random.Range (top, bottom), 0);	
	}
	
	public void setStatus(ObjStatus status) {
		
		iStatus = status;
		
		if (iStatus == ObjStatus.Invisiable)
			obj.renderer.enabled = false;
		else
			obj.renderer.enabled = true;
	}
	
	public int ObjID {
		
		get {
			return objID;
		}
	}
	
	public void Update() {
		
		if (iStatus == ObjStatus.Normal) {
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
	
	public abstract void whenCollide();
}

public class CreatureObject : BaseObject {
	
	CreatureType iType;
	
	public CreatureObject(float top, float bottom) : base(top, bottom) {
	
		obj.tag = "Creature";
		iStatus = ObjStatus.Normal;
		iType = generateType(top, bottom);
		
		if (iType == CreatureType.Dolphin) {
			Material mDolphin = Resources.LoadAssetAtPath("Assets/Materials/mDolphin.mat", typeof(Material)) as Material;
			obj.renderer.material = mDolphin;
			obj.transform.localScale = new Vector3(3, 1.5F, 1);
			obj.transform.Rotate(0, 180, 0);
			fSpeed = Random.Range(0.01F, 0.20F);
		}
		else {
			
		}

	}
	
	public override void whenCollide() {
		if (iStatus == ObjStatus.Normal) {
			Application.LoadLevel("GameOver");
			Debug.Log("die");
		}
	}
		
	CreatureType generateType(float top, float bottom) {
		return CreatureType.Dolphin;
	}
}
