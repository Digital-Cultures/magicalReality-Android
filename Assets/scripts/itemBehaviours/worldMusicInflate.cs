using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldMusicInflate : MonoBehaviour {
	private Renderer rend;
	private float inflation=0.11f;
	private Camera mainCam;

	//Archive viewer
	public Sprite archiveItem;
	bool itemViewed=false;
	public string itemDescription;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		rend=GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(!Global.bearingSet){
			return;
		}
		float distance=distanceFromPlayer();
		if(rend!=null && distance<3){
			Vector2 camXZ=new Vector2(0,mainCam.transform.forward.z);
			Vector2 modelXZ=new Vector2(transform.up.x,transform.up.z);
			inflation=1-Vector2.Dot(camXZ,modelXZ);
			if(inflation<0.1 && inflation>-0.1){
				if(!itemViewed){
					archiveViewer.setText(itemDescription);
					archiveViewer.setSprite(archiveItem);
					itemViewed=true;
					Handheld.Vibrate();
				}
			}
			//Debug.Log("F  "+transform.up+" C "+mainCam.transform.forward+" I "+inflation);
			rend.material.SetFloat("_Extrusion",inflation);
		}
	}

	float distanceFromPlayer(){
		Vector3 playerPos=mainCam.transform.position;
		float distance;
		if(Global.playerDistance.ContainsKey(transform.parent.name)){
			distance=Global.playerDistance[transform.parent.name];
		}
		else{
			distance = Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
				+Mathf.Pow(playerPos.z-transform.position.z, 2));
			
		}
		return distance;
	}
}
	