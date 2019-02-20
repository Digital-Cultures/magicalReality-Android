using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonListener : MonoBehaviour {
public Button m_button0, m_button1, m_button2,m_button3,m_button4,m_button5;
public Sprite[] set0;
public Sprite[] set1;
public Sprite[] set2;
public Sprite[] set3;

    Button btn0;
    Button btn1;
    Button btn2;
    Button btn3;
    Button btn4;
    Button btn5;

    void Start()
    {
        btn0 = m_button0.GetComponent<Button>();
        btn1 = m_button1.GetComponent<Button>();
        btn2 = m_button2.GetComponent<Button>();
        btn3 = m_button3.GetComponent<Button>();
        btn4 = m_button4.GetComponent<Button>();
        btn5 = m_button5.GetComponent<Button>();
        //Calls the TaskOnClick/TaskWithParameters method when you click the Button
        btn0.onClick.AddListener(delegate {TaskWithParameters(0); });
        btn1.onClick.AddListener(delegate {TaskWithParameters(1); });
        btn2.onClick.AddListener(delegate {TaskWithParameters(2); });
        btn3.onClick.AddListener(delegate {TaskWithParameters(3); });
        btn4.onClick.AddListener(delegate {TaskWithParameters(4); });
        btn5.onClick.AddListener(delegate {TaskWithParameters(5); });

    }

    void Update(){
    
        switch(Global.ActiveSet){
            case 0:
                /*
                btn3.enabled=false;
                btn3.gameObject.SetActive(false);
                btn4.enabled=false;
                btn4.gameObject.SetActive(false);
                btn0.image.sprite=set0[0];
                btn1.image.sprite=set0[1];
                btn2.image.sprite=set0[2];
                break;
                */
            case 1:
            /*
                btn3.enabled=false;
                btn3.gameObject.SetActive(false);
                btn4.enabled=false;
                btn4.gameObject.SetActive(false);
                btn0.image.sprite=set1[0];
                btn1.image.sprite=set1[1];
                btn2.image.sprite=set1[2];
                break;
                */
            case 2:
            /*
                btn3.enabled=false;
                btn3.gameObject.SetActive(false);
                btn4.enabled=false;
                btn4.gameObject.SetActive(false);
                btn0.image.sprite=set2[0];
                btn1.image.sprite=set2[1];
                btn2.image.sprite=set2[2];
                */
                this.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case 3:
                if(!this.transform.GetChild(0).gameObject.activeSelf){
                    this.transform.GetChild(0).gameObject.SetActive(true);
                }
                if(!btn3.enabled){
                    btn3.enabled=true;
                    btn3.gameObject.SetActive(true);
                }
                if(!btn4.enabled){
                    btn4.enabled=true;
                    btn4.gameObject.SetActive(true);
                }
                if(!btn5.enabled){
                    btn5.enabled=true;
                    btn5.gameObject.SetActive(true);
                }
                btn0.image.sprite=set3[0];
                btn1.image.sprite=set3[1];
                btn2.image.sprite=set3[2];
                btn3.image.sprite=set3[3];
                btn4.image.sprite=set3[4];
                btn5.image.sprite=set3[5];
                break;

            default:
                if(!this.transform.GetChild(0).gameObject.activeSelf){
                    this.transform.GetChild(0).gameObject.SetActive(true);
                }
                if(!btn3.enabled){
                    btn3.enabled=true;
                    btn3.gameObject.SetActive(true);
                }
                if(!btn4.enabled){
                    btn4.enabled=true;
                    btn4.gameObject.SetActive(true);
                }
                if(!btn5.enabled){
                    btn5.enabled=true;
                    btn5.gameObject.SetActive(true);
                }
                btn0.image.sprite=set3[0];
                btn1.image.sprite=set3[1];
                btn2.image.sprite=set3[2];
                btn3.image.sprite=set3[3];
                btn4.image.sprite=set3[4];
                btn5.image.sprite=set3[5];
                break;
        }
    }

    void TaskWithParameters(int activeModel)
    {
        //Output this to console when the Button is clicked
        Global.activeModel=activeModel;
        Global.showGrid=true;
        switch (activeModel){
            case 0:
                btn0.Select();
                break;
            case 1:
                btn1.Select();
                break;
            case 2:
                btn2.Select();
                break;
            case 3:
                btn3.Select();
                break;
            case 4:
                btn4.Select();
                break;
            case 5:
                btn5.Select();
                break;
            default:
                btn0.Select();
                break;

        }
    }
}
