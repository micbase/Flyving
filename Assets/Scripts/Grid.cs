using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public enum ObjStatus { Normal = 1, Stop, Invisible };
public enum GameDirection { DivingDown = 1, DivingUp, FlyingUp, FlyingDown, GameOver };
public enum CellType { Creature = 1, Weapon, Treasure, Oxygen, Star };
public enum WeaponBoxType { Gun = 1, Spear, Bomb };
public enum TreasureBoxType { Undefeat = 1, SlowDown, Inverse, SpeedUp, Bigger, Dark };
public enum WeaponType { Gun = 1, Bomb, Spear, noWeapon };
public enum PlayerEffect { Inverse = 1, Undefeat, SlowDown, SpeedUp, Bigger, Dark, noEffect };

public class Grid : MonoBehaviour {

    public float speedFactor = 1;
    float gameSpeed = -0.1f;
    float baseSpeed = -0.1f;

    int gridSize;
    float gridMargin;
    float gridHeight;
    float currentHeight;
    float currentBGHeight;
    float initialHeight = -15;
    float initialBGHeight = -26f;
    List<GridCell> gridArraySea;
    List<GridCell> gridArraySky;

    GameObject oPlayer;
    GameObject oCamera;
    GameObject oBlackPlane;

    Config[] objectDetails;
    GameDirection currentDirection;

    Player player;

