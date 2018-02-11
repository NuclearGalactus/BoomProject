using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimHandoff : MonoBehaviour {
    GunController handler;
    private void Start()
    {
        handler = GetComponentInParent<GunController>();
    }


    void ReadyUp(){
		handler.ReadyUp ();
	}

	void Reload(){
		handler.Reload ();
	}

	void CheckZoom(){
		handler.CheckZoom ();
	}

	void ActivateMuzzleFlash(){
		handler.ActivateMuzzleFlash ();
	}

}
