using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

public class RoomButton : MonoBehaviour {
    public MatchInfoSnapshot room;
    public GameObject background;
	// Use this for initialization
	void Start () {
        Debug.Log(room.name);
        GetComponent<Button>().onClick.AddListener(onRoomButton);
        GetComponentInChildren<Text>().text = room.name;
	}
	

    public void onRoomButton()
    {
        NetworkController.singleton.matchMaker.JoinMatch(room.networkId, "", "", "", 0, 1, onJoinRoom);
    }

    public void onJoinRoom(bool success, string info, MatchInfo match)
    {
        if (success)
        {
            background.SetActive(false);
            NetworkController.singleton.StartClient(match);
            Debug.Log("Successfully joined " + room.name);
        }else
        {
            Debug.Log("Room join failed");
            NetworkController.singleton.sendAlert("Room Join failed");
        }
    }
}
