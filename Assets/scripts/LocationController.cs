using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct dLocation{
    public double lat;
    public double lon;
    public dLocation(double longitude,double latitude){
        lat=latitude;
        lon=longitude;
    }
}

namespace AccuraGPS
{
    public class LocationController : MonoBehaviour {
    	public static LocationController instance{get; private set;}//What does this line do
        public GameObject waitingGPS;
        public GameObject gpsMarker;
        public List<dLocation> dLocations;
        public List<dLocation> dGpsLocs;
        public List<dLocation> dLocs{
            get{return dGpsLocs;}
        }

        //Camera point variables
        public List<Vector3> cameraLocs;
        public List<Vector3> camLocs{
            get{return cameraLocs;}
        }

        //Rotated X,Y,Z gps positions
        public List<Vector3> rotatedGPS;
        public List<Vector3> rotGPS{
            get{return rotatedGPS;}
        }
        //In vectors x is Lon and y is lat
        public Vector2 origin=new Vector2(0,0);
        public Vector2 Origin{
            get{return origin;}
        }
        
        public dLocation dOrigin=new dLocation(0,0);
        public dLocation DOrigin{
            get{return dOrigin;}
        }

        public dLocation dCurrentLoc= new dLocation(0,0);
        public dLocation DCurrentLoc {
            get{return dCurrentLoc;}
        }

        private int skipped=1;
        public float heading;
        public float Heading{
            get{
                return heading;
            }
        }
        private float coordsDif2=10000.0f; //Used to assess gps accuracy
        private double minLon= -100.00;
        private double minLat= -80.00;
        private double maxLon= 100.00;
        private double maxLat= 80.00;
        private double latitude;
        private double longitude;
        public bool originSet=false;
        public bool hasOrigin(){
                return originSet;
        }
        WaitForSeconds interval = new WaitForSeconds(2.0f);
        private Camera mainCam;

        //Define rotationSolver
        private GameObject worldRoot;
        private rotationSolver rSolver;
        
    	bool updateDisplay=false;

    	// Use this for initialization
    	void Awake () {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            //Double locations
            dLocations=new List<dLocation>();
            dGpsLocs=new List<dLocation>();

            cameraLocs= new List<Vector3>();
            mainCam=Camera.main;
    		if (instance == null)
    		{
    			instance = this;
    		}		
    		if (instance != this)
    		{
    			Destroy(gameObject);
    		}

    		InvokeRepeating("GetCurrentPos", 0, 0.55f);
    		InvokeRepeating("updateLatLng", 2, 1.5f);
    	}

    	IEnumerator Start()
        {
          //  Debug.Log("------------------Start :  ");
            waitingGPS.SetActive(false);
            worldRoot=GameObject.Find("worldRoot");
            rSolver=worldRoot.GetComponent<rotationSolver>();
            heading=0;
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
                yield break;
            
            do //Start Gps
            {
                GPS.Start();
                yield return interval;
            }
            while (!GPS.IsEnabledByUser);
            updateDisplay=true;
        }

