using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerTransformSync : NetworkBehaviour {
	public Transform head;
    public Transform spine;
	[SyncVar]
	private Vector3 netPos;
	[SyncVar]
	private Quaternion charRot;
	[SyncVar]
	private Quaternion headRot;
	[SerializeField] float freq = 15;
	// Use this for initialization
	void Start () {
        
	}

	void FixedUpdate(){
        
		SendPos ();
		LerpPos ();
	}

    void LateUpdate()
    {
           // spine.rotation = head.rotation;
        
    }

	void LerpPos(){
		if (!isLocalPlayer) {
			transform.rotation = Quaternion.Lerp (transform.rotation, charRot, Time.deltaTime * freq);
			head.rotation = Quaternion.Lerp (head.rotation, headRot, Time.deltaTime * freq);
			transform.position = Vector3.Lerp (transform.position, netPos, Time.deltaTime * freq);
		}
	}
	[Command]
	void CmdSendDataToServer(Vector3 pos, Quaternion rot, Quaternion hrot){
		netPos = pos;
		headRot = hrot;
		charRot = rot;
	}
	[ClientCallback]
	void SendPos(){
		if (isLocalPlayer) {
			CmdSendDataToServer (transform.position, transform.rotation, head.rotation);
		}
	}
}

