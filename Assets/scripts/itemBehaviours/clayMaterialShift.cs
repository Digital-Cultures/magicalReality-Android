using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clayMaterialShift : MonoBehaviour {
	private Renderer rend;
	public bool animate=false;
	public float increment=1;
	private float timer=0;
	float gloss=1;
	float clayAmount=0;
	Color color= new Color(1,1,1);
	string pName;
	Camera mainCam;
	Rigidbody rigidBody;
	bool rigidKinematic;

	// Use this for initialization
	void Start () {
		rend=GetComponent<Renderer>();
		rigidBody=GetComponent<Rigidbody>();
		mainCam=Camera.main;
		if(rend!=null){
			rend.material.SetFloat("_Glossiness",1);
			rend.material.SetFloat("_ClayAmount",0);
			rend.material.color=color;
		}
		rigidKinematic=true;
		if(rigidBody!=null){
			rigidBody.isKinematic=true;
		}
		pName=transform.parent.name;
		
	}
	
	// Update is called once per frame
	void Update () {
		float distance;
		if(Global.playerDistance.TryGetValue(pName, out distance)){
			if(distance<4 && isInFrame()){
				animate=true;
			}else{
				animate=false;
				rigidKinematic=true;
			}
		}
		if(rend!=null){
			if(animate){
				//Handheld.Vibrate();
				if(timer>3&&timer<=6){
					color=Color.Lerp(new Color(1f,1f,1f), new Color(0.5f,0.412f,0.275f), (timer-3)/3);
					rend.material.color=color;
				}
				if(timer>5&&timer<=8){
					clayAmount=Mathf.Lerp(0, 1, (timer-5)/3);
					rend.material.SetFloat("_ClayAmount",clayAmount);
				}
				if(timer>11&&timer<=14){
					gloss=Mathf.Lerp(1, 0, (timer-11)/3);
					rend.material.SetFloat("_Glossiness",gloss);
				}

				if(timer>14&&timer<20){
					rigidKinematic=false;
				}
				if(timer>23 && timer<23.3){
					animate=false;
					rigidKinematic=true;
					
				}
				if(timer>23.3 && !showClayArchive.showItem){
					showClayArchive.showItem=true;
				}
				if(rigidBody!=null){
					rigidBody.isKinematic=rigidKinematic;
				}
				timer+=increment*Time.deltaTime;
			}
		}
	}

	bool isInFrame(){
		Vector3 viewPos=mainCam.WorldToViewportPoint(transform.parent.position);
		if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0){
			return true;
		}
	    else
	        return false;
	}
}
	