    void Start () {

        oPlayer = GameObject.Find("Player");
        oCamera = GameObject.Find("Main Camera");
        oBlackPlane = GameObject.Find("BlackPlane");

        player = oPlayer.GetComponent("Player") as Player;

        objectDetails = new Config[2];
        objectDetails[0] = new Config("Assets/Resources/allfish.txt");
        objectDetails[1] = new Config("Assets/Resources/allbird.txt");

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

        if (!player.isPaused) {

            float startPoint;
            float endPoint;
            int iStart = 0;
            int iEnd = 0;

            //Update camera and background
            oCamera.transform.Translate(0, GameSpeed, 0);

            if (oCamera.transform.localPosition.y < 0) {
                oBlackPlane.renderer.enabled = true;
                oBlackPlane.transform.Translate(0, GameSpeed, 0);
            }
            else {
                oBlackPlane.renderer.enabled = false;
            }
			
			if (player.currentEffect != PlayerEffect.Dark) {
	            if (oCamera.transform.localPosition.y > -500) {
	
	                oBlackPlane.renderer.material.color = new Color(0, 0, 0, (float)(Mathf.Abs(oCamera.transform.localPosition.y) * 0.3) / 255);
	            }
				else {
					
					oBlackPlane.renderer.material.color = new Color(0, 0, 0, 0.588f);
				}
			}

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
                GameSpeed = Mathf.Abs(baseSpeed) * 2;
            }
            else if (value == GameDirection.FlyingDown || value == GameDirection.DivingDown)
                GameSpeed = (-1) * Mathf.Abs(baseSpeed);
            else
                GameSpeed = 0;
        }
    }

    public float GameSpeed {
        get {
            return gameSpeed * speedFactor;
        }

        set {
            gameSpeed = value;
        }
    }

    void GenerateGrid() {

        if (currentDirection == GameDirection.DivingDown) {
            for (int i = 0; i < gridSize; i++) {
                gridArraySea.Add(new GridCell(currentHeight, currentHeight - gridHeight, objectDetails[0]));
                currentHeight -= gridMargin + gridHeight;
            }

            while (currentBGHeight > currentHeight) {
                GenerateBackground();
            }
        }
        else if (currentDirection == GameDirection.FlyingUp) {
            for (int i = 0; i < gridSize; i++) {
                gridArraySky.Add(new GridCell(currentHeight, currentHeight + gridHeight, objectDetails[1]));
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
                    player.Life--;

                    if (player.Life <= 0) {
                        Application.LoadLevel("GameOver");
                        Debug.Log("killed by bomb");
                    }
                }

                for (int i = index - 5; i <= index + 5; i++) {
                    if (i >= 0)
                        gridArraySea[i].applyWeapon(objID, pos, weaponType);
                }
            }
        }
    }

    public int whenCollide(int objID, float top, CellType type) {
        StartCoroutine(FlashWhenHit());

        if (currentDirection == GameDirection.DivingDown || currentDirection == GameDirection.DivingUp) {
            int index = Mathf.CeilToInt((initialHeight - top) / (gridMargin + gridHeight)) - 1;
            return gridArraySea[index].whenCollide(objID, type);
        }
        else if (currentDirection == GameDirection.FlyingUp || currentDirection == GameDirection.FlyingDown) {
            int index = Mathf.CeilToInt((initialHeight + top) / (gridMargin + gridHeight)) - 1;
            return gridArraySky[index].whenCollide(objID, type);
        }

        return 0;
    }

    public class GridCell {

        float topPosition;
        float bottomPosition;

        bool hasCreature = false;
        bool hasTreasure = false;
        bool hasWeapon = false;
        bool hasOxygen = false;
        bool hasStar = false;

        Creature oCreature = null;
        TreasureBox oTreasure = null;
        WeaponBox oWeapon = null;
        OxygenCan oOxygen = null;
        Star oStar = null;

        public GridCell(float top, float bottom, Config objectDetail) {
            topPosition = top;
            bottomPosition = bottom;

            hasCreature = isGenerateCreature(topPosition, bottomPosition);
            if (hasCreature)
                oCreature = new Creature(topPosition, bottomPosition, objectDetail);

            hasWeapon = isGenerateWeapon(topPosition, bottomPosition);
            if (hasWeapon) 
                oWeapon = new WeaponBox(topPosition, bottomPosition);

            hasTreasure = isGenerateTreasure(topPosition, bottomPosition);
            if (hasTreasure) 
                oTreasure = new TreasureBox(topPosition, bottomPosition);

            hasOxygen = isGenerateOxygen(topPosition, bottomPosition);
            if (hasOxygen) {
                oOxygen = new OxygenCan(topPosition, bottomPosition);
            }

            hasStar = isGenerateStar(topPosition, bottomPosition);
            if (hasStar) {
                oStar = new Star(topPosition, bottomPosition);
            }
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

        public int whenCollide(int objID, CellType type) {

            switch (type) {

                case CellType.Creature:
                    if (hasCreature)
                        return oCreature.whenCollide();

                    break;

                case CellType.Weapon:
                    if (hasWeapon)
                        return oWeapon.whenCollide();

                    break;

                case CellType.Treasure:
                    if (hasTreasure)
                        return oTreasure.whenCollide();

                    break;

                case CellType.Oxygen:
                    if (hasOxygen)
                        return oOxygen.whenCollide();

                    break;
				
                case CellType.Star:
                    if (hasStar)
                        return oStar.whenCollide();

                    break;
            }

            return 0;
        } 

        bool isGenerateCreature(float top, float bottom) {

            if (top > -150 || top < 150) {
                return (Random.Range (0.0F, 1.0F) < 0.4F);
            }
            else if (top > -200 || top < 200) {
                return (Random.Range (0.0F, 1.0F) < 0.6F);
            }
            else if (top > -300 || top < 300) {
                return (Random.Range (0.0F, 1.0F) < 0.8F);
            }
            else {
                return (Random.Range (0.0F, 1.0F) < 1.0F);
            }
        }

        bool isGenerateWeapon(float top, float bottom) {

            if (top < 0) {
                return (Random.Range (0.0f,1.0f) < 0.1f);
            }
            else {
                return false;
            }
        }

        bool isGenerateTreasure(float top, float bottom) {

            if (top < 0) {
                return (Random.Range (0.0f,1.0f) < 0.08f);
            }
            else {
                return false;
            }
        }

        bool isGenerateOxygen(float top, float bottom) {

            if (top < 0) {
                return (Random.Range (0.0f,1.0f) < 0.02f);
            }
            else {
                return false;
            }
        }

        bool isGenerateStar(float top, float bottom) {

            if (top > 0) {
                return (Random.Range (0.0f,1.0f) < 0.4f);
            }
            else {
                return false;
            }
        }
    }

    public IEnumerator Fade (float start, float end, float length, GameObject currentObject) { //define Fade parmeters
        if (currentObject.guiTexture.color.a == start){
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length)) { //for the length of time

                Color temp; // temporary variable
                temp = currentObject.guiTexture.color;
                temp.a = Mathf.Lerp(start, end, i); // start appearing when health < 100%
                yield return true;
                temp.a = end;
                currentObject.guiTexture.color = temp;

            } //end for 
        } //end if

    } //end Fade

    public IEnumerator FlashWhenHit(){
        GameObject oTexture = GameObject.Find("Oxygenbar");
        Fade (0, 0.8f, 0.5f, oTexture);
        yield return new WaitForSeconds (.01f);
        Fade (0.8f, 0, 0.5f, oTexture);
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
        else{
            obj.renderer.enabled = true;
            obj.renderer.enabled = true;	
        }
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
    public abstract int whenCollide();
}

