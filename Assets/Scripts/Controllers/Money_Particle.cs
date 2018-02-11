using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money_Particle : MonoBehaviour {
    public float timeUntilGo = 3f;
    float speed = 3f;

    Rigidbody body;
    Money_Particle_Parent parent;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<Money_Particle_Parent>();
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        timeUntilGo -= Time.deltaTime;
        if(timeUntilGo < 0)
        {
            body.velocity += (parent.killer.position - transform.position).normalized * speed;
        }
	}
}
