using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showClayArchive : MonoBehaviour {
	public  Sprite archiveItem;
	public string itemDescription;
	public static bool itemViewed=false;
	public static bool showItem=false;
	
	// Update is called once per frame
	void Update () {
		if(showItem && !itemViewed){
			//archiveViewer.setText(itemDescription);
			archiveViewer.setSprite(archiveItem);
			itemViewed=true;
			//Handheld.Vibrate();
		}
		else{
			return;
		}
		
	}
}
