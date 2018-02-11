using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour {
	//main simple vals
	public float health = 100;
	public bool isAlive = true;

	//TRANSFORMS FOR RAGDOLL TRANSFER
	public Transform handL;
    public Transform handParentL;
    public Transform handR;
    public Transform handParentR;
    public GameObject chin;
    public GameObject head;
    public GameObject physicsHead;
    public Transform footL;
    public Transform footParentL;
    public Transform footR;
    public Transform footParentR;

	//BASIC SLOTS
	public Transform GunHolster;
	public Transform waist;
    public float deathTime = 10f;
    public Light deathEffect;
    public Transform sceneRef;
    public Transform pathGoal;
	public Rigidbody centerRigidbody;
	public WorldGun holstered;
	public Light muzzleFlash;


	//NOT TO BE USED IN EDITOR


	//FULL
	//NEEDS CLEANUP
	private Vector3 suspectedDir;
	private Vector3 knownLocation;
	private bool hasLineOfSight;
	private float defaultSpeed;

	private bool lastCrit;
    NavMeshAgent agent;
    Animator anim;
    GameObject waveController;
    GameObject currentAgro;
    public AudioSource headDeath;
    float topSpeed = 4;
    int dying = 0;
	public Transform refTrans;
	public bool awareOfDanger = false;
	private float twitch = 0;
	private GameObject player;
	private float shootTime = 0;

	//DEBUG
	public bool hasSight;
	public Collider eye;
	public float detectionRange = 30f;
	public bool DEBUG_HASSIGHT = false;
	public float tester_Forward = 0.0f;
	public float tester_Strafe = 0.0f;

    // Use this for initialization
    void Start () {
		//USE BELOW WHEN DOING OVERHEAD TESTING
		//UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
		//Cursor.lockState = CursorLockMode.None;
        sceneRef.localRotation = Quaternion.identity;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        setKinematic(true);
		if (pathGoal != null) {
			agent.destination = pathGoal.position;
		}
		defaultSpeed = agent.speed;
		muzzleFlash.enabled = false;

    }

	public void dmg(float amt, bool crit, Vector3 point, Vector3 dir, Vector3 source)
	{
        Debug.Log(crit);
        if (crit)
        {
            health -= amt * 2.5f;
        }
        else
        {
            health -= amt;
        }

		lastCrit = crit;
		if (!awareOfDanger) {
			awareOfDanger = true;
		}

		if (health <= 0) {
			Die ();
			isAlive = false;
		}
		if (agent.enabled) {
			agent.destination = source;
		}
		centerRigidbody.AddForce (dir.normalized * 500f, ForceMode.Impulse);
	}
	void TakeDamage(Vector3 source, bool natural){
		if (!awareOfDanger) {
			awareOfDanger = true;
			agent.destination = source;

			//suspectedDir = -dir;
			//float attackAngle = Vector3.Angle (refTrans.forward, noVert);
			//refTrans.rotation = Quaternion.Lerp (refTrans.rotation, Quaternion.Euler(new Vector3(refTrans.rotation.eulerAngles.x, refTrans.rotation.eulerAngles.y + attackAngle, refTrans.rotation.eulerAngles.z)), 1);
		}
	}


	private void Fire(){
		
	}
	private float firingEffect = 0f;
    void Update()
	{
		//GRAB PLAYER AT THE BEGINNING (HAS TO RUN AFTER STARTS HAVE RAN)
		if (player == null) {
			player = Toolbox.singleton.player;
		}
		//ESTABLISH WHETHER OR NOT WE ARE ATTEMPTING TO SHOOT AT THE PLAYER
		//IN THIS FRAME
		bool shooting = false;
		//HANDLE BASIC NAV AND AI
		agent.updateRotation = true;
		if (isAlive) {
			Vector3 headv = physicsHead.transform.forward;
			float dot = Vector3.Angle ((player.transform.position - physicsHead.transform.position), physicsHead.transform.forward);
			hasSight = false;
		    RaycastHit hitInfo;
			if (Physics.Linecast (physicsHead.transform.position, player.transform.position, out hitInfo)) {
				if (hitInfo.collider.gameObject == player && agent.remainingDistance < 7) {
					shooting = true;
					agent.updateRotation = false;
					hasSight = true;
					Vector3 noHeightVec = player.transform.position;
					noHeightVec.y = transform.position.y;
					transform.LookAt (noHeightVec);
					awareOfDanger = true;
					agent.destination = transform.position;
                }
                else
                {
                    agent.destination = player.transform.position;
                }
			}
			
		}
		//HANDLE GUN
		if (shooting && anim.GetCurrentAnimatorStateInfo(1).IsTag("Firable")) {
			if (shootTime <= 0) {
				anim.SetTrigger ("Fire");
				muzzleFlash.enabled = true;
				firingEffect = 0.1f;
				RaycastHit hitInfo;
				if (Physics.Raycast (physicsHead.transform.position, physicsHead.transform.forward, out hitInfo, 20f)) {
					if (hitInfo.collider.gameObject == player) {
						player.GetComponent<PlayerController> ().damage (60f);
					}
				}
				shootTime = Random.Range (0.4f, 1.2f);
			}

			//HANDLE LATENT FIRING EFFECT 
			if (firingEffect >= 0) {
				firingEffect -= Time.deltaTime;
			} else {
				muzzleFlash.enabled = false;

			}
		}
		if (shootTime > 0) {
			shootTime -= Time.deltaTime;
		}
		//HANDLE ANIMS
		//anim.SetBool ("HasGun", holstered != null);
		anim.SetBool ("Triggered", awareOfDanger);


		Vector3 localVel = (transform.InverseTransformVector(agent.velocity));
	

		anim.SetFloat("Speed", localVel.z);
		anim.SetFloat("Strafe", localVel.x);

		//DEATH EFFECT
        if (dying != 0)
        {
            deathEffect.enabled = true;
            dying--;
        }
		else if(deathEffect.enabled)
        {
            deathEffect.enabled = false;
        }
		if (twitch >= 0 && isAlive) {
			twitch -= Time.deltaTime;
			if (twitch <= 0) {
				setKinematic (true);
			}
		}
			

       
    }
	private bool hasCrit = false;
    public void Die()
    {
        // NetworkController.singleton.godObject.GetComponent<WaveController>().OnEnemyDied();
        GetComponentInChildren<Animator>().enabled = false;
		if (isAlive) {
			setKinematic (false);
			GameObject gun = (GameObject)GameObject.Instantiate(Resources.Load (holstered.resourceName));
			gun.transform.position = physicsHead.transform.position;
			gun.GetComponent<Rigidbody> ().AddForce(physicsHead.transform.forward * 3f, ForceMode.Impulse);
		}
        handL.parent = handParentL;
        handR.parent = handParentR;
        footL.parent = footParentL;
        footR.parent = footParentR;
		if (GunHolster != null) {
			Destroy (GunHolster.gameObject);
		}


		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.velocity = agent.velocity;
		}


	    //centerRigidbody.velocity = agent.velocity;
		agent.enabled = false;
		if (lastCrit && !hasCrit) {
			hasCrit = true;
			preformCrit ();
		}




    }

	private void preformCrit(){
		dying = 3;
		foreach (MeshRenderer mesh in head.GetComponentsInChildren<MeshRenderer>()) {
			mesh.enabled = true;
		}
		chin.GetComponent<MeshRenderer> ().enabled = false;
		head.GetComponent<MeshRenderer> ().enabled = false;
		foreach (Transform tf in head.GetComponentsInChildren<Transform>()) {
			Rigidbody body = tf.GetComponent<Rigidbody> ();
			Collider collider = tf.GetComponent<Collider> ();
			if (tf != chin.transform && collider != null) {
				collider.enabled = true;
			}
			tf.SetParent (null, true);
			if (body != null) {
				body.isKinematic = false;
				body.AddExplosionForce (.0001f, head.transform.position, 100f);
			}
			DestroyAfterTime dat = tf.gameObject.AddComponent<DestroyAfterTime> ();
			dat.timeUntilDeath = 3f;
		}
			/*foreach (RaycastHit hit in Physics.RaycastAll(new Ray(start, dir), 3f))
            {
                if (hit.collider.GetComponent<Rigidbody>() != null)
                {
                    //this will be set to some value porportional to damage given at some poitn
                    Debug.Log(hit.collider.name);
                    hit.collider.GetComponent<Rigidbody>().AddForce(dir * 300f);
                    if (hit.collider.gameObject != head)
                    {
                        Debug.Log("Didnt hit head");
                        hit.collider.GetComponent<Rigidbody>().useGravity = false;
                        hit.collider.transform.parent.SetParent(null, true);
                    }
                    hit.collider.enabled = false;
                }
            }*/


	}
    private void setKinematic(bool kin)
    {
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
			if (body.transform.parent != head.transform) {
				body.isKinematic = kin;
			}
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
