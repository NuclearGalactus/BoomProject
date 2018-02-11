using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassjouCurve : MonoBehaviour {
	public float ampX;
	public float ampY;
	public float ampZ;
	public float freqX;
	public float freqY;
	public float freqZ;
	public float offsetX;
	public float offsetZ;

	private Vector3 initialPos;
	// Use this for initialization
	void Start () {
		initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = initialPos + new Vector3 ((ampX * Mathf.Sin ((freqX * Time.time) + offsetX)), (ampY * Mathf.Sin ((freqY * Time.time))), (ampZ * Mathf.Sin ((freqZ * Time.time) + offsetZ)));
	}
}
