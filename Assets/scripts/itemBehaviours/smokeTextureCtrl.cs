using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeTextureCtrl : MonoBehaviour {
	Renderer mRend;
	Material mat;
	public float smokeDistance;
	public float solidDistance;
	public float scrollSpeed;
	float gradient;
	float displace;
	//Archive viewer
	public Sprite archiveItem;
	bool itemViewed=false;
	public string itemDescription;
	
	// Use this for initialization
	void Start () {
		mRend=GetComponent<Renderer>();
		mat=mRend.material;
		mat.SetFloat("_SmokeAmount",1);
		gradient=1.0f/(smokeDistance-solidDistance);
		displace=-gradient*solidDistance;
	}
	
	// Update is called once per frame
	void Update () {
		if(!Global.bearingSet){
			return;
		}
		float distance=distanceFromPlayer();
		if(distance<=1 && !itemViewed){
			archiveViewer.setText(itemDescription);
			archiveViewer.setSprite(archiveItem);
			itemViewed=true;
		}
		else if (distance>20){
			itemViewed=false;
		}
		float smokeAmount = gradient*distance+displace;
		if(smokeAmount<0){
			smokeAmount=0;
		}
		if(smokeAmount>1){
			smokeAmount=1;
		}
		mat.SetFloat("_SmokeAmount",smokeAmount);
		float offsetX = Time.time * scrollSpeed/3;
		float offsetY = Time.time * scrollSpeed;
        mat.SetTextureOffset("_SmokeTex", new Vector2(offsetX, offsetY));	
	}

	float distanceFromPlayer(){
		Camera mainCam;
		mainCam=Camera.main;
		Vector3 playerPos=mainCam.transform.position;
		float distance = Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
			+Mathf.Pow(playerPos.z-transform.position.z, 2));
		return distance;
	}
}
