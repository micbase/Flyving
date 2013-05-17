using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public enum ObjStatus { Normal = 1, Stop, Invisible };
public enum CreatureType { Dolphin = 1 };
public enum TreasureType { Gun = 1 };
public enum GameDirection { DivingDown = 1, DivingUp, FlyingUp, FlyingDown, GameOver };
public enum WeaponType { Gun = 1, Bomb, Spear};

public class Grid : MonoBehaviour {
	
	public float gameSpeed = -0.2f;
	float baseSpeed = -0.2f;

	int gridSize;
	float gridMargin;
	float gridHeight;
	float currentHeight;
	float currentBGHeight;
	float initialHeight = -15;
	float initialBGHeight = -26.6f;
	List<GridCell> gridArraySea;
	List<GridCell> gridArraySky;

	GameObject oPlayer;
	GameObject oCamera;
	
	GameObject oWater;
	
	Config[] objectDetails;
	GameDirection currentDirection;
		
	float colorspeed = 0.1f;
	float Acolor = 0.0f;
	Color mycolor = new Color(15.0f,17.0f,29.0f,0.0f);	
	
	void Start () {
		
		oPlayer = GameObject.Find("Player");
		oCamera = GameObject.Find("Main Camera");
		
		//oWater = GameObject.Find ("backPlane");
		
		objectDetails = new Config[2];
        objectDetails[0] = new Config("Assets/Resources/allfish.txt");
		
		gridSize = 20;
		gridMargin = 1;
		gridHeight = 1;
		
		currentHeight = initialHeight;
		currentBGHeight = initialBGHeight;
		currentDirection = GameDirection.DivingDown;
		
		gridArraySea = new List<GridCell>();
		gridArraySky = new List<GridCell>();
		GenerateGrid();
	}
	
	void Update () {
		
		float startPoint;
		float endPoint;
		int iStart = 0;
		int iEnd = 0;
		
		//Update camera and background
		oCamera.transform.Translate(0, gameSpeed, 0);	
		//oWater.transform.Translate(0, 0, gameSpeed / 3);
		
		/*
		if (Acolor<=200.0f)
		{	
			Acolor = Acolor+colorspeed;
			mycolor = new Color(15.0f/255,17.0f/255,29.0f/255,Acolor/255);
			oWater.renderer.material.color = mycolor;
		}
		*/

		//Update the creatures inside the grid.
		if (currentDirection == GameDirection.DivingDown ||
			currentDirection == GameDirection.DivingUp) {
				
			startPoint = oPlayer.transform.localPosition.y + (gridHeight + gridMargin) * 15;
			endPoint = oPlayer.transform.localPosition.y - (gridHeight + gridMargin) * 15;
			iStart = (int)Mathf.Abs(startPoint / (gridMargin + gridHeight));
			iEnd = (int)Mathf.Abs(endPoint / (gridMargin + gridHeight));
			
			if (startPoint >= 0)
				iStart = 0;
			
			if (iEnd > gridArraySea.Count) {
				GenerateGrid();
				iEnd = gridArraySea.Count;
			}
			
			for (int i = iStart; i < iEnd; i++ ) {
				gridArraySea[i].Update();
			}
		}	
		else if (currentDirection == GameDirection.FlyingUp ||
			currentDirection == GameDirection.FlyingDown) {
			
			startPoint = oPlayer.transform.localPosition.y - (gridHeight + gridMargin) * 15;
			endPoint = oPlayer.transform.localPosition.y + (gridHeight + gridMargin) * 15;
			iStart = (int)Mathf.Abs(startPoint / (gridMargin + gridHeight));
			iEnd = (int)Mathf.Abs(endPoint / (gridMargin + gridHeight));
			
			if (startPoint <= 0)
				iStart = 0;
			
			if (iEnd > gridArraySky.Count) {
				GenerateGrid();
				iEnd = gridArraySky.Count;
			}
			
			for (int i = iStart; i < iEnd; i++ ) {
				gridArraySky[i].Update();
			}
			
		}
	}
	