public class TreasureBox: Base {

    public TreasureBox(float top, float bottom) : base(top, bottom) {
        obj.tag = "TreasureBox";
        iStatus = ObjStatus.Stop;
        iType = generateType(top, bottom, null);
        Material mat = Resources.Load ("Materials/TreasureBox", typeof(Material)) as Material;
        obj.renderer.material = mat;		
        obj.transform.localScale = new Vector3(3.0f, 3.0f, 0.001F);
        fSpeed = 0;
    }

    public override int whenCollide() {

        base.setStatus(ObjStatus.Invisible);
        return iType;
    }

    protected override int generateType(float top, float bottom, Config oDetails) {
		
        if (top > -200)
            return Random.Range(1, 3);
        else
            return Random.Range(1, 7);
    }

    protected override void changeDirection(int newDirection) {}
}

public class WeaponBox: Base {

    public WeaponBox(float top, float bottom) : base(top, bottom) {

        obj.tag = "WeaponBox";
        iStatus = ObjStatus.Stop;
        iType = generateType(top, bottom, null);
        Material mat = Resources.Load ("Materials/mWeapon", typeof(Material)) as Material;
        obj.renderer.material = mat;
        obj.transform.localScale = new Vector3(3.0f, 3.0f, 0.001F);
    }

    public override int whenCollide() {

        base.setStatus(ObjStatus.Invisible);
        return iType;
    }

    protected override int generateType(float top, float bottom, Config oDetails) {
		
        return Random.Range(1, 4);
    }

    protected override void changeDirection(int newDirection) {}
}


//Oxygen can class
public class OxygenCan: Base {

    public OxygenCan(float top, float bottom) : base(top, bottom) {

        obj.tag = "OxygenCan";
        iStatus = ObjStatus.Stop;
        Material mat = Resources.Load ("Materials/mOxygenCan", typeof(Material)) as Material;
        obj.renderer.material = mat;
        obj.transform.localScale = new Vector3(3.0f, 3.0f, 0.001F);
    }

    public override int whenCollide() {

        base.setStatus(ObjStatus.Invisible);
        return 0;
    }

    protected override int generateType(float top, float bottom, Config oDetails) { return 0;}
    protected override void changeDirection(int newDirection) {}
}

public class Star: Base {

    Dashboard dashBoard;

    public Star(float top, float bottom) : base(top, bottom) {

        obj.tag = "Star";
        iStatus = ObjStatus.Stop;
        Material mat = Resources.Load ("Materials/mStar", typeof(Material)) as Material;
        obj.renderer.material = mat;
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 0.001F);
        obj.transform.Rotate(0, 180, 0);

        dashBoard = GameObject.Find("Main Camera").GetComponent("Dashboard") as Dashboard;
    }

    public override int whenCollide() {

        dashBoard.iScore += 50;
        base.setStatus(ObjStatus.Invisible);
        return 0;
    }

    protected override int generateType(float top, float bottom, Config oDetails) { return 0;}
    protected override void changeDirection(int newDirection) {}
}

public class Creature : Base {

