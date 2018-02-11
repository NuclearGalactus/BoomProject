using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTester : MonoBehaviour {
	public float vel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x >= 2) {
			transform.position = new Vector3 (-2, transform.position.y, transform.position.z);
		}
		transform.position = new Vector3 (transform.position.x + vel * Time.deltaTime, transform.position.y, transform.position.z);
	}
}
