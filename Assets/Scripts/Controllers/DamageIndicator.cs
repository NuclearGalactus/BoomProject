using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {
	float vspeed;
	float hspeed;
	public Color crit;
	public bool isCrit;
	// Use this for initialization
	void Start () {
		if (isCrit) {
			GetComponentInChildren<Text> ().color = crit;
		}
		vspeed = Random.Range(0,1f);
		hspeed = Random.Range(-1f,1f);
		GetComponent<Canvas> ().worldCamera = Camera.main;	
		GetComponentInChildren<Text> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * hspeed * Time.deltaTime;
		transform.position += transform.up * vspeed * Time.deltaTime;
		if (Player.singleton != null) {
			transform.LookAt (2 * transform.position - Player.singleton.cam.transform.position);
			GetComponentInChildren<Text> ().enabled = true;
		}
	}

	public void AnimDestroy(){
		Destroy (gameObject);
	}
}
