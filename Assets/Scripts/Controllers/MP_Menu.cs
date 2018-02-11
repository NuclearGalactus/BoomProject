using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class MP_Menu : MonoBehaviour {
    public Button host;
    public Button join;
    public RectTransform buttonParent;
    public RectTransform listParent;

	public bool autoHost = false;

    bool started = false;
	// Use this for initialization
	void Start () {
        host.onClick.AddListener(hostButton);
        join.onClick.AddListener(joinButton);
    }
	
	// Update is called once per frame
	void Update () {
        if (!started && NetworkController.singleton != null)
        {
            Debug.Log("Matchmaker Started");
            NetworkController.singleton.StartMatchMaker();
            started = true;
        }
		if (started && autoHost) {
			hostButton ();
			autoHost = false;
		}
	}

    public void hostButton()
    {
        NetworkController.singleton.matchMaker.CreateMatch(NetworkController.singleton.name + "'s Room", 4, true, "", "", "", 0, 1, OnMatchCreate);
        
    }

    public void OnMatchCreate(bool success, string info, MatchInfo Minfo)
    {
        if (success)
        {
            Debug.Log("Room successfully created, " + NetworkController.singleton.name + "'s Room");
            gameObject.SetActive(false);
            MatchInfo newInfo = Minfo;
            NetworkServer.Listen(newInfo, 9000);
            NetworkController.singleton.StartHost(newInfo);
        }else
        {
            NetworkController.singleton.sendAlert("The stuff doesnt work yet");
        }
    }

    public void joinButton()
    {

        NetworkController.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 1, OnListMatches);
    }

    public void OnListMatches(bool success, string info, List<MatchInfoSnapshot> matches)
    {
        if(matches.Count == 0)
        {
            NetworkController.singleton.sendAlert("No matches found");
            return;
        }
        buttonParent.gameObject.SetActive(false);
        listParent.gameObject.SetActive(true);
        //Debug.Log(matches[0] == null);
        GameObject lastobj = (GameObject)GameObject.Instantiate(Resources.Load("RoomButton"), listParent);
        lastobj.GetComponent<RoomButton>().room = matches[0];
        lastobj.GetComponent<RoomButton>().background = gameObject;
        for (int i = 1; i < matches.Count; i++)
        {
            //Debug.Log(matches[i].name);
            GameObject tempObj = lastobj;
            GameObject newObj = GameObject.Instantiate(tempObj, listParent);
            newObj.GetComponent<RoomButton>().room = matches[i];
            newObj.GetComponent<RectTransform>().position = new Vector2(newObj.GetComponent<RectTransform>().position.x, newObj.GetComponent<RectTransform>().position.y - newObj.GetComponent<RectTransform>().sizeDelta.y);
            newObj.GetComponent<RoomButton>().background = gameObject;
            lastobj = newObj;
        }
        
    }

}