    int iHealth;
    Config oCDetails;
    Dashboard dashBoard;
    Grid grid;
    Player player;

    public Creature(float top, float bottom, Config details) : base(top, bottom) {

        obj.tag = "Creature";
        oCDetails = details;
        iStatus = ObjStatus.Normal;
        iType = generateType(top, bottom, oCDetails);
        iHealth = oCDetails.getHealth(iType);

        dashBoard = GameObject.Find("Main Camera").GetComponent("Dashboard") as Dashboard;
        grid = GameObject.Find("Main Camera").GetComponent("Grid") as Grid;
        player = GameObject.Find("Player").GetComponent("Player") as Player;

        Material mat;
        if (iDirection == 1){
            mat	= Resources.Load("Materials/m" + oCDetails.getTypes(iType) + "Left", typeof(Material)) as Material;
        }
        else {
            mat	= Resources.Load("Materials/m" + oCDetails.getTypes(iType) + "Right", typeof(Material)) as Material;
        }

        obj.renderer.material = mat;
        obj.transform.localScale = new Vector3(oCDetails.getSize(iType)[0], oCDetails.getSize(iType)[1], 0.001F);

        fSpeed = Random.Range(oCDetails.getSpeed(iType)[0], oCDetails.getSpeed(iType)[1]);
    }

    public void attackedBy(WeaponType weaponType) {

        if (iStatus == ObjStatus.Normal) {

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

            if (iHealth <= 0) {
                //base.setStatus(ObjStatus.Invisible);
                base.setStatus(ObjStatus.Stop);
                obj.transform.Rotate(0, 180, 0);	
            }
        }
    }

    public override int whenCollide() {

        if (grid.CurrentDirection == GameDirection.DivingDown || grid.CurrentDirection == GameDirection.FlyingDown) {

            if (oCDetails.getCategory(iType) >= 1 && iStatus == ObjStatus.Normal) {
                player.Life--;

                if (player.Life <= 0) {
                    Application.LoadLevel("GameOver");
                    Debug.Log("die");

                }
            }
        }

        if (grid.CurrentDirection == GameDirection.DivingUp || grid.CurrentDirection == GameDirection.FlyingUp) {

            if ((oCDetails.getCategory(iType) == 0 && iStatus != ObjStatus.Invisible) 
                    || iStatus == ObjStatus.Stop) {

                dashBoard.iScore += oCDetails.getPoints(iType);
                base.setStatus(ObjStatus.Invisible);
            }
            else if (oCDetails.getCategory(iType) >= 1 && iStatus == ObjStatus.Normal) {
                player.Life--;

                if (player.Life <= 0) {
                    Application.LoadLevel("GameOver");
                    Debug.Log("die");
                }
            }
        }
        return 0;
    }
    //public Player 
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
        //After reading the config file, we use the selectedIndex 

        int index = 0;
        int[] indCat = new int[details.getCategorys().Length];
        List<int> selectedIndex = new List<int>();

        float small, medium;
        int category = 0, firstDepth = -125, secondDepth = -250; 

        float randNum = Random.Range(0.0f, 1.0f);

        if(top > firstDepth){
            small = 0.8f;
            medium = 0.98f;
        }
        else if ((top < firstDepth) && (top > secondDepth)){
            small = 0.4f;
            medium = 0.9f;
        }
        else{
            small = 0.1f;
            medium = 0.4f;
        }

        if(randNum < small)
            category = 0;
        else if ((randNum>small) && (randNum<medium))
            category = 1;
        else
            category = 2;

        indCat = details.getCategorys();

        for(int i=0; i<indCat.Length; i++){
            if(indCat[i] == category){
                selectedIndex.Add(i);
            }
        }

        int rInd = Random.Range(0,selectedIndex.Count);
        index = selectedIndex[rInd];
        selectedIndex.Clear(); 

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
    int[] category;

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
        category = new int[lineCount];

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
            category[i] = int.Parse(s[7], CultureInfo.InvariantCulture.NumberFormat);
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


    public int getCategory(int iType) {
        return category[iType];
    }

    public int[] getCategorys(){
        return category;
    }
}
