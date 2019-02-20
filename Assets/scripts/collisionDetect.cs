using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
namespace AccuraGPS
{
	public class collisionDetect : MonoBehaviour {
		private GameObject worldRoot;
		private rotationSolver rSolver;
		

		// Use this for initialization
		void Start () {
			worldRoot=GameObject.Find("worldRoot");
	        rSolver=worldRoot.GetComponent<rotationSolver>();
		}
		
		// Update is called once per frame
		void Update () {
			if (Time.frameCount % 30 == 0){
 				findNearestPlane();
			}			
		}

		void OnTriggerEnter(Collider other){
			if(other.gameObject.CompareTag("headingTrig")){
				other.gameObject.SetActive(false);
				rSolver.addCollisionPoint();
				poi poiScript=other.gameObject.GetComponent<poi>();
				if(poiScript!=null){
					poiScript.destroyMarker();
				}
			}
		}

		void findNearestPlane(){
			float minDistance=9999999.0f;
			List<DetectedPlane> planes = new List<DetectedPlane>();
			List<Vector3> boundryPoints=new List<Vector3>();
			float distance=0;
			Session.GetTrackables<DetectedPlane>(planes);
			for (int i = 0; i < planes.Count; i++){
				planes[i].GetBoundaryPolygon(boundryPoints);
				for(int p=0;p<boundryPoints.Count;p++){
					distance=Mathf.Pow(transform.position.x-boundryPoints[i].x,2)+Mathf.Pow(transform.position.z-boundryPoints[i].z,2);
		        	if(distance<minDistance){
		        		minDistance=distance;
		        		Global.planeY=boundryPoints[i].y;
		        	}
				}	
	        }
		}
	}
}
