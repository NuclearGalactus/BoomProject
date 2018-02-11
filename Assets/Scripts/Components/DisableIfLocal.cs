using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfLocal : MonoBehaviour {
	public bool isLocal = true;
	void Update()
	{
		if (isLocal) {
			gameObject.SetActive (false);
			isLocal = false;
		}

	}

}
