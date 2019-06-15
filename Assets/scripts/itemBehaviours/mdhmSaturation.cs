using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class mdhmSaturation : MonoBehaviour {
	public Material saturationMaterial;
	public float bwDistance = 4;
	public float colorDistance = 10;
    public float effect = 0;
    private Camera mainCam;
	private float gradient;
	private float displace;
	private Desaturate saturationScript;
   // private PostProcessLayer postProcessLayer;
    //public Text infoUI;

    public Sprite archiveItem;
	bool itemViewed=false;
	public string itemDescription;
	// Use this for initialization
	void Start () {
		mainCam=Camera.main;
		saturationScript=mainCam.GetComponent<Desaturate>();
       // colourScript = mainCam.GetComponent<pro>();
        saturationScript.enabled=false;
		gradient=-1/(colorDistance-bwDistance);
		displace=-(gradient*colorDistance);
		
	}
	
	// Update is called once per frame
	void Update () {
		float distance=distanceFromPlayer();
		
		float saturation=(gradient*distance)+displace;
		//infoUI.text="Distance: "+distance+" Sat: "+saturation;
		if(distance<colorDistance && Global.bearingSet){
			if(!saturationScript.enabled){
				saturationScript.enabled=true;	
			}
			if(saturation<0){
				saturation=0;
			}
			else if(saturation>1){
				saturation=1;
			}

			if(distance<bwDistance && !itemViewed){
				archiveViewer.setText(itemDescription);
				archiveViewer.setSprite(archiveItem);
				itemViewed=true;
				Handheld.Vibrate();
			}

		}else{
			saturationScript.enabled=false;
		}
        Debug.Log("SATURATION: = " + saturation +"  ("+ gradient +" * "+ distance+") + "+displace);
		saturationMaterial.SetFloat("_Desaturation",saturation);
	}

	float distanceFromPlayer(){
		float distance;
		Vector3 playerPos=mainCam.transform.position;
		if(Global.playerDistance.TryGetValue(name+gameObject.GetInstanceID(), out distance)){
			return distance;
		}
		else{
			distance = Conversions.xzDistance(playerPos,transform.position);
			// Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
			// 	+Mathf.Pow(playerPos.z-transform.position.z, 2));
			Global.playerDistance.Add(name + gameObject.GetInstanceID(), distance);
		}
		return distance;
	}
}