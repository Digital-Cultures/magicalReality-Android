using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour {
	Transform camTransform;
	Vector3 target;
	//Archive viewer
	public Sprite archiveItem;
	bool itemViewed=false;
	public string itemDescription;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		camTransform = Camera.main.transform;
		target=new Vector3(camTransform.position.x,transform.position.y,camTransform.position.z);
		transform.LookAt(target);
		transform.Rotate(-90,0,0);
		//
	}

	private void OnTriggerEnter(Collider other){
		if(!Global.bearingSet){
			return;
		}
   	if(other.gameObject.CompareTag("Player")){
	   		if(!itemViewed){
				archiveViewer.setText(itemDescription);
				archiveViewer.setSprite(archiveItem);
				itemViewed=true;
				Handheld.Vibrate();
			}
    	}
    }
}