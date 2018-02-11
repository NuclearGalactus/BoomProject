using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FloatingCube : NetworkBehaviour {
    [SyncVar]
    Vector3 netPos;
    // Use this for initialization
    void Start() {
        if (!isServer)
        {
            transform.position = netPos;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void FixedUpdate()
    {
        if (isServer) {
            transform.position = transform.position + (new Vector3(Mathf.Sin(Time.realtimeSinceStartup), .2f, Mathf.Cos(Time.realtimeSinceStartup)) * Time.deltaTime);
            netPos = transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, netPos, Time.deltaTime * 15);     
        }
        
    }
}
