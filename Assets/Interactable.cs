using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public bool interactable = false;
	public virtual void interact(Interactor inter){
		
	}
	void Start(){
	}
	void Update(){
		
	}
    

    

    public virtual void found(bool f)
    {

    }

	public virtual void shutdown(){
		
	}


}
