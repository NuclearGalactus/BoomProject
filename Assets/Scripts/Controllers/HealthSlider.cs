using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour {
	public GameObject fillAmt;
	// Use this for initialization
	void Start () {
		GetComponent<Slider> ().onValueChanged.AddListener (onvalChanged);
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Slider> ().value == 0) {
			fillAmt.GetComponent<Image> ().enabled = false;
		}
	}

	public void onvalChanged(float val){
		if (GetComponent<Slider> ().value == 0) {
			fillAmt.GetComponent<Image> ().enabled = false;
		} else {
			fillAmt.GetComponent<Image> ().enabled = true;
		}
	}
}
