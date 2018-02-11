using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnEff : NetworkBehaviour {
	public float spawnTime = 1f;
	bool spawning = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
        {
            return;
        }
		spawnTime -= Time.deltaTime;
		if (spawnTime <= 0 && !spawning) {
			spawning = true;
			Vector3 pos = transform.position;
			pos.y = 0;
			NetworkServer.Spawn ((GameObject)GameObject.Instantiate(Resources.Load ("Grunt"), pos, Quaternion.identity));
		}
		if (spawnTime <= -1f) {
			NetworkServer.Destroy (this.gameObject);
		}
	}

	
}
