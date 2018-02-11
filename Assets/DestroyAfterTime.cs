using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public float timeUntilDeath = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeUntilDeath -= Time.deltaTime;
        if(timeUntilDeath <= 0)
        {
            Destroy(gameObject);
        }
	}
}
