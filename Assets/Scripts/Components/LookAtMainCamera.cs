using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.singleton != null)
        {
            transform.LookAt(2 * transform.position - Player.singleton.cam.transform.position);
        }
    }
}
