using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LookController : NetworkBehaviour
{

	public float sensitivity = 1;
	public Slider SenseSlider;
    public float minX = -360F;
    public float maxX = 360F;
    public float minY = -60F;
    public float maxY = 60F;
	public Transform Head;
    public float rotationY = 0F;
	//public float RotationX = 0F;
	public float rotationX = 0F;


	void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
           
            rotationY += Input.GetAxis("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
			Head.localRotation = Quaternion.Euler(new Vector3 (-rotationY, 0, 0));
         

    }

    void Start()
	{
        if (!isLocalPlayer)
        {
            this.enabled = false;
        }
        Cursor.lockState = CursorLockMode.Locked;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
		//SenseSlider = GameObject.FindGameObjectWithTag ("Slider UI").GetComponent<Slider> ();
		//SenseSlider.onValueChanged.AddListener (SensitivityFunc);
    }
		
	public void SensitivityFunc(float val)
	{
		sensitivity = val;
	}
}