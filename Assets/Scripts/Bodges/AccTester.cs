using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccTester : MonoBehaviour {

	public InputField umovementSpeed;
	public InputField uaimDownSights;
	public InputField uisTouchingGround;
	public InputField utimeSinceLastShot;

	public double movementSpeed;
	public double aimDownSights;
	public double isTouchingGround;
	public double timeSinceLastShot;



	public Text OUTPUT;
	void Update () {
		
		movementSpeed = float.Parse(umovementSpeed.text);
		aimDownSights = float.Parse(uaimDownSights.text);
		isTouchingGround = float.Parse(uisTouchingGround.text);
		timeSinceLastShot = float.Parse(utimeSinceLastShot.text); 


		//type equation in parenthesis
		OUTPUT.text = (0.9f - (.5f * isTouchingGround) - (.1f * aimDownSights) - (1f - System.Math.Pow(.977f, movementSpeed)) + ((.1f) / (1f + (30f * (System.Math.Pow(1/30f, timeSinceLastShot)))))).ToString();
	}
}
