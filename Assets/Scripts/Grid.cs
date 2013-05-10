using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

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
	Config[] objectDetails;
	
	void Start () {
		
		oPlayer = GameObject.Find("Player");
		objectDetails = new Config[2];
        objectDetails[0] = new Config("Assets/Resources/allfish.txt");
		
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
				
			startPoint = oPlayer.transform.localPosition.y + (gridHeight + gridMargin) * 12;
			endPoint = oPlayer.transform.localPosition.y - (gridHeight + gridMargin) * 12;
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
			gridArray.Add(new GridCell(currentHeight, currentHeight - gridHeight, objectDetails));
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
		Creature oCreature = null;
		
		public GridCell(float top, float bottom, Config[] objectDetail) {
			topPosition = top;
			bottomPosition = bottom;
			
			hasCreature = isGenerate(topPosition, bottomPosition);
			if (hasCreature)
				oCreature = new Creature(topPosition, bottomPosition, objectDetail[0]);
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


public abstract class Base {
			
	protected int objID;
	protected GameObject obj;
	protected ObjStatus iStatus;
	protected float fSpeed;
	protected int iType;
	protected int iDirection;
	
	float leftInitial = -13;
	float rightInitial = 13;
	float screenLeft = -14;
	float screenRight = 14;
	
	public Base(float top, float bottom) {
		//iType = generateType(top, bottom);
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
	protected abstract int generateType(float top, float bottom, Config oDetails);
	public abstract void whenCollide();
}

public class Creature : Base {
	
	int iHealth;
	
	public Creature(float top, float bottom, Config oCDetails) : base(top, bottom) {
		
		obj.tag = "Creature";
		iStatus = ObjStatus.Normal;
		iType = generateType(top, bottom, oCDetails);
		iHealth = oCDetails.getHealth(iType);
		
		Material mat = Resources.Load("Materials/m" + oCDetails.getTypes(iType), typeof(Material)) as Material;
		obj.renderer.material = mat;
		obj.transform.localScale = new Vector3(3, 1.5F, 1);
		obj.transform.Rotate(0, 180, 0);
		
		fSpeed = Random.Range(oCDetails.getSpeed(iType)[0], oCDetails.getSpeed(iType)[1]);
	}
	
	public override void whenCollide() {
		if (iStatus == ObjStatus.Normal) {
			//Application.LoadLevel("GameOver");
			Debug.Log("die");
		}
	}
		
	protected override int generateType(float top, float bottom, Config oDetails) {
		int index = Random.Range(0,oDetails.getCount() - 1);
		return index;
	}
	
}


public class Config{
	//type,size(width,height),speed(low,high),health,points
	string[] type;
	float[,] size;
	float[,] speed;
	int[] health;
	int[] points;
    int count;
	
	public  Config(string filepath){
		int lineCount = 0;
		using (var reader = File.OpenText(filepath))
		{
		    while (reader.ReadLine() != null)
		    {
		        lineCount++;
		    }
		}
		
		type = new string[lineCount];
		size = new float[2,lineCount];
		speed = new float[2,lineCount];
		health = new int[lineCount];
		points = new int[lineCount];
        count = lineCount;
		
		TextReader tr = new StreamReader(filepath);
		
		for(int i=0; i<lineCount;i++){
			string[] s = tr.ReadLine().Split(',');
			type[i] = s[0];
			size[0,i] = float.Parse(s[1],CultureInfo.InvariantCulture.NumberFormat);
			size[1,i] = float.Parse(s[2],CultureInfo.InvariantCulture.NumberFormat);
			speed[0,i] = float.Parse(s[3],CultureInfo.InvariantCulture.NumberFormat);
			speed[1,i] = float.Parse(s[4],CultureInfo.InvariantCulture.NumberFormat);
			health[i] = int.Parse(s[5],CultureInfo.InvariantCulture.NumberFormat);
			points[i] = int.Parse(s[6], CultureInfo.InvariantCulture.NumberFormat);
		}
	}

    public int getCount() {
        return count;
    }
	
	public string getTypes(int iType){
		return type[iType];
	}
	
	public float[] getSize(int iType){
		float[] retSize = new float[2];
		retSize[0] = size[0,iType];
		retSize[1] = size[1,iType];
		return retSize;
	}
	
	public float[] getSpeed(int iType){
		float[] retSpeed = new float[2];
		retSpeed[0] = speed[0,iType];
		retSpeed[1] = speed[1,iType];
		return retSpeed;
	}
	
	public int getHealth(int iType){
		return health[iType];
	}
	
	public int getPoints(int iType){
		return points[iType];
	}
}
