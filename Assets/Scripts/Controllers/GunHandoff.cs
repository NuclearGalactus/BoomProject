using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandoff : MonoBehaviour {
	private GunController guncont;
	// Use this for initialization
	void Start () {
		guncont = GetComponentInParent<GunController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReadyUp(){
		guncont.ReadyUp ();
	}

	public void playActionSound(){
        guncont.playAudio(guncont.getGun().action);
    }

	public void playBoltOpen(){
        guncont.playAudio(guncont.getGun().boltOpen);
    }

	public void playBoltClose(){
        guncont.playAudio(guncont.getGun().boltClose);
    }

	public void AddBullet(){
		guncont.addBullet ();
        guncont.playAudio(guncont.getGun().singleBullet);
    }
}
