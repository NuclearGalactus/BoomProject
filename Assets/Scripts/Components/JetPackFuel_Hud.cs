using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class JetPackFuel_Hud : NetworkBehaviour {
    public Image circle;
    public RectTransform pointer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


	}

    public void setColor(Vector3 vect)
    {
        circle.color = new Color(vect.x, vect.y, vect.z);
    }


    public void setFuel(float ratio)
    {
        circle.fillAmount = ratio;
        if(ratio == 1)
        {
            pointer.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return;
        }
        pointer.localRotation = Quaternion.Euler(new Vector3(0,0,ratio * 360f));
    }
}
