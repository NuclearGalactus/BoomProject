using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour {
	private float timer = 0.0f;
	public float bobSpeed = 0.18f;
	public float bobAmplitude = 0.2f;
	public float refPoint = 2.0f;
	
	// Update is called once per frame
	void Update () {
		if (!GetComponentInParent<CharacterController> ().isGrounded) {
			return;
		}
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		Vector3 cSharpConversion = transform.localPosition;
		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
			timer = 0.0f;
		}
		else {
			waveslice = Mathf.Sin(timer);
			timer = timer + bobSpeed;
			if (timer > Mathf.PI * 2) {
				timer = timer - (Mathf.PI * 2);
			}
		}

		if (waveslice != 0) {
			float translateChange = waveslice * bobAmplitude;
			float totalAxes = Mathf.Abs (horizontal) + Mathf.Abs (vertical);
			totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			cSharpConversion.y = refPoint + translateChange;
		} else {
			cSharpConversion.y = refPoint;
		}
		transform.localPosition = cSharpConversion;
	}

}
