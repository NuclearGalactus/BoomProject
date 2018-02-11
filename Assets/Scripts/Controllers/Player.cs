using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {
	public static Player singleton;
	public Camera cam;
	public Camera uicam;
	public Camera viewmodelcam;
	public GameObject head;
	public GameObject external;
    public Text namePlate;


    [SyncVar]
    public int money;
	[SyncVar]
	public string localName;


    public AudioListener audiolist;
	GameObject DeadUI;
	public MonoBehaviour[] clientActivators;


	NetworkController controller;

    Text MoneyCounter;

	[Command]
	public void CmdSendPlayerData(string name){
		localName = name;
		controller.players.Add (name, gameObject);
		RpcAnnouncePlayerJoin (name);
	}

	[ClientRpc]
	public void RpcAnnouncePlayerJoin(string name){
        localName = name;
		Debug.Log (name + " has joined the game.");
	}

    public void addMoney(int amt)
    {
        if (isServer)
        {
            money += amt;
        }
    }



    public void takeMoney(int amt)
    {
        if (isServer)
        {
            money -= amt;
        }
    }
	void Start(){
        MoneyCounter = GameObject.FindGameObjectWithTag("MoneyCounter").GetComponent<Text>();
        MoneyCounter.text = money.ToString();
		cam.enabled = false;
		DeadUI = NetworkController.singleton.deadUI;
		controller = NetworkController.singleton;
		string tempname;
		if (isLocalPlayer) {
            namePlate.enabled = false;
			if (!Application.isEditor) {
				tempname = PlayerPrefs.GetString ("MP_name");
			} else {
				int id = 0;
				string name = "DEV_";
				while (controller.players.ContainsKey (name + id)) {
					id++;
				}
				tempname = name + id;
			}
			CmdSendPlayerData(tempname);
			cam.enabled = true;
			uicam.enabled = true;
			singleton = this;
			foreach (MeshRenderer mesh in external.GetComponentsInChildren<MeshRenderer>()) {
				mesh.enabled = false;
			}
			StartUpOnClient ();
			audiolist.enabled = true;
		} else {

            GameObject.Destroy(GetComponent<GunHandler>().GUN);
		}
        namePlate.text = localName;
        gameObject.name = localName;
		if (isLocalPlayer) {
			gameObject.name += " (local)";
		}
	}

	public void postDead(){
		DeadUI.SetActive (false);
		GetComponent<LookController> ().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    void Update()
    {
        
        MoneyCounter.text = money.ToString();
        if(gameObject.name != localName)
        {
            namePlate.text = localName;
            gameObject.name = localName;
        }
    }
	public void Die(){
		if (isLocalPlayer) {
			singleton = null;
			DeadUI.SetActive (true);
			controller.lobbyCam.enabled = true;
			CmdDestroyPlayer ();
		}
	}
		

	public void StartUpOnClient(){
		cam.enabled = true;
		foreach (MonoBehaviour go in clientActivators) {
			go.enabled = true;
		}
	}
	[Command]
	void CmdDestroyPlayer(){
		NetworkServer.ReplacePlayerForConnection (this.connectionToClient, controller.ghost, this.playerControllerId);
		NetworkServer.Destroy (this.gameObject);
	}
}
