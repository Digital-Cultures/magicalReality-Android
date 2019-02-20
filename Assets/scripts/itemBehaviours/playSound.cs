using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour {
	//public AudioClip soundClip;
	private AudioSource source;
	//Archive viewer
	public Sprite archiveItem;
	public string itemDescription;

	// Use this for initialization
	void Awake () {
		source=GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   private void OnTriggerEnter(Collider other){
   	if(!Global.bearingSet){
   		return;
   	}
   	if(other.gameObject.CompareTag("Player")){
    	if(source!=null && !source.isPlaying){
    		source.Play();
			archiveViewer.setText(itemDescription);
			archiveViewer.setSprite(archiveItem);
			Handheld.Vibrate();
    	}
    }

    }




}
