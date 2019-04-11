using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlacementTutorial : MonoBehaviour
{

	public Sprite[] pages;
	public Sprite closeIcon;
	public GameObject textContainer;
	public Button mButton;
	private Button btn;
	private int currentPage=0;
	private Image pageImage;
	private bool destroy=false;
	public GameObject panel;

	void Awake(){
		if(PlayerPrefs.GetInt("placementTutorial")==1){
			Global.placeTutorialComplete=true;
			Destroy(panel);
			return;
        }

        pageImage = textContainer.GetComponent<Image>();
        pageImage.sprite = pages[currentPage];
		btn=mButton.GetComponent<Button>();
		panel.SetActive(false);

	}

	void Update(){
		if(Global.ActiveSet<3){
			return;
		}
		if(Global.bearingSet && !Global.placeTutorialComplete){
			panel.SetActive(true);
		}
		else{
			return;
		}
	}

	public void nextPage(){
		currentPage++;
		if(currentPage<pages.Length){
			pageImage.sprite=pages[currentPage];
			if(currentPage==pages.Length-1){
				btn.image.sprite=closeIcon;
			}
		}else{
			Global.placeTutorialComplete=true;
			if(PlayerPrefs.GetInt("placementTutorial")==0){
				PlayerPrefs.SetInt("placementTutorial",1);
			}
			destroy=true;
		}

		if(destroy){
			Destroy(panel);
		}

	}
}
