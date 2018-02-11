using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testerAI : MonoBehaviour {
    Animator main;
	// Use this for initialization
	void Start () {
        main = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        main.SetBool("Running", false);
	}
}
