using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCube : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(25,30,45) * Time.deltaTime );
		transform.position=new Vector3(transform.position.x,Camera.main.transform.position.y,transform.position.z);
	}
}
