using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AccuraGPS
{
public class LineGenerator : MonoBehaviour {
	public List<Vector3>points;
    public List<Vector3>points2;
	public Material lineMat;
    public Material lineMat2;
	public float lwidth;
	private GameObject line;
    private GameObject line2;
	private LineRenderer LR;
    private LineRenderer LR2;
	GameObject locCtrl;
	public GameObject originMarker;
	public GameObject worldRoot;
	LocationController lcScript;
	// Use this for initialization
	void Start () {
		line=new GameObject();
		line.AddComponent<LineRenderer>();
		line.name="GPS";
		LR=line.GetComponent<LineRenderer>();
		LR.material=lineMat;
		LR.startWidth=lwidth;
		LR.endWidth=lwidth;

        line2=new GameObject();
        line2.AddComponent<LineRenderer>();
        line2.name="CAM";
        LR2=line2.GetComponent<LineRenderer>();
        LR2.material=lineMat2;
        LR2.startWidth=lwidth/2;
        LR2.endWidth=lwidth/2;
        line.SetActive(false);
        line2.SetActive(false);
		originMarker.SetActive(false);
		points=new List<Vector3>();
        points2=new List<Vector3>();
		locCtrl= GameObject.Find("locationController");
		lcScript=locCtrl.GetComponent<LocationController>();
		worldRoot=GameObject.Find("worldRoot");
	}
	
	// Update is called once per frame
	void Update () {
		bool originSet=lcScript.hasOrigin();
		if(originSet){
			//points=gpsPoints2xz(lcScript.locs,lcScript.origin);
			points=lcScript.rotGPS;
            points2=lcScript.camLocs;
			line.SetActive(true);
            line2.SetActive(true);
			Vector3[] vPoints=points.ToArray();
            Vector3[] cPoints=points2.ToArray();
			LR.positionCount=points.Count;
            LR2.positionCount=points2.Count;
			LR.SetPositions(vPoints);
            LR2.SetPositions(cPoints);
			originMarker.SetActive(true);
			originMarker.transform.position=points[points.Count-1];
		}
	}

	public List<Vector3> gpsPoints2xz(List<Vector2> in_points, Vector2 origin){
		List<Vector3>xzPoints=new List<Vector3>();
		foreach(Vector2 p in in_points){
			Vector2 xz = Conversions.lonLatToXZ(origin,p);
			xzPoints.Add(new Vector3(xz.x,-1.0f,xz.y));
		}
		return xzPoints;
	}
}
}