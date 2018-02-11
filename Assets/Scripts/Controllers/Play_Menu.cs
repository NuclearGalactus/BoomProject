using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Play_Menu : MonoBehaviour {
	public Button playButton;
	public RectTransform nameALert;
	public Button change;
	public Text alertText;
	public Text displayName;
	// Use this for initialization
	void Start () {
		change.onClick.AddListener (changeName);
		playButton.onClick.AddListener (playButtonClick);
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetString ("MP_name") == "") {
			displayName.text = "Name not set";
		} else {
			displayName.text = "Current Name: " + PlayerPrefs.GetString ("MP_name");
		}
	}

	public void playButtonClick(){
		if (PlayerPrefs.GetString ("MP_name") == "") {
			nameALert.gameObject.SetActive (true);
		} else {
			Debug.Log (PlayerPrefs.GetString ("MP_name"));
			SceneManager.LoadScene ("TestPlace");
		}
	}

	public void changeName(){
		alertText.text = "";
		nameALert.gameObject.SetActive (true);
	}


}
