using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpScale : MonoBehaviour {
Vector3 acceldata;
	public float increment=0.2f;
	public float maxScale;
	public float jumpDistance=3;
	public GameObject jumpText;
	bool atRest=true;
	//Archve viewer
	public Sprite archiveItem;
	bool itemViewed=false;
	public string itemDescription;
	
	// Use this for initialization
	void Start () {
		acceldata=new Vector3(0,0,0);
		if(jumpText!=null){
			jumpText.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!Global.bearingSet){
			return;
		}
		acceldata= Input.acceleration;
		float distance;
		if(Global.playerDistance.TryGetValue(name, out distance)){
			if(distance<=jumpDistance){
				if(atRest && transform.localScale.y<maxScale){
					if(jumpText!=null){
						jumpText.SetActive(true);
					}
					if(acceldata.y>-0.1){
						Vector3 scale=new Vector3(transform.localScale.x+increment,
							transform.localScale.y+increment,
							transform.localScale.z+increment);
						transform.localScale=scale;
						atRest=false;
					}
				}
				else{
					if(jumpText!=null){
						jumpText.SetActive(false);
					}
				}

				if(transform.localScale.y>maxScale && !itemViewed){
					archiveViewer.setText(itemDescription);
					archiveViewer.setSprite(archiveItem);
					itemViewed=true;
				}
			}
		}

		if(acceldata.y<-0.3){
			atRest=true;
		}
	}
}
