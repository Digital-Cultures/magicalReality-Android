using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backgroundVideo : MonoBehaviour {
	public GameObject backgroundPanel;
	public GameObject foregroundPanel;
	public Sprite[] images;
	public float duration;
	int currentImage=1;
	Image bgImage;
	Image fgImage;
	Color tempColor;
	float rate;

	// Use this for initialization
	void Start () {
		bgImage=backgroundPanel.GetComponent<Image>();
		fgImage=foregroundPanel.GetComponent<Image>();
		bgImage.sprite=images[0];
		fgImage.sprite=images[1];
		tempColor=fgImage.color;
		tempColor.a=0;
		fgImage.color=tempColor;
		rate=1/duration;
	}

	void Update(){
		if(currentImage>=images.Length){
			currentImage=0;
		}
		tempColor=fgImage.color;
		tempColor.a+=rate*Time.deltaTime;
		if(tempColor.a>1){
			tempColor.a=0;
			bgImage.sprite=fgImage.sprite;
			fgImage.sprite=images[currentImage];
			currentImage++;
		}
		fgImage.color=tempColor;
		
		
	}

	
}
