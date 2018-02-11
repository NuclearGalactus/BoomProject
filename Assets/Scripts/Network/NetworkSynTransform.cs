using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkSynTransform : NetworkBehaviour {
	[SyncVar]
	private Vector3 netPos;
	[SyncVar]
	private Quaternion netRot;
	[SerializeField] float freq = 15;
	// Use this for initialization
	void Start () {
		if (isServer) {
			netPos = transform.position;
			netRot = transform.rotation;
		}
	}
	
	void FixedUpdate(){
		SendPos ();
		ReceivePos ();
	}

	void SendPos(){
		if(isServer){
			netPos = transform.position;
			netRot = transform.rotation;
		}
	}


	[ClientCallback]
	void ReceivePos(){
		transform.rotation = Quaternion.Lerp (transform.rotation, netRot, Time.deltaTime * freq);
		transform.position = Vector3.Lerp (transform.position, netPos, Time.deltaTime * freq);
	}
}
