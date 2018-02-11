using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class DJ : NetworkBehaviour {
    [SyncVar]
    public float syncedTime;
    AudioSource main;
    public bool playing = false;
    void Start()
    {
        main = GetComponent<AudioSource>();
        if (!isServer)
        {
            main.Play();
            main.time = syncedTime;
        }
    }

	void Update () {
        if (isServer)
        {
            if(Player.singleton != null && !playing)
            {
                main.Play();
                playing = true;
            }
            syncedTime = main.time;
        }
	}
}
