using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class poi : MonoBehaviour {
	public GameObject uiContainer;
	public GameObject marker;
	public Sprite visitedIcon;
    public Global.Effect effect = Global.Effect.None;
    public int id;
    public string popupText = "";

    public float compassWidth = 975; 
	private GameObject markerGO;
	private Vector3 startPosition;
	private Camera cam;
	public float transparentDistance;
	public float solidDistance = 5;
	private float playerDistance=100;
	private bool visited=false;
    private bool itemViewed = false;

    private float alpha = 0f;

    // Use this for initialization
    void Start () {
        Debug.Log("------------------Start POI ");
        cam =Camera.main;
		markerGO = Instantiate(marker) as GameObject;
		markerGO.transform.SetParent(uiContainer.transform, false);
		startPosition = new Vector3(0,0,0);
		markerGO.transform.localPosition=startPosition;
		markerGO.SetActive(false);
	}

    public void SetUiContainert(GameObject uiContainerMask)
    {
        uiContainer = uiContainerMask;

    }

    public void SetEffect(Global.Effect chossenEffect)
    {
        effect = chossenEffect;
    }

    public void SetID(int ID)
    {
        id = ID;
    }
    public void SetPopupText(string txt)
    {
        popupText = txt;
    }

    // Update is called once per frame
    void Update () {
		if(Global.bearingSet){
			if(!markerGO.activeSelf){
				markerGO.SetActive(true);
			}
			markerGO.transform.localPosition = MarkerPosition();


            if (Time.frameCount % Global.opacityInterval == 1){
				playerDistance=distanceFromPlayer();


                if (playerDistance<solidDistance/2 ){
					if(!visited){
						visited=true;
						Image img=markerGO.GetComponent<Image>();
						img.sprite=visitedIcon;

                    }
				}
				setOpacity();
                //Debug.Log("------------------DISTANCE 1: " + playerDistance +"  "+ transform.name);
            }

			//Debug.Log("pos  "+markerGO.transform.localPosition);
		}else if(CompareTag("headingTrig")){
            // it is a cube

			if(!markerGO.activeSelf){
				markerGO.SetActive(true);
			}
			markerGO.transform.localPosition = MarkerPosition();
			if (Time.frameCount % Global.opacityInterval == 1){
				playerDistance=distanceFromPlayer();
				setOpacity();
               // Debug.Log("------------------DISTANCE 2: " + playerDistance + "  " + transform.name);
            }
		}else{
			playerDistance=distanceFromPlayer();
            //Debug.Log("------------------DISTANCE 3: " + playerDistance + "  " + transform.name);
        }


        if (Time.frameCount % 30 == 0)
        {
            Debug.Log("--------ObjectPlacement----------: " + Global.originSet + "  ID:" + name + gameObject.GetInstanceID() + "  " + playerDistance + "  " + transparentDistance + "  " + solidDistance + "  " + alpha + "  " + id);
        }


        //set effect if distance is less tham 2 ( this could go in the collider below)
        if (playerDistance < 2){
            if (Global.EffectsApllied.ContainsKey(name + gameObject.GetInstanceID())){
                Global.EffectsApllied[name + gameObject.GetInstanceID()] = effect;
            }else{
                Global.EffectsApllied.Add(name + gameObject.GetInstanceID(), effect);
            }
        }else{
             if (Global.EffectsApllied.ContainsKey(name + gameObject.GetInstanceID())){
                Global.EffectsApllied[name + gameObject.GetInstanceID()] = Global.Effect.None;
            }else{
                Global.EffectsApllied.Add(name + gameObject.GetInstanceID(), Global.Effect.None);
            }
        }

    }

	Vector3 MarkerPosition(){
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
 //       Debug.Log("------------------markerPosition : " + outvector + "  " + transform.name);
        return outvector;
	}

	float distanceFromPlayer(){
		Vector3 playerPos=cam.transform.position;
		float distance = Conversions.xzDistance(playerPos,transform.position);
		// Mathf.Sqrt(Mathf.Pow(playerPos.x-transform.position.x, 2)
		//		+Mathf.Pow(playerPos.z-transform.position.z, 2));
		if(Global.playerDistance.ContainsKey(name + gameObject.GetInstanceID())){
			Global.playerDistance[name + gameObject.GetInstanceID()] =distance;
		}
		else{
			Global.playerDistance.Add(name + gameObject.GetInstanceID(), distance);
		}
		return distance;
	}

	void setOpacity(){
		Image markerImg=markerGO.GetComponent<Image>();
		double gradient=-1.0f/(transparentDistance-solidDistance);
		double displace=-(gradient*transparentDistance);
		alpha=(float)gradient*playerDistance+(float)displace;
       // Debug.Log("------------------alpha: " + alpha);
        if (alpha<0){
    		alpha=1.0f;
    	}else if(alpha>1){
	    	alpha=1.0f;
    	}
		Color col =markerImg.color;
		col.a=alpha;
		markerImg.color=col;
	}

	public void destroyMarker(){
		Destroy(markerGO);
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("------------------TRY EFFECT : " + effect + "   "+ Global.bearingSet +"   "+ other.gameObject.tag + "   " + other.gameObject.name); 

        if (!Global.bearingSet)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("------------------EFFECT : " + effect);
            Global.chosenEffect = effect;
            if (!itemViewed)
            {
                archiveViewer.setText(popupText);
                //archiveViewer.setSprite(popupText);
                itemViewed = true;
                Handheld.Vibrate();
            }
        }
    }
}
