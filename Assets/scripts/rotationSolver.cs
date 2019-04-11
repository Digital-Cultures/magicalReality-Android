using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumUtils.NelderMeadSimplex;
using System.Reflection;
using System.Linq;
namespace AccuraGPS
{
public class rotationSolver : MonoBehaviour {
	public float baseRot;
	public float BaseRot{
		get{return baseRot;}
	}
	private bool baseSet=false;
	public bool hasBase{
		get{return baseSet;}
	}
	public float yAngle;
	public float angle{
        get{return yAngle;}
    }
    //private List<Vector2> collisions;
    private List<dLocation> dCollisions;
	public double threshold=50.0;
	public double tolerance=10e-6;
	public int maxEvals=20000;
	double diff=0;
	private GameObject locControl;
	private LocationController lcScript;
	public GameObject instruction;
	public int calibrationPoints;
	GameObject calibrator;
	public int minAlignmentPoints=15;
	SimplexConstant[] constants=new SimplexConstant[] {new SimplexConstant(180,40)};

	public Sprite archiveItem;
	public string itemDescription="";
	// Use this for initialization
	void Start () {
		instruction.SetActive(false);
		calibrator=GameObject.Find("headingCal");
		calibrator.SetActive(false);
		locControl=GameObject.Find("locationController");
		lcScript=locControl.GetComponent<LocationController>();
		yAngle=0;
		baseRot=0;
		//collisions=new List<Vector2>();
		dCollisions=new List<dLocation>();
	}
	
	// Update is called once per frame
	void Update () {	
		if(Global.tutorialComplete && Global.originSet && !baseSet){			
			instruction.SetActive(true);
			calibrator.SetActive(true);
			if(dCollisions.Count==calibrationPoints){

				if(!Global.sampleShown){
					archiveViewer.setText(itemDescription);
					archiveViewer.setSprite(archiveItem);
					if(PlayerPrefs.GetInt("sampleShown")==0){
						PlayerPrefs.SetInt("sampleShown",1);
					}
					Global.sampleShown=true;
				}
				float min=999999999999;
				float[] headings=new float[2*calibrationPoints-3];
				//Headings from origin = 0.5+dCollisions.Count/2
				for(int i=1;i<dCollisions.Count;i++){
					headings[i-1]=Conversions.bearing(Global.dOrigin,dCollisions[i]);
				}
				//Headings between points
				for(int i=0;i<dCollisions.Count-2;i++){
					headings[dCollisions.Count-1+i]=Conversions.bearing(dCollisions[i],dCollisions[i+2]);
				}

				foreach(float H0 in headings){
					float total=0;

					foreach(float Hn in headings){
						total+=Mathf.Abs(H0-Hn);
					}
					if(total<min){
						min=total;
						baseRot=H0;
					}
				}
				baseSet=true;
				Global.bearingSet=true;
				instruction.SetActive(false);
				calibrator.SetActive(false);
				//collisions=null;
				dCollisions=null;
				transform.rotation =Quaternion.Euler(0,-baseRot,0);
			}
	
		}

		if(lcScript.rotGPS.Count>minAlignmentPoints){
			
			diff=pointDiff(lcScript.rotGPS[lcScript.rotGPS.Count-1],lcScript.camLocs[lcScript.camLocs.Count-1]);
			if(baseSet && diff>threshold)
			{
				yAngle=360-(float)runCalculation(constants)*Mathf.Rad2Deg%360;
				baseRot=resultingAngle(new float[] {yAngle,baseRot});
				transform.rotation =Quaternion.Euler(0,-baseRot,0);//-baseRot

			}
		}
		
		

	}

	//RegressionResult
	double runCalculation(SimplexConstant[] consts){
		ObjectiveFunctionDelegate objFunc=new ObjectiveFunctionDelegate(minimizeAngle);
		RegressionResult result = NelderMeadSimplex.Regress(consts,tolerance,maxEvals,objFunc);
		return result.Constants[0];
	}

	double minimizeAngle(double[] constants){
		double error=0;
		double rotation=constants[0];
		for(int i=0;i<lcScript.dLocs.Count;i++){
			Vector3 rotated=Conversions.rotatePoint(Conversions.lonLatToXZ(lcScript.dOrigin,lcScript.dLocs[i]),rotation);
			error+=pointDiff(rotated,lcScript.camLocs[i]);
		}
		return error;
	}

	double pointDiff(Vector3 point1, Vector3 point2){
		double difference = Mathf.Sqrt(Mathf.Pow(point1.x-point2.x,2)+Mathf.Pow(point1.z-point2.z,2));
		return difference;
	}

	public void addCollisionPoint(){
		//collisions.Add(new Vector2((float)GPS.Longitude,(float)GPS.Latitude));
		dCollisions.Add(new dLocation(GPS.Longitude,GPS.Latitude));
	}

	private float resultingAngle(float[] angles){
		float sumX=0;
		float sumY=0;
		foreach (float angle in angles){
			sumX+=Mathf.Cos(angle*Mathf.Deg2Rad);
			sumY+=Mathf.Sin(angle*Mathf.Deg2Rad);
		}
		float result=Mathf.Atan2(sumY, sumX);
		return  result*Mathf.Rad2Deg;
	}

}
}
