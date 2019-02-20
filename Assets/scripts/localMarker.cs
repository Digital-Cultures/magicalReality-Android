using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AccuraGPS
{
public class localMarker : MonoBehaviour {
	private dLocation lonLat;
	private Vector3 worldPos;
	// private GameObject locControl;
	// private LocationController lcScript;

	// Use this for initialization
	// void Start () {
 //        locControl=GameObject.Find("locationController");
	// 	lcScript=locControl.GetComponent<LocationController>();
	// }
	
	// Update is called once per frame
	void Update () {
		if(Global.originSet){
				lonLat=Global.dPlayerLatLon;
				worldPos=Conversions.lonLatToXZ(Global.dOrigin,lonLat);
				transform.localPosition=worldPos;
		}
		
	}
}
}
