using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class tutorialNav : MonoBehaviour {

	public Sprite[] pages;
	public Sprite closeIcon;
	public GameObject textContainer;
	public GameObject arrowMessage;
	public Button mButton;
	private Button btn;
	private int currentPage=0;
	private Image pageImage;
	private bool destroy=false;
	public GameObject panel;

	void Awake(){
		if(PlayerPrefs.GetInt("sampleShown")==1){
			Global.sampleShown=true;
			}
		if(PlayerPrefs.GetInt("tutorialComplete")==1){
			Global.tutorialComplete=true;
			Destroy(panel);
			Destroy(arrowMessage);
			return;
		}
		pageImage = textContainer.GetComponent<Image>();
		pageImage.sprite=pages[currentPage];
		btn=mButton.GetComponent<Button>();
		arrowMessage.SetActive(false);

	}

	public void nextPage(){
		currentPage++;
		if(currentPage<pages.Length){
			pageImage.sprite=pages[currentPage];
			if(currentPage==1){
				arrowMessage.SetActive(true);
			}
			else{
				arrowMessage.SetActive(false);
			}
			if(currentPage==pages.Length-1){
				btn.image.sprite=closeIcon;
			}
		}else{
			Global.tutorialComplete=true;
			if(PlayerPrefs.GetInt("tutorialComplete")==0){
				PlayerPrefs.SetInt("tutorialComplete",1);
			}
			destroy=true;
		}

		if(destroy){
			Destroy(panel);
		}
		

	}
}
