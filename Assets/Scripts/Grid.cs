using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class Grid : MonoBehaviour {
	
	int gridSize;
	float gridMargin;
	float gridHeight;
	float currentHeight;
	float initialHeight = -10;
	
	List<GridCell> gridArray;
	GameObject oPlayer;
	int currentDirection;
	
	void Start () {
		
		oPlayer = GameObject.Find("Player");
		
		gridSize = 20;
		gridMargin = 1;
		gridHeight = 1;
		
		currentHeight = initialHeight;
		currentDirection = 1;
		
		gridArray = new List<GridCell>();
		GenerateGrid();
	}
	
	void Update () {
		
		float startPoint;
		float endPoint;
		int iStart;
		int iEnd;
				
		//direction 1: diving down
		//direction 2: diving up
		//direction 3: flying up
		//direction 4: flying down

		if (currentDirection == 1) {
				
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
	
	public void setDirection(int direction) {
		
		currentDirection = direction;
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
		Creature oCreature = null;
		
		public GridCell(float top, float bottom) {
			topPosition = top;
			bottomPosition = bottom;
			
			hasCreature = isGenerate(topPosition, bottomPosition);
			if (hasCreature)
				oCreature = new Creature(topPosition, bottomPosition);
		}
		
		public void Update() {
			
			if (hasCreature)
				oCreature.Update();
		}
		
		public void updateStatus(int objID, int newStatus) {
		
			if (hasCreature && oCreature.ObjID == objID)
				oCreature.setStatus(newStatus);
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
	protected int iType;
	protected int iStatus;
	protected float fSpeed;
	protected int iDirection;
	protected Config objectDetails;
	
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
	
	public void setStatus(int status) {
		
		iStatus = status;
		
		if (iStatus == 2)
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
	protected abstract int generateType(float top, float bottom, Config oDetails);
	//int generateType(float top, float bottom);// { return 0; }
	
	//protected abstract List<string> readTypes(); //returns the type of objects (e.g. fish, boxes)
	
	public void whenCollide() {}
}

public class Creature : Base {
	
	public Creature(float top, float bottom) : base(top, bottom) {
		objectDetails = new Config("Assets/Resources/allfish.txt");
		obj.tag = "Creature";
		iStatus = 1;
		
		iType=generateType(top,bottom,objectDetails);
		
		Material mat = Resources.Load("Materials/m" + objectDetails.getTypes()[iType], typeof(Material)) as Material;
		obj.renderer.material = mat;
		obj.transform.localScale = new Vector3(3, 1.5F, 1);
		obj.transform.Rotate(0, 180, 0);
		
		//fSpeed = Random.Range(0.01F, 0.20F);
		fSpeed = Random.Range(objectDetails.getSpeed()[0,iType], objectDetails.getSpeed()[1,iType]);

	}
	
	public void whenCollide() {
		if (iStatus == 1) {
			Application.LoadLevel("GameOver");
			Debug.Log("die");
		}
	}
		
	protected override int generateType(float top, float bottom, Config oDetails) {
		int index = Random.Range(0,oDetails.getTypes().Length - 1);
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
	
	public string[] getTypes(){
		return type;
	}
	
	public float[,] getSize(){
		return size;
	}
	
	public float[,] getSpeed(){
		return speed;
	}
	
	public int[] getHealth(){
		return health;
	}
	
	public int[] getPoints(){
		return points;
	}
}