using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelController : MonoBehaviour {
	public Text mainText;
	public Text timer;

	private PlayerController player;
	private BasicEnemy[] enemies;
	private float time = 0;
	private bool gameOver = false;
	// Use this for initialization
	void Start () {
		enemies = FindObjectsOfType<BasicEnemy> ();
		player = FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameOver && checkDead()) {
			gameOver = true;
			mainText.enabled = true;
		}

		if (!gameOver && !(player.HEALTH <= 0)) {
			time += Time.deltaTime;
			//timer.text = ((int)(time % 60)) + ":" + ((((time * 100) % 100) / 100));
			timer.text = "" + ((float)((int)(time * 100)) / 100f);
		}
	}

	private bool checkDead(){
		foreach (BasicEnemy enemy in enemies) {
			if (enemy.isAlive) {
				return false;
			}
		}
		return true;
	}
}
