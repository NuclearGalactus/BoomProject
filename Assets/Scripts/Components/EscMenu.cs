using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscMenu : MonoBehaviour {

	public Animation optionsup;
    public Button escapeButton;
	private GameObject button;
	private GameObject Menu;
	private GameObject player;
	private GameObject vignette;
	public int status = 0;

	private bool woke = false;

	public void GetWoke()
	{
        escapeButton.onClick.AddListener(escape);
		player = Player.singleton.gameObject;
		button = GameObject.FindGameObjectWithTag ("Escape Button");
		Menu = GameObject.FindGameObjectWithTag ("Menu UI");
		Menu.GetComponent<Animator>().SetBool ("Trans", false);
		button.GetComponent<Button> ().onClick.AddListener (closemenu);

	}

	void Update()
	{
		if (!woke && Player.singleton != null) {
			GetWoke ();
			woke = true;
		}

		if (Input.GetButtonDown ("Esc Menu") && Player.singleton != null) {
			status += 1;
			openmenu ();
		}

		if (Input.GetButtonDown ("Esc Menu") && GetComponent<Animator> ().GetBool ("Settings")) {
			status = 2;
			openmenu ();
		}
	}

	void openmenu()
	{
		if (status == 1){
			Menu.GetComponent<Animator> ().SetBool ("Trans", true);
			expandmenu();
		}
		else if (status == 2)
		{
			closemenu ();
		}
	}

	void closemenu()
	{
		Menu.GetComponent<Animator> ().SetBool ("Settings", false);
		Menu.GetComponent<Animator> ().SetBool ("Trans", false);
		player.GetComponent<LookController> ().enabled = true;
		player.GetComponent<GunHandler> ().enabled = true;
		player.GetComponent<Animator> ().enabled = true;
		player.GetComponent<PlayerMovement> ().isPaused = false;
		player.GetComponentInChildren<Headbob> ().enabled = true;
		status = 0;
	}

	void expandmenu()
	{
		player.GetComponent<LookController> ().enabled = false;
		player.GetComponent<GunHandler> ().enabled = false;
		player.GetComponent<Animator> ().enabled = false;
		player.GetComponent<PlayerMovement> ().isPaused = true;
		player.GetComponentInChildren<Headbob> ().enabled = false;
	}

	public void resume()
	{
		Menu.GetComponent<Animator> ().SetBool ("Trans", false);

	}

	public void options()
	{
		Menu = GameObject.FindGameObjectWithTag ("Menu UI");
		Menu.GetComponent<Animator>().SetBool("Settings",true);
	}

    public void escape()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
        }else
        {
          //  UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}