using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class poi : MonoBehaviour {
	public GameObject uiContainer;
	public GameObject marker;
	public Sprite visitedIcon;
	public float compassWidth = 975; 
	private GameObject markerGO;
	private Vector3 startPosition;
	private Camera cam;
	public float transparentDistance;
	public float solidDistance = 5;
	private float playerDistance=100;
	private bool visited=false;

	// Use this for initialization
	void Start () {
		cam=Camera.main;
		markerGO = Instantiate(marker) as GameObject;
		//markerGO.transform.SetParent(uiContainer.transform);
		startPosition = new Vector3(0,0,0);
		markerGO.transform.localPosition=startPosition;
		markerGO.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Global.bearingSet){
			if(!markerGO.activeSelf){
				markerGO.SetActive(true);
			}
			markerGO.transform.localPosition = markerPosition();
			if (Time.frameCount % Global.opacityInterval == 1){
				playerDistance=distanceFromPlayer();
				if(playerDistance<solidDistance/2 ){
					if(!visited){
						visited=true;
						Image img=markerGO.GetComponent<Image>();
						img.sprite=visitedIcon;
					}
				}
				setOpacity();
			}

			//Debug.Log("pos  "+markerGO.transform.localPosition);
		}else if(CompareTag("headingTrig")){
			if(!markerGO.activeSelf){
				markerGO.SetActive(true);
			}
			markerGO.transform.localPosition = markerPosition();
			if (Time.frameCount % Global.opacityInterval == 1){
				playerDistance=distanceFromPlayer();
				setOpacity();
			}
		}else{
			playerDistance=distanceFromPlayer();
		}
	}

	Vector3 markerPosition(){
		Vector3 heading=transform.position-cam.transform.position;
		Vector3 normalizedHeading=heading/heading.magnitude;
		Vector3 perp = Vector3.Cross(normalizedHeading, cam.transform.forward);
        float dir = Vector3.Dot(perp, Vector3.up);
        // return new Vector3(Vector3.Angle(normalizedHeading,cam.transform.forward) * -Mathf.Sign(dir) * rationAngleToPixel,0,0);
        Vector2 camXZ=new Vector2(cam.transform.forward.x,cam.transform.forward.z);
        Vector2 normHeadingXZ=new Vector2(normalizedHeading.x,normalizedHeading.z);
        float angle=Vector2.Angle(normHeadingXZ,camXZ);
        if(float.IsNaN(angle)){
        	return new Vector3(0,7.0f,0);
        }
        //float angle=Vector3.Angle(normalizedHeading,Camera.main.transform.forward);
        Vector3 outvector=new Vector3(-2*Mathf.Sign(dir)*angle,7.0f,0);
        return outvector;
	}

	float distanceFromPlayer(){
		Vector3 playerPos=cam.transform.position;
		float distance = Conversions.xzDistance(playerPos,transform.position);
		// Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
		//		+Mathf.Pow(playerPos.z-transform.position.z, 2));
		if(Global.playerDistance.ContainsKey(name)){
			Global.playerDistance[name]=distance;
		}
		else{
			Global.playerDistance.Add(name,distance);
		}
		return distance;
	}

	void setOpacity(){
			Image markerImg=markerGO.GetComponent<Image>();
			double gradient=-1.0f/(transparentDistance-solidDistance);
			double displace=-(gradient*transparentDistance);
			float alpha=(float)gradient*playerDistance+(float)displace;
	    	if(alpha<0){
	    		alpha=0.0f;
	    	}
	    	else if(alpha>1){
		    	alpha=1.0f;
	    	}
			Color col =markerImg.color;
			col.a=alpha;
			markerImg.color=col;
	}

	public void destroyMarker(){
		Destroy(markerGO);
	}
}
