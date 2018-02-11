using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : MonoBehaviour {
    public Text clipText;
    public Text totalText;

    private int clipAmmo;
    private int totalAmmo;


    public void setClipAmmo(int val)
    {
        clipAmmo = val;
    }

    public void setTotalAmmo(int val)
    {
        totalAmmo = val;
    }


    void Update()
    {
        clipText.text = clipAmmo.ToString();
        totalText.text = totalAmmo.ToString();
    }
}
