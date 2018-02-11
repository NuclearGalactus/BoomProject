using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour { 
    public delegate void simple(Vector3 start, Vector3 dir);
    public simple onDeath;
	public simple onDamage;
    public string killer;
    Vector3 start;
    Vector3 direction;
    public bool isAlive = true;
    public bool lastCrit;
    public float health = 100;
    // Use this for initialization
    void Start()
    {
    }

    public void heal(int amt)
    {
        if (isAlive)
        {
            health += amt;
        }
    }

	public void dmg(float amt, bool isCrit, Vector3 point, Vector3 dir, Vector3 source, bool natural)
    {
		
        start = point;
        direction = dir;
		if (isAlive) {
			health -= amt;
			onDamage (point, dir);
			//RpcDisplayDamage(amt, isCrit, point);
		}
        lastCrit = isCrit;
		//Debug.Log (isCrit);
    }
    // Update is called once per frame
    void Update () {
        if (health <= 0 && isAlive)
        {
            //healthUI.gameObject.SetActive (false);
            if (onDeath != null)
            {
                onDeath(start, direction);
            }
            isAlive = false;
        }
    }
}