    	//OWN FUNCTIONS
    	void GetCurrentPos() // Get position and add to lists
        {
            if (!Global.originSet && Global.tutorialComplete){
                waitingGPS.SetActive(true);
            }

             //Vector2 currentPoint;
            dLocation dCurrentPoint;
            //Are you in Newcastle?

           
            if (minLon<GPS.Longitude &&
                GPS.Longitude<maxLon && 
                minLat<GPS.Latitude &&
                GPS.Latitude<maxLat
                ){
                //currentPoint=new Vector2(Input.location.lastData.longitude,Input.location.lastData.latitude);
             //   Debug.Log("------------------Longitude: " + GPS.Longitude + "  Latitude: " + GPS.Latitude);
                dCurrentPoint = new dLocation(GPS.Longitude,GPS.Latitude);
            }
            else{
                //TODO add out of bounds behaviour
    
                dCurrentPoint = new dLocation(GPS.Longitude,GPS.Latitude);
                //return;
            }



            if (dLocations.Count < 10)
            {
                dLocations.Add(dCurrentPoint);
            }
            else // If list contains 10 items
            {
                dLocations.RemoveAt(0);
                dLocations.Add(dCurrentPoint);
                coordsDif2=dSquareDifference();
            }

            if(dLocations.Count>2){
                if(inRange(dCurrentPoint,dLocations[dLocations.Count-2])){
                    dCurrentLoc=dCurrentPoint;
                    skipped=1;
                }else{
                    skipped++;
                }
            }
            //infoUI.text="Camera X: "+mainCam.transform.position.x+" Z: "+mainCam.transform.position.z+" base: "+rSolver.BaseRot+" / "+heading;
        }

        
        void updateLatLng(){
            //GPS translated to xyz
            Vector3 xyz=new Vector3();
            xyz.x=0;
            xyz.y=0;
            xyz.z=0;
            float theta=rSolver.angle;
            if(dLocations.Count==0){
                return;
            }
        	if(updateDisplay){
                //Set origin if not set
                if(coordsDif2<0.001 && origin==new Vector2(0,0)){
                    dOrigin=new dLocation(averageLon(),averageLat());
                    Global.dOrigin=dOrigin;
                    origin=new Vector2 ((float)averageLon(),(float)averageLat());
                    Global.origin=origin;
                    //xyz=Conversions.lonLatToXZ(origin,currentLoc);
                    xyz=Conversions.lonLatToXZ(dOrigin,dCurrentLoc);
                    dGpsLocs.Add(dOrigin);
                    Global.dPlayerLatLon=dOrigin;
                    cameraLocs.Add(mainCam.transform.position);
                    //rotatedGPS.Add(xyz);
                    rotatedGPS.Add(gpsMarker.transform.position);
                    originSet=true;
                    Global.originSet=true;
                    //Debug.Log("origin= "+dOrigin.lon+","+dOrigin.lat);
                }


           //     Debug.Log("------------------updateLatLng :  originSet: " + originSet);
                //Update display
                if (originSet){
                    waitingGPS.SetActive(false);
        
                   xyz =Conversions.lonLatToXZ(dOrigin,dCurrentLoc);
                    if(dGpsLocs.Count<150){
                        dGpsLocs.Add(dCurrentLoc);
                        Global.dPlayerLatLon=dCurrentLoc;
                        cameraLocs.Add(mainCam.transform.position);
                        rotatedGPS.Add(gpsMarker.transform.position);

                    }else{
                        dGpsLocs.RemoveAt(0);
                        cameraLocs.RemoveAt(0);
                        rotatedGPS.RemoveAt(0);
                        dGpsLocs.Add(dCurrentLoc);
                        Global.dPlayerLatLon=dCurrentLoc;
                        cameraLocs.Add(mainCam.transform.position);
                        rotatedGPS.Add(gpsMarker.transform.position);
                    }
                }
                setRegion(dCurrentLoc);
        	}
        }

        bool inRange(Vector2 lonLat, Vector2 lastLonLat, float range=2.0f){
            if(Conversions.coordsDistance(lastLonLat, lonLat)<range*skipped){
                return true;
            }else{
                return false;
            }
        }

        bool inRange(dLocation lonLat, dLocation lastLonLat, float range=2.0f){
            if(Conversions.coordsDistance(lastLonLat, lonLat)<range*skipped){
                return true;
            }else{
                return false;
            }
        }

        double averageLon(){
            double sum=0;
            int count=0;
            foreach(dLocation loc in dLocations){
                sum+=loc.lon;
                count++;
            }
            return sum/count;
        }

        double averageLat(){
            double sum=0;
            int count=0;
            if(dLocations.Count==0){
                count=1;
            }
            foreach(dLocation loc in dLocations){
                sum+=loc.lat;
                count++;
            }
            return sum/count;
        }

        float dSquareDifference(){
            double sumOf2s=0;
            if(dLocations.Count>2){
                for(int i=1;i<dLocations.Count;i++){
                    double diff =(dLocations[i].lon-dLocations[i-1].lon)+(dLocations[i].lat-dLocations[i-1].lat);
                    double diff2=diff*diff;
                    sumOf2s+=diff2;
                }

            }
            return (float)sumOf2s;
        }

        void setRegion(dLocation location){
            double distance=Conversions.coordsDistance(location,Global.region1);
            if(distance<150000){
                Global.ActiveSet=0;
            }
            else{
                Global.ActiveSet=3;
            }
        }
    }
}