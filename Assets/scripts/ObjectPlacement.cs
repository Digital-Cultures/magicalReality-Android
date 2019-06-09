using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using System;

using ImaginationOverflow.UniversalDeepLinking;
using UnityEngine.Networking;


public class ObjectPlacement : MonoBehaviour {
	//Game object properties
	public double latitude;
	public double longitude;
    public int id;
    public float transparentDistance = 500;
	public float solidDistance = 10;
	public bool faceCamera=true;
	public bool useGlobalDistance=true;
	private Vector3 worldPos;
	public bool positionSet=false;
	private float minDistance=9999999.0f;
	float alpha=0;
	private dLocation lonLat;
	private Camera mainCam;
	private float playerDistance=0f;
	// private bool recalcualtePos=false;
	// private float recalculateDistance;
	bool visible=false;

    public Text debugText;

    // Use this for initialization
    void Start () {
		setVisible(false);
		mainCam=Camera.main;
		lonLat=new dLocation(longitude,latitude);

        //
        //DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
        Debug.Log("------------------Start Object Placment ");
    }
	
    public void SetLonLat(double longitudeVal, double latitudeVal)
    {
        longitude = longitudeVal;
        latitude = latitudeVal;
    }

    public void SetID(int ID)
    {
        id = ID;
    }


    //private void Instance_LinkActivated(LinkActivation linkActivation)
    //{
    //    //
    //    //  my activation code
    //    //
    //    var uri = linkActivation.Uri;
    //    var querystring = linkActivation.RawQueryString;
    //    Global.userid = linkActivation.QueryString["user"];
    //    Global.walkid = linkActivation.QueryString["walkid"];


    //    Debug.Log("TRY TO LOAD FROM LINK ON MAIN PAGE!");

    //  //  coroutine = GetRequest(OriginalJsonSite);
    //    //coroutine = GetRequest(OriginalJsonSite + userid + ".json");
    //   // StartCoroutine(coroutine);

    //}

    // Update is called once per frame
    void Update () {

        //Debug.Log("--------ObjectPlacement----------: " + Global.originSet  +"  "+ transform.name + "  " + playerDistance + "  " + transparentDistance + "  " + solidDistance + "  " + alpha);

        Global.objectDistance[id] = playerDistance;
        Global.objectAlpha[id] = alpha;

        if (Global.originSet){ //&& Global.bearingSet
			//Init
			if(!positionSet){
				worldPos=Conversions.lonLatToXZ(Global.dOrigin,lonLat);
				transform.localPosition=worldPos;
				positionSet=true;
			}

			
			if (Time.frameCount % Global.opacityInterval == 0){
				if(!positionSet || !Global.bearingSet){
					return;
				}
				setVisible(visible);
				playerDistance=distanceFromPlayer();

				//Set Opacity
				setOpacity();

				//Recalculate position from player
				/*
				if(playerDistance<recalculateDistance && recalcualtePos){
					Vector3 xyzDistance=Conversions.lonLatToXZ(Global.dPlayerLatLon,lonLat);
					//Vector3 localPlayer=Conversions.lonLatToXZ(Global.dOrigin,Global.dPlayerLatLon);
					Vector3 localPlayer=mainCam.transform.position;
					worldPos=new Vector3(localPlayer.x+xyzDistance.x,transform.position.y,localPlayer.z+xyzDistance.z);
					transform.localPosition=worldPos;
					recalcualtePos=false;
				}
				// else if(playerDistance>recalculateDistance && !recalcualtePos){
				// 	recalcualtePos=true;
				// }
				*/

				//Make the model face the player (only happens when approached for the first time)
				if(faceCamera){
					if(playerDistance<transparentDistance+2){
						Vector3 target = new Vector3(mainCam.transform.position.x,transform.position.y,mainCam.transform.position.z);
						transform.LookAt(target);
						faceCamera=false;
					}
				}

				//Adjust y position to nearst plane
				if(playerDistance<minDistance){
					transform.position=new Vector3(transform.position.x,Global.planeY,transform.position.z);
					minDistance=playerDistance;
				}

            }

		}

		
	}

	float distanceFromPlayer(){
		float distance;
		Vector3 playerPos=mainCam.transform.position;
		if(useGlobalDistance){
			if(Global.playerDistance.TryGetValue(name, out distance)){
				return distance;
			}
			else{
				distance = Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
					+Mathf.Pow(playerPos.z-transform.position.z, 2));
				Global.playerDistance.Add(name,distance);
			}
		}
		else{
			distance = Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
					+Mathf.Pow(playerPos.z-transform.position.z, 2));
		}
		return distance;
	}

	void setVisible(bool enable){
		Renderer rend;
		if(transform.childCount > 0){
			for(int i=0; i< transform.childCount; i++)
			{
				var child =transform.GetChild(i).gameObject;
			    if(child != null){
			    	rend = child.GetComponent<Renderer>();
			    	if(rend != null){
        				rend.enabled = enable;
        			}
        			else{
						child.SetActive(enable);
					}
				}
			}
		}else{
			rend = GetComponent<Renderer>();
			rend.enabled=enable;
		}
	}

	void setOpacity(){
		Renderer rend;
		float distance=playerDistance;

        //odd maths?
		double gradient=-1.0f/(transparentDistance-solidDistance);
		double displace=-(gradient*transparentDistance);
		alpha=(float)gradient*distance+(float)displace+4;

		Color currentCol;
		if(GetComponent<ParticleSystem>() != null)
 		{
 			var tempSys=GetComponent<ParticleSystem>().main;
 			currentCol=new Color(1.0f,1.0f,1.0f,alpha);
 			tempSys.startColor = new ParticleSystem.MinMaxGradient(currentCol);
 			return;
 		}
		if(transform.childCount > 0){
			for(int i=0; i< transform.childCount; i++)
			{
				var child =transform.GetChild(i).gameObject;
			    if(child != null){
	    			if(child.GetComponent<ParticleSystem>() != null)
				 		{
				 			var tempSys=child.GetComponent<ParticleSystem>().main;
				 			currentCol=new Color(1.0f,1.0f,1.0f,alpha);
				 			tempSys.startColor = new ParticleSystem.MinMaxGradient(currentCol);

				 		}
				 	else{
				    	rend = child.GetComponent<Renderer>();
				    	currentCol=rend.material.color;
				    	if(alpha>0){
				    		currentCol.a=alpha;
    				    	if(!visible){
					    		visible=true;
					    	}
				    	}
				    	else{
                            currentCol.a=0.0f;
                            visible=false;
                        }
                        rend.material.color=currentCol;
				    }
        		}
			}
		}else{
			rend = GetComponent<Renderer>();
			currentCol=rend.material.color;
	    	if(alpha<0){
	    		currentCol.a=0.0f;
	    		visible= false;
	    	}
	    	else if(alpha>1){
		    	currentCol.a=1.0f;
		    	visible=true;
	    	}
	    	
	    	else{
	    		currentCol.a=alpha;
		    	if(!visible){
		    		visible=true;
		    	}
	    	}
	    	rend.material.color=currentCol;
		}
	}
}
