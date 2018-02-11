using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyHandoff : MonoBehaviour {
	private BasicEnemy main;

	public void Start(){
		main = GetComponentInParent<BasicEnemy> ();
	}


	public void FullAggro(){
	//	main.readyToFire = true;
	
	}
}
