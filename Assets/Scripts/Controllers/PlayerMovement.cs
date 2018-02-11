using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
	public GameObject headJoint;
	public Animator head;
	public Animator externalAnim;
	public bool isPaused;
	public float speed = 7.5f;
	public float crouchDivider = 3f;
	float jumpSpeed = 5f;
	public float verticalVelocity = 0;
	Vector3 direction = Vector3.zero;
	CharacterController cc;
	Animator anim;

	Headbob headbob;
    float horizAxis = 0F;
    float vertAxis = 0F;
	private float ispeed;
	private float ibob;
	// Use this for initialization
	void Start () {
		headbob = GetComponentInChildren<Headbob> ();
		cc = GetComponent<CharacterController>();
	//	anim = GetComponent<Animator>();
		ibob = headbob.bobSpeed;
		ispeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPaused || !isLocalPlayer) {
			direction = Vector3.Lerp (direction, Vector3.zero, Time.deltaTime);
			return;
		}
		if (Input.GetButton ("Sprint")) {
			speed = ispeed * 1.2f;
			headbob.bobSpeed = ibob * 1.5f;
		} else {
			speed = ispeed;
			headbob.bobSpeed = ibob;
		}
        //lateral movement
        direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		externalAnim.SetBool ("Running", direction.magnitude > 0.1f);
        externalAnim.SetBool("Sprinting",Input.GetButton("Sprint"));
        if (direction.magnitude > 1f)
        {
            direction = direction.normalized;
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
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;
					
            }
        }
	    externalAnim.SetBool("Jumping", !cc.isGrounded);
	}


	void FixedUpdate (){
		if (!isLocalPlayer) {
			return;
		}
		Vector3 distance = direction * speed * Time.deltaTime;
		if(cc.isGrounded && verticalVelocity < 0){
			//anim.SetBool("Jumping", false);
			verticalVelocity = Physics.gravity.y * Time.deltaTime;
		}else{
			if(Mathf.Abs(verticalVelocity) > 0.75f){
				//anim.SetBool("Jumping", true);
			}
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
		}
		distance.y = verticalVelocity * Time.deltaTime;

		cc.Move(distance);
	}
}
