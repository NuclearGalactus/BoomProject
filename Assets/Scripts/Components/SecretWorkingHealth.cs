using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SecretWorkingHealth : NetworkBehaviour {
	public Slider healthUI;
	public delegate void simple(Vector3 start, Vector3 dir);
	public simple onDeath;
    public string killer;
    Vector3 start;
    Vector3 direction;
	public bool isAlive = true;
    [SyncVar]
	public bool lastCrit;
	[SyncVar]
	public float health = 100;


	void Start(){
		if (healthUI != null) {
			healthUI.maxValue = health;
		}
	}

	public void heal(int amt){
		if (isServer && isAlive) {
			health += amt;
		}
	}

	public void dmg(float amt, bool isCrit, Vector3 point, Vector3 dir){
        start = point;
        direction = dir;
		if (isServer && isAlive) {
			health -= amt;
            RpcDisplayDamage(amt, isCrit, point);
		}
	}
	[ClientRpc]
    public void RpcDisplayDamage(float amt, bool crit, Vector3 point)
    {
        
        GameObject ind = (GameObject)Instantiate(Resources.Load("DamageIndicator"), point, Quaternion.identity);
        ind.GetComponentInChildren<Text>().text = amt.ToString();
        ind.GetComponent<DamageIndicator>().isCrit = crit;
        lastCrit = crit;
    }	

	void Update(){
		if (healthUI != null) {
			healthUI.value = health;
		}

		if (health <= 0 && isAlive) {
			Debug.Log ("Dead");
			//healthUI.gameObject.SetActive (false);
			onDeath (start, direction);
			isAlive = false;
		}
	}
		
}
