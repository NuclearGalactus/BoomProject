using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTimer : MonoBehaviour {
    public float timeUntilEnd = 0.1f;
    public float timeUntilDestroy = 1.3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeUntilDestroy -= Time.deltaTime;
        timeUntilEnd -= Time.deltaTime;
        if(timeUntilDestroy <= 0)
        {
            Destroy(gameObject);
        }
        if (timeUntilEnd <= 0)
        {
			GetComponentInChildren<ParticleSystem> ().Stop ();
        }
    }
}
