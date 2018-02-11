using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RespawnButton : NetworkBehaviour {
	public GameObject deadUI;
	public Button button;

	NetworkController controller;
	void Start(){
		controller = NetworkController.singleton;
	button.onClick.AddListener (clicked);
	}

	void clicked(){
		CmdRespawnPlayer ();
		controller.lobbyCam.enabled = false;
		deadUI.SetActive (false);
	}

	[Command]
	void CmdRespawnPlayer(){
		GameObject player = (GameObject)Instantiate (controller.playerPrefab, controller.GetStartPosition ().position, controller.GetStartPosition ().rotation);
		NetworkServer.ReplacePlayerForConnection (this.connectionToClient, player, this.playerControllerId);
	}
}
