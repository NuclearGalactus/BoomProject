using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour {
	public Text textLabel;
	public RectTransform canvas;
	private int layermask = 1 << 11;
    private GunController gunc;
	// Use this for initialization
	void Start () {
        gunc = GetComponentInParent<GunController>();
	}
    Interactable interacted;
	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		if (Physics.Raycast (new Ray (transform.position, transform.forward), 
            out hitInfo, 3f, layermask)) {
			Interactable interactable = hitInfo.collider.GetComponent<Interactable> ();
		//	textLabel.enabled = interactable != null;
			if (interactable != null) {
                if (interacted == null)
                {
                    interacted = interactable;
                    interactable.found(true);
                }
				//textLabel.text = interactable.interactText;
			//	Vector2 pos;
				//RectTransformUtility.ScreenPointToLocalPointInRectangle (canvas, Camera.main.WorldToScreenPoint 
              //      (interactable.transform.position), null, out pos);
				//textLabel.rectTransform.position = Camera.main.WorldToScreenPoint 
              //      (interactable.transform.position);
				if (Input.GetKeyDown (KeyCode.F)) {
					interactable.interact(this);
				}
            }
            else if(interacted != null)
            {
                interacted.found(false);
                interacted = null;
            }
		} else if(interacted != null){
            interacted.found(false);
            interacted = null;
			//textLabel.enabled = false;
		}
	}


    public void equipGun(int ind)
    {
        gunc.equip(ind);
    }
}
