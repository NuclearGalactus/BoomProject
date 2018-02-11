using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

[RequireComponent(typeof(SecretWorkingHealth))]
public class Grunt : NetworkBehaviour {

	public Transform handL;
	public Transform handParentL;
	public Transform handR;
	public Transform handParentR;
	public GameObject chin;
	public GameObject head;
	public Transform footL;
	public Transform footParentL;
	public Transform footR;
	public Transform footParentR;
	public float deathTime = 10f;
    GameObject waveController;
	GameObject currentAgro;
	NavMeshAgent agent;
    public AudioSource headDeath;

	// Use this for initialization
	void Start () {
        headDeath = GetComponent<AudioSource>();
		GetComponent<SecretWorkingHealth> ().onDeath += Die;
		setKinematic (true);
		setGrav(true);
		agent = GetComponent<NavMeshAgent> ();
		if (!isServer) {
			agent.enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<SecretWorkingHealth> ().isAlive) {
			deathTime -= Time.deltaTime;
			if (deathTime < 0) {
				Destroy (gameObject);
			}
		}
		if (!GetComponent<SecretWorkingHealth>().isAlive || !isServer) {
			return;
		}
		if (currentAgro == null) {
			
			GameObject cloest = null;
			foreach (KeyValuePair<string, GameObject> entry in NetworkController.singleton.players) {
				if (cloest == null) {
					cloest = entry.Value;
					continue;
				}
				if (Vector3.Distance (transform.position, entry.Value.transform.position) <
                    Vector3.Distance (transform.position, cloest.transform.position)) {
					cloest = entry.Value;
				}
			}
			currentAgro = cloest;
		} else {
			agent.destination = currentAgro.transform.position;
		}
		GetComponentInChildren<Animator>().SetBool("Running", agent.remainingDistance > 2.5f);
		if (agent.remainingDistance > 5f || currentAgro == false) {
			foreach (KeyValuePair<string, GameObject> entry in NetworkController.singleton.players) {
				if (Vector3.Distance (transform.position, entry.Value.transform.position) < 5f) {
					currentAgro = entry.Value;
				}
			}
        }

		agent.isStopped = agent.remainingDistance < 2.5f;
		if (agent.isStopped) {
			agent.velocity = Vector3.zero;
		}

	}




	public void Die(Vector3 start, Vector3 dir){
		Debug.Log ("Dead");
        if (isServer)
        {
            NetworkController.singleton.godObject.GetComponent<WaveController>().OnEnemyDied();
            agent.velocity = Vector3.zero;
            agent.destination = transform.position;
        }
		GetComponentInChildren<Animator> ().enabled = false;
		handL.parent = handParentL;
		handR.parent = handParentR;
		footL.parent = footParentL;
		footR.parent = footParentR;
		setKinematic (false);
		if (GetComponent<SecretWorkingHealth> ().lastCrit) {
            // headDeath.Play();
            //head_explodo.SetActive (false);
          
            Debug.Log(head);
            chin.GetComponent<MeshRenderer> ().enabled = false;
            foreach (MeshRenderer mesh in head.GetComponentsInChildren<MeshRenderer>()) {
            	mesh.enabled = true;
            }
            head.GetComponent<MeshRenderer>().enabled = false;
            foreach (RaycastHit hit in Physics.RaycastAll(new Ray(start, dir), 3f))
            {
                if(hit.collider.GetComponent<Rigidbody>() != null)
                {
                    //this will be set to some value porportional to damage given at some poitn
                    hit.collider.GetComponent<Rigidbody>().AddForce(dir * 3f);
                    hit.collider.GetComponent<Rigidbody>().useGravity = false;
                    hit.collider.transform.parent.SetParent(null,true);
                    hit.collider.enabled = false;
                }
            }
			
			setGrav(false);
		}
		
	}

	private void setKinematic(bool kin){
		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = kin;
		}
	}

    private void setGrav(bool kin)
    {
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.useGravity = kin;
        }
    }
}
