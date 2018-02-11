using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class NetworkController : NetworkManager{
    public new static NetworkController singleton;
    public Transform spawn;
    public Camera lobbyCam;
	public GameObject deadUI;
    public GameObject godObject;
	public GameObject ghost;
    public Animator UI_alert;
	public Transform spawnLoc;
	public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    public string playerName; 

    public NetworkWrapper wrapper;

    void Start(){
        if (Application.isEditor)
        {
            name = "DEV_";
        }else
        {
            name = PlayerPrefs.GetString("MP_name"); 
        }
		wrapper = GetComponent<NetworkWrapper> ();
		singleton = this;
	}
	public override void OnClientConnect(NetworkConnection conn){
		base.OnClientConnect (conn);
        Debug.Log("connected");
		lobbyCam.enabled = false;
	}
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Debug.Log("client did a thing");
        var player = (GameObject)GameObject.Instantiate(playerPrefab, spawn.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    public void sendAlert(string msg)
    {
        UI_alert.GetComponentInChildren<Text>().text = msg;
        UI_alert.SetTrigger("Alert");
    }
    public override void OnStopHost()
    {
        base.OnStopHost();
        Debug.Log("host stopped");
        matchMaker.DestroyMatch(matchInfo.networkId, 1, null);
    }

    public void onDestroyMatch(bool success, string info, MatchInfo data)
    {

    }


    [RPC]
    public void RpcSendMsgToAll(string msg)
    {
        Debug.Log(msg);
        sendAlert(msg);
    }
}
