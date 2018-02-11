using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour {
	public AudioSource spotLightSound;
	public float highVal = 1.3f;
	public float goalVal;
	public MeshRenderer[] servers;
	public MeshRenderer laser;
	public MeshRenderer laser2;
	public MeshRenderer[] laserParts;
	public MeshRenderer floor;
	public LineRenderer beam;
	// Use this for initialization
	void Start () {
		goalVal = 0;
		beam.enabled = false;
	}

	private bool ready = false;
	private bool bootedUp = false;
	private float animSpeed = 1;

	// Update is called once per frame
	void Update () {
		if (!ready) {
			return;
		}
			
		if (goalVal == highVal) {
			animSpeed = .25f;
		} else {
			animSpeed = 2f;
		}
		laser.materials [2].SetColor ("_EmissionColor", new Color(Mathf.Lerp(laser.materials[2].GetColor("_EmissionColor").r,goalVal, Time.deltaTime * animSpeed),0,0));
		laser2.materials [1].SetColor ("_EmissionColor", new Color(Mathf.Lerp(laser2.materials[1].GetColor("_EmissionColor").r,goalVal, Time.deltaTime * animSpeed),0,0));
		foreach (MeshRenderer mesh in laserParts) {
			mesh.materials [0].SetColor ("_EmissionColor", new Color(Mathf.Lerp(mesh.materials[0].GetColor("_EmissionColor").r,goalVal, Time.deltaTime * animSpeed),0,0));
		}

		floor.materials [0].SetColor ("_EmissionColor", new Color(0,0,Mathf.Lerp(floor.materials[0].GetColor("_EmissionColor").b,goalVal, Time.deltaTime * animSpeed)));

		foreach (MeshRenderer mesh in servers) {
			mesh.transform.parent.GetComponentInChildren<Light> ().intensity = Mathf.Lerp (mesh.transform.parent.GetComponentInChildren<Light> ().intensity, 5.8f*(goalVal / highVal), Time.deltaTime);
			mesh.materials [2].SetColor ("_EmissionColor", new Color(0,0,Mathf.Lerp(mesh.materials[2].GetColor("_EmissionColor").b,goalVal, Time.deltaTime * animSpeed)));
			mesh.materials [3].SetColor ("_EmissionColor", new Color(0,0,Mathf.Lerp(mesh.materials[3].GetColor("_EmissionColor").b,goalVal, Time.deltaTime * animSpeed)));
			mesh.materials [4].SetColor ("_EmissionColor", new Color(Mathf.Lerp(mesh.materials[4].GetColor("_EmissionColor").r,goalVal, Time.deltaTime * animSpeed),0,0));
		}
		Debug.Log (floor.materials [0].GetColor ("_EmissionColor").b);
		if (floor.materials [0].GetColor ("_EmissionColor").b > (highVal * .6f)) {
			bootedUp = true;
		}

	}


	public void playSpotlight(){
		spotLightSound.Play ();
	}

	public void activate(){
		goalVal = highVal;
	}

	public void deactivate(){
		goalVal = 0;
	}

	public void readyUp(){
		ready = true;
	}

	public void playButton(){
		Debug.Log ("BEAM ENABLED");
		if (bootedUp) {

			beam.enabled = true;
		}
	}
}
