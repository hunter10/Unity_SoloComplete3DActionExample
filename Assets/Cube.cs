using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody>().velocity = new Vector3(10f, 0, 0); 

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
