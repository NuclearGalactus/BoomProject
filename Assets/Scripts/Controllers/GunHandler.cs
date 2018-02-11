using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class GunHandler : NetworkBehaviour {
    public Transform accuracyReference;

	public bool debug_BottomlessClip;

	public int numOfHoles = 50;

	public float MoveAmount = 0;
	public float MoveSpeed = 2;

	public float MoveOnX;

	public float MoveOnY;

	public float xRecoil = 100f;
	public float yRecoil = 0f;

	public int totalAmmo = 30;
	public int clipAmmo = 7;

	public int damage = 40;

	public float zoomFOV = 20;

	public float GunAcc;

	public Vector3 DefaultPos;

	public Vector3 NewGunPos;

	public Animator viewAnimator;

	public GameObject GUN;

	public MeshRenderer MuzzleFlash;

	public Transform Head;

	public Camera viewModelCamera;

	public AudioSource[] gunsounds;
	public AudioSource reload;

	public bool fullAuto = true;
	public bool longAction = false;

    public AudioSource hitmarker;
    public AudioSource crithitmarker;
    public AudioSource gunShot;

    public float RPM = 950;

	Text clipAmmoUI;
	Text totalAmmoUI;
	GameObject AmmoUI;
    Animator externalAnim;
    int clipSize;

    

	bool enableMuzzle = false;
	bool recoiling = false;
	bool recoiling2 = false;
	bool readyToFire = true;

    float range = 1;
	bool aimingDown = false;

	int clickFrames = 0;
	float goalFov;
	public float defaultFov;
    float timeBetweenShots;
    float rpmTimer;
    //float recoilCurrent;
    //float recoilAmt;
    float speed;
	RectTransform mainCanvas;
    public int WEAPON_TYPE = 0;
    public Transform cylinder;
    Vector3 originalCam;
	[SyncVar]
	int index = 0;

	[ClientRpc]
	public void RpcHandleDamage(Vector3 point, float amt){
		Vector2 loc = viewModelCamera.WorldToViewportPoint (point);
		Vector2 screenloc = new Vector2((loc.x * mainCanvas.sizeDelta.x) - (mainCanvas.sizeDelta.x * .5f),
			((loc.y * mainCanvas.sizeDelta.y) - (mainCanvas.sizeDelta.y * 0.5f)));
		GameObject obj = (GameObject)Instantiate (Resources.Load ("DamageIndicator"), Vector3.zero, Quaternion.identity, mainCanvas.transform);
		obj.GetComponent<RectTransform> ().anchoredPosition = screenloc;
	}

    public virtual void Fire()
    {
        speed += 5f;
    }

	void Start(){
        originalCam = GetComponent<Player>().transform.localEulerAngles;
        timeBetweenShots = 60 / RPM;
        rpmTimer = 0;
        externalAnim = GetComponent<Player>().external.GetComponent<Animator>();
		mainCanvas = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<RectTransform>();
        defaultFov = 40;
        goalFov = defaultFov;
		AmmoUI = GameObject.FindWithTag ("GunUI");
		totalAmmoUI = AmmoUI.transform.Find ("TotalAmmo").GetComponent<Text>();
		clipAmmoUI = AmmoUI.transform.Find ("ClipAmmo").GetComponent<Text>();
		DefaultPos = GUN.transform.localPosition;
		clipSize = clipAmmo;
		gunsounds = GetComponents<AudioSource> ();
		reload = gunsounds [0];
		//gunshot = gunsounds [1];
	}
    bool shaking = false;
    float shakeVal = 0f;
    float amp = 0;
    
	void ShakeCam(float amplitude)
    {
        shaking = true;
        shakeVal = 0f;
        amp = amplitude;
    }

	void Update() {
		if (!isLocalPlayer) {
			return;
		}
        if (shaking)
        {
            if((2 * Mathf.PI * 3 * shakeVal) < (Mathf.PI))
            {
                shakeVal += Time.deltaTime;
                GetComponent<Player>().cam.transform.localEulerAngles = originalCam + new Vector3(0, 0,amp *  Mathf.Sin(2 * Mathf.PI * 3 * shakeVal));
            }else
            {
                shakeVal = 0;
                shaking = false;
                GetComponent<Player>().cam.transform.localEulerAngles = originalCam;
            }
        }
        if (speed >= 0)
        {
            speed -= Time.deltaTime * 2;
        }else
        {
            speed = 0;
        }

        if(rpmTimer > 0)
        {
            rpmTimer -= Time.deltaTime;
        }
		viewModelCamera.fieldOfView = Mathf.Lerp (viewModelCamera.fieldOfView, goalFov, Mathf.SmoothStep (0.0f, 1.0f, Time.deltaTime * 10));
		GetComponent<Player>().uicam.fieldOfView = Mathf.Lerp (GetComponent<Player>().uicam.fieldOfView, goalFov, Mathf.SmoothStep (0.0f, 1.0f, Time.deltaTime * 10));
		GetComponent<Player>().cam.fieldOfView = Mathf.Lerp (GetComponent<Player>().cam.fieldOfView, goalFov, Mathf.SmoothStep (0.0f, 1.0f, Time.deltaTime * 10));

        if (WEAPON_TYPE == 0)
        {
            //MELEE WEAPON HANDLING
            viewAnimator.SetBool("HoldingMelee",Input.GetButton("Fire1"));

        }
        else
        {
            viewAnimator.SetBool("IsIronSights", aimingDown);



            if (aimingDown)
            {
                goalFov = zoomFOV;
                clickFrames++;
                if (Input.GetButtonUp("Fire2") && clickFrames > 10 && !isReloading() && rpmTimer <= 0)
                {
                    goalFov = defaultFov;
                    //viewAnimator.Play ("IronSights Backwards", 0,1 - Mathf.Clamp(viewAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime,0,1));
                    //viewAnimator.SetBool ("IsIronSights", false);
                    aimingDown = false;
                    clickFrames = 0;
                }
            }
            else
            {
                goalFov = defaultFov;
                if (Input.GetButtonDown("Fire2"))
                {
                    aimingDown = true;
                    viewAnimator.SetBool("IsIronSights", true);

                    goalFov = zoomFOV;
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                viewAnimator.SetTrigger("Equip");

            }


            if (((Input.GetButtonDown("Fire1") && !longAction) || (Input.GetButton("Fire1") && fullAuto) || (Input.GetButtonDown("Fire1") && longAction && readyToFire)) && clipAmmo > 0 && rpmTimer <= 0)
            {
                Debug.Log("Firing");
                if (isLocalPlayer)
                {
                    ShakeCam(2f);
                    Fire();
                    accuracyReference.localEulerAngles += new Vector3(Random.Range(0, 0), Random.Range(0, 0), 0f);
                    CmdFireGun();
                    accuracyReference.localEulerAngles = Vector3.zero;

                    //ActivateMuzzleFlash ();
                    //gunshot.Play ();
                    rpmTimer = timeBetweenShots;
                    //MuzzleFlash.transform.Rotate (new Vector3 (0, Random.Range (0, 360), 0));
                    //Debug.Log (viewAnimator.gameObject.name);
                    int num = Random.Range(1, 2);
                    viewAnimator.ResetTrigger("Fire 1");
                    viewAnimator.ResetTrigger("Fire 2");
                    viewAnimator.ResetTrigger("Fire 3");
                    viewAnimator.SetTrigger("Fire " + num);
                    externalAnim.SetTrigger("Fire");
                    recoiling = true;
                    //	recoilAmt = 2;
                    //	recoilCurrent = 0;
                    readyToFire = false;
                    clipAmmo--;
                }
            }




            if (recoiling)
            {
                //recoilCurrent += Time.deltaTime * yRecoil;
                //GetComponent<LookController> ().rotationY += Time.deltaTime * yRecoil;
                //float horizRecoil = xRecoil;
                //bool Rval = Random.value > .5;
                //if (Rval) {
                //horizRecoil = -xRecoil;
                //}
                //Head.transform.parent.rotation = Quaternion.Euler(new Vector3 (Head.transform.parent.rotation.eulerAngles.x,Head.transform.parent.rotation.eulerAngles.y + (Time.deltaTime * Random.Range(horizRecoil - 20,horizRecoil + 20)),Head.transform.parent.rotation.eulerAngles.z));
                if (Input.GetButtonUp("Fire1"))
                {
                    recoiling = false;
                    recoiling2 = true;
                }
            }

            if (recoiling2)
            {
                //recoilCurrent -= Time.deltaTime * yRecoil / 3;
                //GetComponent<LookController> ().rotationY -= Time.deltaTime * yRecoil / 3;
                //if (recoilCurrent <= 0) {
                //	recoiling2 = false;
                //	}
            }

            if (Input.GetButtonDown("Reload") && clipAmmo < clipSize && !isReloading())
            {
                reload.Play();
                Debug.Log("reset");
                goalFov = defaultFov;
                viewAnimator.SetTrigger("Reload");
                externalAnim.SetTrigger("Reload");
            }
        }
		MoveOnX = -Input.GetAxis ("Mouse X") * Time.deltaTime * MoveAmount;

		MoveOnY = -Input.GetAxis ("Mouse Y") * Time.deltaTime * MoveAmount;

		NewGunPos = new Vector3 (DefaultPos.x+MoveOnX, DefaultPos.y+MoveOnY, DefaultPos.z);

		GUN.transform.localPosition = Vector3.Lerp(GUN.transform.localPosition, NewGunPos, MoveSpeed*Time.deltaTime*2);

		totalAmmoUI.text = totalAmmo.ToString();
		clipAmmoUI.text = clipAmmo.ToString();

	}

	bool isReloading(){
		return viewAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Reload");
	}


	public void ReadyUp(){
		readyToFire = true;
		if (fullAuto && Input.GetButton("Fire1") && clipAmmo > 0) {
			if (isLocalPlayer) {
				CmdFireGun();
                //ActivateMuzzleFlash ();
                //gunshot.Play ();
                //MuzzleFlash.transform.Rotate (new Vector3 (0, Random.Range (0, 360), 0));
                int num = Random.Range(1,3);
				viewAnimator.SetTrigger ("Fire " + num);
                externalAnim.SetTrigger("Fire");
                recoiling = true;
				//recoilAmt = 2;
				//recoilCurrent = 0;
				readyToFire = false;
				clipAmmo--;
			}
		}
	}

    public void MeleeAttack()
    {
        CmdFireGun();
    }


	public void Reload(){
		totalAmmo -= (clipSize - clipAmmo);
		clipAmmo = clipSize;
	}

	public void CheckZoom(){
		if (aimingDown) {
			goalFov = zoomFOV;
		}
	}

	public void ActivateMuzzleFlash(){
		enableMuzzle = true;
        
	}
    [TargetRpc]
    public void TargetPlayHitmarker(NetworkConnection target, bool crit)
    {
        if (crit)
        {
            crithitmarker.Play();
        }else
        {
            hitmarker.Play();
        }
    }




	[Command]
	public void CmdFireGun(){
        int layermask = ~(1 << 9);
		bool crit = false;
		RaycastHit hitInfo;
        Vector3 pos = GetComponent<Player>().head.transform.position;
        Vector3 dir = GetComponent<Player>().head.transform.forward;
        Ray ray = new Ray (pos, dir);
        RpcPlayGunShot();
        if (Physics.Raycast(ray, out hitInfo, range, layermask)){
			Debug.Log (hitInfo.collider.name);
			SecretWorkingHealth health = hitInfo.collider.GetComponentInParent<SecretWorkingHealth> ();
            if (health == null) {

			} else if(health.isAlive){
	
                TargetPlayHitmarker(connectionToClient, crit);
				crit = hitInfo.collider.gameObject.tag == "Crit";
				if (crit) {
					health.dmg (damage * 2, true, hitInfo.point, dir.normalized);

				} else {
					health.dmg (damage, false, hitInfo.point, dir.normalized);
				}
                if(health.health <= 0)
                {
                    health.killer = GetComponent<Player>().name;
                }
			}
			if (hitInfo.collider.GetComponent<Rigidbody> () != null) {
				hitInfo.collider.GetComponent<Rigidbody> ().AddForce (Head.forward.normalized * 1000f);
			}
		}


	}


    [ClientRpc]
    public void RpcPlayGunShot()
    {
        gunShot.Play();
    }

}
