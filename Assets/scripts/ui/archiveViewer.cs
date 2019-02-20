using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class archiveViewer : MonoBehaviour {

	public GameObject descriptionTextObject;
	TextMeshProUGUI itemText;
	public GameObject archiveContainer;
	private Image archiveImage;
	public GameObject panel;
	public static archiveViewer Instance;
	public static bool viewerVisible=false;

	void Awake(){
		Instance=this;
		archiveImage = archiveContainer.GetComponent<Image>();
		itemText=descriptionTextObject.GetComponent<TextMeshProUGUI>();
		hideViewer();

	}

	public static void setSprite(Sprite item){
		Instance.archiveImage.sprite=item;
		showViewer();
	}

	public static void showViewer(){
		Instance.panel.SetActive(true);
		viewerVisible=true;
	}

	public static void hideViewer(){
		Instance.panel.SetActive(false);
		viewerVisible=false;
	}

	public static void setText(string text){
		Instance.itemText.SetText(text);
	}

}
