using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automatic_Rotate : MonoBehaviour {
	public bool x_axis;
	public bool y_axis;
	public bool z_axis;
	public float speed = 1f;
	Vector3 rotation;
	// Use this for initialization
	void Start () {
		rotation = Vector3.zero;
		if (x_axis) {
			rotation += new Vector3(speed, 0, 0);
		}
		if (y_axis) {
			rotation += new Vector3(0, speed, 0);
		}
		if (z_axis) {
			rotation += new Vector3(0, 0, speed);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(rotation * Time.deltaTime);
	}
}
