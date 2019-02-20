using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnCompass : MonoBehaviour {

    public float compassWidth;
    public GameObject target;
    Vector3 startPosition;
    float rationAngleToPixel;
    Camera mainCamera;
 
    void Start()
    {
        startPosition = transform.position;
        rationAngleToPixel = compassWidth / 360f;
        mainCamera=Camera.main;
    }
 
    void Update ()
    {
        Vector3 perp = Vector3.Cross(target.transform.forward, mainCamera.transform.forward);
        float dir = Vector3.Dot(perp, Vector3.up);
        Vector2 camXZ=new Vector2(mainCamera.transform.forward.x,mainCamera.transform.forward.z);
        Vector2 targetXZ=new Vector2(target.transform.forward.x,target.transform.forward.z);
        transform.position = startPosition + (new Vector3(Vector2.Angle(camXZ, targetXZ) * -Mathf.Sign(dir) * rationAngleToPixel, 0, 0));
        //transform.position = startPosition + (new Vector3(Vector3.Angle(Camera.main.transform.forward, target.transform.forward) * -Mathf.Sign(dir) * rationAngleToPixel, 0, 0));
    }
}
