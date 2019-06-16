using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuActions : MonoBehaviour {
	public GameObject loadingAnimation;
	Image loadingImage;
	// Use this for initialization
	void Awake(){
		loadingImage=loadingAnimation.GetComponent<Image>();
		loadingImage.enabled=false;
	}
	public void launchMainScene () {
		loadingImage.enabled=true;
	   // Application.LoadLevel("main");
		StartCoroutine(asyncLoadScene());
	}

	public void openLink(){
		Application.OpenURL("https://magicalreality.ncl.ac.uk/");
	}

	IEnumerator asyncLoadScene(){
		AsyncOperation async= Application.LoadLevelAsync("main");
		while(!async.isDone){
			yield return null;
		}
	}

}
