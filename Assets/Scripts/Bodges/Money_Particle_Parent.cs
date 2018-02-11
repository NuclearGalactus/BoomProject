using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money_Particle_Parent : MonoBehaviour {
    public Transform killer;
    public float timeUntilGo = 3f;
    float speed = 3f;
    Rigidbody[] bodies;
    bool started = false;
    // Use this for initialization
    void Start () {
        killer = GameObject.Find(GetComponentInParent<SecretWorkingHealth>().killer).transform;
        bodies = GetComponentsInChildren<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!started)
        {
            timeUntilGo -= Time.deltaTime;
            if (timeUntilGo < 0)
            {
                foreach (Rigidbody body in bodies)
                {
                    body.GetComponent<Collider>().enabled = false;
                }
                started = true;
            }
        }
        else
        {

            foreach (Rigidbody body in bodies)
            {
                if (body != null)
                {
                    body.velocity += (killer.position - body.transform.position).normalized * speed;
                    if ((killer.position - body.transform.position).magnitude < 1f)
                    {
                        killer.GetComponent<Player>().addMoney(1);
                        Destroy(body.gameObject);
                    }
                }
            }
            
        }


    }


}
