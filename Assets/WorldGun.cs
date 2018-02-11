using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGun : Interactable {
    public Material highlight;
	public string resourceName;
    public int index;

	private Material[] oldMats;
	private Material[] highlights;
	private Renderer renderer;
	// Use this for initialization
	public void Start(){
		renderer = GetComponent<Renderer> ();
		highlights = new Material[renderer.materials.Length];
		for (int i = 0; i < highlights.Length; i++) {
			highlights [i] = highlight;
		}
		oldMats = renderer.materials;
		renderer.materials [0] = highlight;
	}

    public override void interact(Interactor inter)
    {
		Debug.Log ("jeff");
        inter.equipGun(index);
        Destroy(gameObject);
    }


    public override void found(bool f)
    {
		
		if (f) {
			renderer.materials = highlights;
		} else {
			renderer.materials= oldMats;
		}
    }
}
