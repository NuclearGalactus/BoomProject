using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public enum GunType {
        FULL,
        SEMI,
        LONG
    }

    public GunType type = GunType.LONG;
    public float damage = 60;
	public int clipSize = 7;
    public int RPM = 3600;
    //public Transform MainBone;
    //public Transform ParentBone;
    //public Animator animator;
    public SkinnedMeshRenderer mainRenderer;


    public AudioClip fire;
	public AudioClip outOfAmmo;
	public AudioClip action;
	public AudioClip boltOpen;
	public AudioClip boltClose;
	public AudioClip singleBullet;


    public Vector3 delta;
    public Vector3 rotation;
	// Use this for initialization
	void Start () {
        //MainBone.SetParent(ParentBone);
        //animator = GetComponent<Animator>();
    }

    
    // Update is called once per frame
    void LateUpdate () {
       // MainBone.localPosition = delta;
        //MainBone.localRotation = Quaternion.Euler(rotation);
    }

    public void MeleeAttack()
    {
    }


	public void ReadyUp(){
	}
}
