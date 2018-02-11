using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wacky : MonoBehaviour {
	public Transform one;
	public Transform two;
	public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		one.Rotate(new Vector3(Time.deltaTime * speed, Time.deltaTime * speed, 0));
		two.Rotate (new Vector3 (0, -Time.deltaTime * speed, -Time.deltaTime * speed));
	}
}
