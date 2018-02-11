using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	public float HEALTH = 100;

    public Transform headJoint;
    public Camera cam;
    public PostProcessingBehaviour ppb;
	public Transform leanJoint;
    PostProcessingProfile profile;

    public bool isCrouched;

    public Text deadText;

    public float sensitivity = 1;
    public float minX = -360F;
    public float maxX = 360F;
    public float minY = -60F;
    public float maxY = 60F;
    public float rotationY = 0F;
    public float rotationX = 0F;

    public float crouchSpeed = 1;

    public bool isPaused;
    public float speed = 7.5f;
    public float crouchDivider = 3f;
    float jumpSpeed = 5f;

    public float verticalVelocity = 0;
    
    Vector3 direction = Vector3.zero;
    CharacterController cc;

    Headbob headbob;

    float horizAxis = 0F;
    float vertAxis = 0F;
    private float ispeed;
    private float ibob;
    private float leanPos = 0;
    private float addH;
    private float addV;

    // Use this for initialization
    void Start()
    {



        profile = ppb.profile;
        headbob = GetComponentInChildren<Headbob>();
        cc = GetComponent<CharacterController>();
        //	anim = GetComponent<Animator>();
        
        ibob = headbob.bobSpeed;
        ispeed = speed;
		chroma = ppb.profile.chromaticAberration.settings;
		chroma.intensity = 0;
		ppb.profile.chromaticAberration.settings = chroma;
    }


	public void damage(float amt){
		HEALTH -= amt;
		if (HEALTH <= 0) {
			Debug.Log ("DEAD");
		}
	}

    VignetteModel.Settings vignette;
	ChromaticAberrationModel.Settings chroma;
    // Update is called once per frame
    void Update()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        if (isPaused)
        {
            direction = Vector3.Lerp(direction, Vector3.zero, Time.deltaTime);
            return;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            isCrouched = !isCrouched;
        }

        if (isCrouched)
        {
            vignette = profile.vignette.settings;
            vignette.intensity = Mathf.Lerp(vignette.intensity, .19f, Time.deltaTime * crouchSpeed);
            profile.vignette.settings = vignette;
            cc.height = Mathf.Lerp(cc.height,1,Time.deltaTime * crouchSpeed);
            cc.center = new Vector3(cc.center.x, Mathf.Lerp(cc.center.y, -0.5f, Time.deltaTime * crouchSpeed), cc.center.z);
			headJoint.localPosition = new Vector3(0,Mathf.Lerp(headJoint.localPosition.y, 0.25f, Time.deltaTime * crouchSpeed),0);
        }
        else
        {
            vignette = profile.vignette.settings;
            vignette.intensity = Mathf.Lerp(vignette.intensity, .088f, Time.deltaTime * crouchSpeed);
            profile.vignette.settings = vignette;
            cc.height = Mathf.Lerp(cc.height, 2, Time.deltaTime * crouchSpeed);
            cc.center = new Vector3(cc.center.x, Mathf.Lerp(cc.center.y, 0, Time.deltaTime * crouchSpeed), cc.center.z);
			headJoint.localPosition = new Vector3(0,Mathf.Lerp(headJoint.localPosition.y, 0.692f, Time.deltaTime * crouchSpeed),0);
        }
        //transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        rotationX = (Input.GetAxis("Mouse X") * sensitivity);
        rotationX += addH;
        addH = 0;
		Vector3 oldRot = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(new Vector3(oldRot.x, oldRot.y + rotationX, oldRot.z));

        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY += addV;
        addV = 0;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        headJoint.transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, 0, 0));


        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
		if (Input.GetButton("Sprint") && !(isCrouched) && Mathf.Abs(Input.GetAxis("Vertical")) > 0.5f)
        {
			
            speed = ispeed * 2;
            //headbob.bobSpeed = ibob * 1.5f;
		}
        else if (isCrouched)
        {
            speed = ispeed / 2;
            headbob.bobSpeed = ibob / 1.5f;
        }
        else
        {
            speed = ispeed;
            headbob.bobSpeed = ibob;
        }
		GetComponent<GunController> ().setSprint (Input.GetButton("Sprint") && !(isCrouched) && Mathf.Abs(Input.GetAxis("Vertical")) > 0.5f);
        //lateral movement
        direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (direction.magnitude > 1f)
        {
            direction = direction.normalized;
        }
		if (HEALTH <= 50) {
			chroma = ppb.profile.chromaticAberration.settings;
			chroma.intensity = 1;
			ppb.profile.chromaticAberration.settings = chroma;
		}
        //anim.speed = direction.magnitude;
        //anim.SetFloat("Speed", Input.GetAxis("Vertical"));
        //jump handler
        /*if (Input.GetButtonDown ("Crouch")) {
			speed = speed / crouchDivider;
			anim.SetBool ("Crouching", true);
		}
		if(Input.GetButtonUp("Crouch")){
			speed = speed * crouchDivider;
			anim.SetBool ("Crouching", false);
		}*/
		leanPos = 0;
		if (Input.GetKey (KeyCode.Q)) {
			leanPos += 15;
		}
		if (Input.GetKey (KeyCode.E)) {
			leanPos -= 15;
		}
		//leanJoint.localRotation = Quaternion.Lerp(leanJoint.localRotation, Quaternion.Euler(new Vector3(0,0,leanPos)), Time.deltaTime * 3f);
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;

            }
        }
    }

    public Vector3 getDirection()
    {
        return direction;
    }


    public void addHRotation(float val)
    {
        addH = val;
    }

    public void addVRotation(float val)
    {
        addV = val;
    }

    void FixedUpdate()
    {
        Vector3 distance = direction * speed * Time.deltaTime;
        if (cc.isGrounded && verticalVelocity < 0)
        {
            //anim.SetBool("Jumping", false);
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            if (Mathf.Abs(verticalVelocity) > 0.75f)
            {
                //anim.SetBool("Jumping", true);
            }
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        distance.y = verticalVelocity * Time.deltaTime;

        cc.Move(distance);
    }
}
