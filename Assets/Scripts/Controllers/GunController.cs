using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunController : MonoBehaviour
{
    public bool gunDebug;

    public Gun[] gunData;

    public bool debug_BottomlessClip;

    public int numOfHoles = 50;

    public float MoveAmount = 0;
    public float MoveSpeed = 2;


    public float swayAmp = 1;
    public float swayFreqX = 3;
    public float swayFreqY = 5;
    public float swayOffsetX = 45;
    public float swayOffsetY = 30;

    public float damage = 40f;

    public float zoomFOV = 20;



    private Vector3 DefaultPos;

    private Vector3 NewGunPos;

    public Animator viewAnimator;

    public Transform GUN;
    public Transform recoilParent;

    public MeshRenderer MuzzleFlash;
    public Light MuzzleFlashLight;

    public Transform Head;

    public Camera viewModelCamera;

    public float sideRecoil = 0.4f;
    public float initialRecoil = 4f;
    public float secondaryRecoil = .5f;

    private bool hasShot = false;
    private Vector3 lastShotPosition;

    Text clipAmmoUI;
    Text totalAmmoUI;
    public AmmoUIManager ammoUI;
    Animator externalAnim;
    int clipSize;

    bool enableMuzzle = false;
    bool recoiling = false;
    bool recoiling2 = false;
    bool readyToFire = true;

    float range = 100;
    bool aimingDown = false;

    int clickFrames = 0;
    float goalFov;
    private float defaultFov;
    float timeBetweenShots;
    float rpmTimer;
    float speed;
    RectTransform mainCanvas;
    Vector3 originalCam;
    int index = -1;
    private int clipAmmo;
    private int totalAmmo;
    PlayerController player;
    Inventory inven;
    private Headbob hbob;
    private float iampbob;
    
    private int muzzleFlashCounter;
    public void equip(int ind)
    {
        viewAnimator.SetLayerWeight(index + 2, 0);
        viewAnimator.SetLayerWeight(ind + 2, 1);
        if (index != -1)
        {
            gunData[index].mainRenderer.enabled = false;
        }
        gunData[ind].mainRenderer.enabled = true;
        index = ind;
        clipAmmo = gunData[ind].clipSize;
        clipSize = gunData[ind].clipSize;
        damage = gunData[ind].damage;
        viewAnimator.SetInteger("Equipping", ind);
        viewAnimator.SetTrigger("Equip");
        ammoUI.setClipAmmo(clipAmmo);
        ammoUI.setTotalAmmo(inven.pistolAmmo);
    }


    public virtual void Fire()
    {
        speed += 5f;
    }
    private float currentMoveSpeed;
    void Start()
    {
        inven = GetComponent<Inventory>();
        equip(0);
        hbob = GetComponentInChildren<Headbob>();
        iampbob = hbob.bobAmplitude;
        currentMoveSpeed = MoveSpeed;
        originalCam = GetComponent<PlayerController>().headJoint.localEulerAngles;
        rpmTimer = 0;
        defaultFov = viewModelCamera.fieldOfView;
        goalFov = defaultFov;
        DefaultPos = GUN.localPosition;
        player = GetComponent<PlayerController>();
    }

    bool shaking = false;
    float shakeVal = 0f;
    float amp = 0;

    void ShakeCam(float amplitude, float sign)
    {
        shaking = true;
        shakeVal = 0f;
        int isNeg = Random.Range(0, 2);
        amp = amplitude * sign;
    }

    private Vector3 recoil;


    void StartRecoil() {
        recoiling = true;
        float yRecoil = .3f;
        float zRecoil = -1.6f;
        float xRecoil = Random.Range(-sideRecoil, sideRecoil);
        float sign;
        if (xRecoil > 0) {
            sign = 1f;
        } else {
            sign = -1f;
        }
        if (aimingDown) {
            yRecoil /= 3f;
            xRecoil /= 2f;
            ShakeCam(4f, sign);
        } else {
            ShakeCam(1f, sign);
        }
        recoil = new Vector3(xRecoil, yRecoil, zRecoil);
        if (aimingDown)
        {
            currentMoveSpeed = 14f * recoil.magnitude;
        }
        else
        {
            currentMoveSpeed = 5f * recoil.magnitude;
        }

    }
    float MoveOnX = 0f;
    float MoveOnY = 0f;
    private float timer = 0;
    void Update()
    {
        muzzleFlashCounter--;
        if (muzzleFlashCounter < 0) {
            //MuzzleFlashLight.enabled = false;
            //MuzzleFlash.enabled = false;
        }
        float xMouse = -Input.GetAxis("Mouse X");
        float yMouse = -Input.GetAxis("Mouse Y");
        
        float SwayOnX;
        float SwayOnY;
        if (!aimingDown) {
            MoveOnX = Mathf.Lerp(MoveOnX, xMouse * MoveAmount, Time.deltaTime * MoveSpeed);
            MoveOnY = Mathf.Lerp(MoveOnY, yMouse * MoveAmount, Time.deltaTime * MoveSpeed);
            SwayOnX = (swayAmp * Mathf.Sin((swayFreqX * Time.time)));
            SwayOnY = (swayAmp * Mathf.Sin((swayFreqY * Time.time)));
        } else {
            MoveOnX = Mathf.Lerp(MoveOnX, xMouse * MoveAmount / 2, Time.deltaTime * MoveSpeed);
            MoveOnY = Mathf.Lerp(MoveOnY, yMouse * MoveAmount / 2, Time.deltaTime * MoveSpeed);
            SwayOnX = (swayAmp * Mathf.Sin((swayFreqX * Time.time))) / 6f;
            SwayOnY = (swayAmp * Mathf.Sin((swayFreqY * Time.time))) / 6f;
        }
        if (aimingDown)
        {
            GUN.localPosition =DefaultPos +  new Vector3(MoveOnX + SwayOnX , MoveOnY + SwayOnY, 0);
        }
        else
        {
            GUN.localPosition =DefaultPos +  new Vector3(MoveOnX + SwayOnX - (Input.GetAxis("Horizontal") * .005f), MoveOnY + SwayOnY, -(Input.GetAxis("Vertical") * .01f));
        }

        NewGunPos = Vector3.zero;
        if (shaking)
        {
            if ((2 * Mathf.PI * 3 * shakeVal) < (Mathf.PI))
            {
                shakeVal += Time.deltaTime;
                GetComponent<PlayerController>().cam.transform.localEulerAngles = originalCam + new Vector3(0, 0, amp * Mathf.Sin(2 * Mathf.PI * 3 * shakeVal));
            }
            else
            {
                shakeVal = 0;
                shaking = false;
                GetComponent<PlayerController>().cam.transform.localEulerAngles = originalCam;
            }
        }
        if (speed >= 0)
        {
            speed -= Time.deltaTime * 2;
        }
        else
        {
            speed = 0;
        }
        if (rpmTimer > 0)
        {
            rpmTimer -= Time.deltaTime;
        }
        viewModelCamera.fieldOfView = Mathf.Lerp(viewModelCamera.fieldOfView, goalFov, Time.deltaTime * 15f);
        GetComponent<PlayerController>().cam.fieldOfView = Mathf.Lerp(GetComponent<PlayerController>().cam.fieldOfView, goalFov * 1.5f, Mathf.SmoothStep(0.0f, 1.0f, Time.deltaTime * 15f));
        if (index != -1) { 
            viewAnimator.SetBool("Crouched", GetComponent<PlayerController>().isCrouched);
		    viewAnimator.SetBool("IsFull", clipAmmo >= getGun().clipSize);
		    viewAnimator.SetBool("Reloading", Input.GetButton("Reload"));
        }
        if (index == -1)
        {
            //MELEE WEAPON HANDLING
            // viewAnimator.SetBool("HoldingMelee", Input.GetButton("Fire1"));

        }
        else
        {
            viewAnimator.SetBool("IsIronSights", aimingDown);
            if (aimingDown)
            {
				hbob.bobAmplitude = iampbob / 2f;
                goalFov = zoomFOV;
                clickFrames++;
                if (Input.GetButtonUp("Fire2")  && !isReloading())
                {
                    goalFov = defaultFov;
                    aimingDown = false;
                    clickFrames = 0;
                }
            }
            else
            {
				hbob.bobAmplitude = iampbob;
                goalFov = defaultFov;
                if (Input.GetButtonDown("Fire2"))
                {
                    aimingDown = true;
                    viewAnimator.SetBool("IsIronSights", true);

                    goalFov = zoomFOV;
                }
            }

			if (((Input.GetButtonDown ("Fire1") && !(getGun().type == Gun.GunType.LONG)) || (Input.GetButton ("Fire1") && (getGun().type == Gun.GunType.FULL)) || 
                (Input.GetButtonDown ("Fire1") && (getGun().type == Gun.GunType.LONG) && readyToFire) && rpmTimer <= 0) && clipAmmo > 0) { 
				//recoilParent.localPosition = Vector3.zero;
                StartCoroutine(playAudio(getGun().fire));
                FireGun ();
				rpmTimer = 60 / getGun().RPM;
                viewAnimator.SetTrigger ("Fire");
				StartRecoil ();
 
                player.addHRotation(recoil.x * 2f);
                player.addVRotation(recoil.y * 2f);
                readyToFire = false;
				clipAmmo--;
                ammoUI.setClipAmmo(clipAmmo);
			} else if (Input.GetButtonDown("Fire1") && clipAmmo < 1) {
                StartCoroutine(playAudio(getGun().outOfAmmo));
            }
				
			if (recoiling) {
				NewGunPos += recoil;
			} else if (recoiling2) {
				currentMoveSpeed = MoveSpeed;
			}

            if (Input.GetButtonDown("Reload") && clipAmmo < clipSize && inven.pistolAmmo > 0)
            {
                Debug.Log("yeet");
                goalFov = defaultFov;
                aimingDown = false;
                viewAnimator.SetTrigger("Reload");
            }
        }


        if (recoilParent.localPosition == NewGunPos)
        {
            if (recoiling)
            {
                recoiling = false;
                currentMoveSpeed = MoveSpeed;
                recoiling2 = true;
            }
            else if (recoiling2)
            {
                recoiling2 = false;
                currentMoveSpeed = MoveSpeed;
            }
        }
		recoilParent.localPosition = Vector3.Lerp(recoilParent.localPosition, NewGunPos, Time.deltaTime * currentMoveSpeed * 5f);
    }

    bool isReloading()
    {
        return viewAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload");
    }


    public Gun getGun()
    {
        return gunData[index];
    }


    public void ReadyUp()
    {
        readyToFire = true;
        if ((getGun().type == Gun.GunType.FULL) && Input.GetButton("Fire1") && clipAmmo > 0)
        {
                FireGun();
                int num = Random.Range(1, 3);
                viewAnimator.SetTrigger("Fire");

                recoiling = true;
                readyToFire = false;
                clipAmmo--;
        }
    }

    public IEnumerator playAudio(AudioClip soundEffect)
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        yield break;
    }


    public void addBullet(){
		clipAmmo++;
	}

    public void MeleeAttack()
    {
        FireGun();
    }


	public void setSprint(bool sprinting){
		viewAnimator.SetBool ("Sprinting", sprinting);
		if (sprinting && aimingDown) {
			aimingDown = false;
			goalFov = defaultFov;
		}
	}

    public void Reload()
    {

        inven.pistolAmmo -= clipSize - clipAmmo;
        ammoUI.setTotalAmmo(inven.pistolAmmo);
        clipAmmo = clipSize;
        ammoUI.setClipAmmo(clipAmmo);
    }

    public void CheckZoom()
    {
        if (aimingDown)
        {
            goalFov = zoomFOV;
        }
    }

    public void ActivateMuzzleFlash()
    {
		//MuzzleFlashLight.enabled = true;
		//MuzzleFlash.enabled = true;
		//muzzleFlashCounter = 1;
    }
    public void TargetPlayHitmarker(bool crit)
    {
    }


    public void FireGun()
    {
		ActivateMuzzleFlash ();
		lastShotPosition = transform.position;
        int layermask = ~(1 << 2);
        RaycastHit hitInfo;
        Vector3 pos = GetComponent<PlayerController>().cam.transform.position;
        Vector3 dir = GetComponent<PlayerController>().cam.transform.forward;
        Ray ray = new Ray(pos, dir);
		//Accuracy calculation
        if (Physics.Raycast(ray, out hitInfo, range,layermask))
        {
            if (gunDebug)
            {
                GameObject effect = (GameObject)GameObject.Instantiate(Resources.Load("DebugCube"));
                effect.transform.position = hitInfo.point;
            }
            
            BasicEnemy enemy = hitInfo.collider.GetComponentInParent<BasicEnemy>();
            if(enemy != null)
            {
                enemy.dmg(getGun().damage, hitInfo.collider.tag == "Crit", hitInfo.point, hitInfo.normal, transform.position);
            }
            else
            {
                GameObject effect = (GameObject)GameObject.Instantiate(Resources.Load("SparkEffect"));
                effect.transform.position = hitInfo.point;
                effect.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            }



            Rigidbody body = hitInfo.collider.GetComponent<Rigidbody>();
			if(body != null) {
				body.AddForce(dir * 1000f,ForceMode.Impulse);
			}
        }


    }


}