	public GameDirection CurrentDirection {
		get {
			return currentDirection;
		}
		
		set {
			currentDirection = value;
			
			if (value == GameDirection.FlyingUp) {
				currentHeight = Mathf.Abs(initialHeight);
				currentBGHeight = Mathf.Abs(initialBGHeight);
			}
			
			if (value == GameDirection.DivingUp || value == GameDirection.FlyingUp) {
				gameSpeed = Mathf.Abs(baseSpeed) * 3;
			}
			else if (value == GameDirection.FlyingDown || value == GameDirection.DivingDown)
				gameSpeed = (-1) * Mathf.Abs(baseSpeed);
			else
				gameSpeed = 0;
		}
	}
	
	void GenerateGrid() {
		
		if (currentDirection == GameDirection.DivingDown) {
			for (int i = 0; i < gridSize; i++) {
				gridArraySea.Add(new GridCell(currentHeight, currentHeight - gridHeight, objectDetails));
				currentHeight -= gridMargin + gridHeight;
			}
			
			while (currentBGHeight > currentHeight) {
				GenerateBackground();
			}
		}
		else if (currentDirection == GameDirection.FlyingUp) {
			for (int i = 0; i < gridSize; i++) {
				gridArraySky.Add(new GridCell(currentHeight, currentHeight + gridHeight, objectDetails));
				currentHeight += gridMargin + gridHeight;
			}
			
			while (currentBGHeight < currentHeight) {
				GenerateBackground();
			}
		}
	}
	
	void GenerateBackground() {
		
		GameObject obj;
		Material mat;
		
		obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.transform.localScale = new Vector3(48, 27, 0.001f);
		obj.transform.localPosition = new Vector3(0, currentBGHeight, 1);
		obj.transform.Rotate(0, 180, 0);
		
		if (currentDirection == GameDirection.DivingDown) {

			mat	= Resources.Load("Materials/sea", typeof(Material)) as Material;
			obj.renderer.material = mat;
			currentBGHeight -= Mathf.Abs(initialBGHeight);
		}
		else if (currentDirection == GameDirection.FlyingUp) {
			
			mat	= Resources.Load("Materials/sky", typeof(Material)) as Material;
			obj.renderer.material = mat;
			currentBGHeight += Mathf.Abs(initialBGHeight);
		}
	}
	
	public void applyWeapon(int objID, Vector3 pos, WeaponType weaponType) {
		
		if (currentDirection == GameDirection.DivingDown) {
			
			int index = Mathf.CeilToInt((initialHeight - pos.y) / (gridMargin + gridHeight)) - 1;

			if (weaponType == WeaponType.Gun) {
				gridArraySea[index].applyWeapon(objID, pos, weaponType);
			}
			else if (weaponType == WeaponType.Spear) {
				
				int iStart = Mathf.CeilToInt((initialHeight - oPlayer.transform.localPosition.y ) / (gridMargin + gridHeight)) - 1;
				
				for (int i = iStart; i <= index + 5; i++) {
					gridArraySea[i].applyWeapon(objID, pos, weaponType);
				}				
			}
			else if (weaponType == WeaponType.Bomb) {
				
				if (Vector3.Distance(oPlayer.transform.localPosition, pos) < 5) {
					Application.LoadLevel("GameOver");
					Debug.Log("killed by bomb");
				}
				
				for (int i = index - 5; i <= index + 5; i++) {
					if (i >= 0)
						gridArraySea[i].applyWeapon(objID, pos, weaponType);
				}
			}
		}
	}
		
