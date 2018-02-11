using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSetter : MonoBehaviour {
	public InputField input;
	public Button submitButton;
	public Text fieldText;
	// Use this for initialization
	void Start () {
		submitButton.onClick.AddListener (submit);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void submit(){
		if (input.text == "") {
			fieldText.text = "Please put an actual name";
		} else {
			PlayerPrefs.SetString ("MP_name", input.text);
			gameObject.SetActive (false);
		}
	}
}
