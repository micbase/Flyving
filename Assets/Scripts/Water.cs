using UnityEngine;
using System.Collections;

	
public class Water : MonoBehaviour
{
    static int size = 400; // Number of vertices
    static float velocityDamping = 0.999999f; // Proprotional velocity damping, must be less than or equal to 1.
    static float timeScale = 50f;
   
    float[] newHeight = new float[size];
    float[] velocity = new float[size];
	
    GameObject[] vertex = new GameObject[size];
	float rippleTimer = 0.0f;
 
	
    void Start ()
    {	
		//using this to set initial water particle heights
        float iHeight = 0.0f;
		Material waterMat = Resources.LoadAssetAtPath("Assets/darkWaterParticle.mat", typeof(Material)) as Material;
        
		for(int i=0; i<size; i++)
        {
            vertex[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			vertex[i].transform.localScale = new Vector3(0.1f,.5f,0.25f);
			vertex[i].renderer.material = waterMat;
			
			if(i==40||i==120){
				iHeight = 5.0f;	
			}
			else if(i==80 || i==240){
				iHeight = -10.0f;
			}
			else{
				iHeight = 0.0f;
			}
            vertex[i].transform.position = new Vector3((i - size/2)*.1f, iHeight, 0);
        }
    }
   
    void Update ()
    {
		rippleTimer += Time.deltaTime;
		print (rippleTimer.ToString());
		
		if(rippleTimer >= 5){
			int vertIndx = Random.Range(51,199);
			vertex[vertIndx].transform.position = new Vector3((vertIndx - size/2)*.1f, 5.0f, 0);
			vertex[vertIndx*Random.Range(1,2)].transform.position = new Vector3((vertIndx*Random.Range(1,2) - size/2)*.1f, -10.0f, 0);
			rippleTimer = 0.0f;
		}
        
		// Water tension is simulated by a simple linear convolution over the height field.
        for(int i=1; i<size-1; i++)
        {
            int j=i-1;
            int k=i+1;
            newHeight[i] = (vertex[i].transform.position.y + vertex[j].transform.position.y + vertex[k].transform.position.y) / 3.0f;
		}
       
        // Velocity and height are updated...
        for(int i=0; i<size; i++)
        {
            // update velocity and height
            velocity[i] = (velocity[i] + (newHeight[i] - vertex[i].transform.position.y)) * velocityDamping;
           
            float timeFactor = Time.deltaTime * timeScale;
            if (timeFactor > 1f) timeFactor = 1f;
           
            newHeight[i] += velocity[i] * timeFactor;
           
            // update the vertex position
            Vector3 newPosition = new Vector3(
                vertex[i].transform.position.x,
                newHeight[i],
                vertex[i].transform.position.z);
            vertex[i].transform.position = newPosition;
        }
    }
}