	public void whenCollide(int objID, float top, int type) {
		
		if (currentDirection == GameDirection.DivingDown) {
			int index = Mathf.CeilToInt((initialHeight - top) / (gridMargin + gridHeight)) - 1;
			gridArraySea[index].whenCollide(objID, type);
		}
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
		
		public void applyWeapon(int objID, Vector3 pos, WeaponType weaponType) {
			
			switch (weaponType) {
				
			case WeaponType.Gun:
				if (hasCreature) {
					oCreature.attackedBy(weaponType);
				}
				break;
				
			case WeaponType.Spear:
				if (hasCreature) {
					if (Mathf.Abs(oCreature.getPosition().x - pos.x) < 1) {
						oCreature.attackedBy(weaponType);
					}
				}
				break;
				
			case WeaponType.Bomb:
				if (hasCreature) {
					if (Vector3.Distance(oCreature.getPosition(), pos) < 10) {
						oCreature.attackedBy(weaponType);
					}
				}
				break;
			}
		}
		
		public void whenCollide(int objID, int type) {
		
			oCreature.whenCollide();
		}
		
		bool isGenerate(float top, float bottom) {
			
			return (Random.Range (0.0F, 1.0F) < 0.6);
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
	
	float leftInitial = -17;
	float rightInitial = 17;
	float screenLeft = -18;
	float screenRight = 18;
	
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
		
		if (iStatus == ObjStatus.Invisible){
			obj.renderer.enabled = false;
			obj.collider.enabled = false;
		}
		else
			obj.renderer.enabled = true;
	}
	
	public int ObjID {
		
		get {
			return objID;
		}
	}
	
	public Vector3 getPosition() {
		return obj.transform.localPosition;
	}
	
	public void Update() {
		
		if (iStatus == ObjStatus.Normal) {
			if (iDirection == 0)
				obj.transform.Translate(fSpeed, 0, 0);
			else
				obj.transform.Translate((-1) * fSpeed, 0, 0);
			
			if (obj.transform.localPosition.x > screenRight) {
				changeDirection(1);
			}
			else if (obj.transform.localPosition.x < screenLeft) {
				changeDirection(0);
			}
		}
	}
	protected abstract int generateType(float top, float bottom, Config oDetails);
	protected abstract void changeDirection(int newDirection);
	public abstract void whenCollide();
}

public class Creature : Base {
	
	int iHealth;
	Config oCDetails;
	
	public Creature(float top, float bottom, Config details) : base(top, bottom) {
		
		obj.tag = "Creature";
		oCDetails = details;
		iStatus = ObjStatus.Normal;
		iType = generateType(top, bottom, oCDetails);
		iHealth = oCDetails.getHealth(iType);
		
		Material mat;
		if (iDirection == 1)
			mat	= Resources.Load("Materials/m" + oCDetails.getTypes(iType) + "Left", typeof(Material)) as Material;
		else
			mat	= Resources.Load("Materials/m" + oCDetails.getTypes(iType) + "Right", typeof(Material)) as Material;

		obj.renderer.material = mat;
		obj.transform.localScale = new Vector3(oCDetails.getSize(iType)[0], oCDetails.getSize(iType)[1], 0.001F);
		
		fSpeed = Random.Range(oCDetails.getSpeed(iType)[0], oCDetails.getSpeed(iType)[1]);
	}
	
	public void attackedBy(WeaponType weaponType) {
		
		Dashboard dashBoard = GameObject.Find("Main Camera").GetComponent("Dashboard") as Dashboard;
		dashBoard.iScore += oCDetails.getPoints(iType);
			
		if (weaponType == WeaponType.Gun) {
			iHealth -= 5;
		}
		else if (weaponType == WeaponType.Spear) {
			iHealth -= 10;
		}
		else if (weaponType == WeaponType.Bomb) {
			iHealth -= 100;
		}
		
		if (iHealth <= 0)
			base.setStatus(ObjStatus.Invisible);
			//base.setStatus(ObjStatus.Stop);
	}
	
	public override void whenCollide() {
		if (iStatus == ObjStatus.Normal) {
			//Application.LoadLevel("GameOver");
			Debug.Log("die");
		}
	}
	
	protected override void changeDirection(int newDirection) {
		iDirection = newDirection;
		
		Material mat;
		if (iDirection == 1)
			mat	= Resources.Load("Materials/m" + oCDetails.getTypes(iType) + "Left", typeof(Material)) as Material;
		else
			mat	= Resources.Load("Materials/m" + oCDetails.getTypes(iType) + "Right", typeof(Material)) as Material;
		
		obj.renderer.material = mat;
	}
		
	protected override int generateType(float top, float bottom, Config details) {
		int index = 0;
		
		if(top >= -125.0){
			index = Random.Range(0, details.getCount() - 4);
		}
		else{
			index = Random.Range(4,details.getCount() - 1);
		}
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
