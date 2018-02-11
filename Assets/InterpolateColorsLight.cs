using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolateColorsLight : MonoBehaviour {
    public Color c1;
    public Color c2;
    public float speed;
    Light l;
    Color i;
    // Use this for initialization
    void Start () {
        i = c1;
        l = GetComponent<Light>();
        l.color = c1;
	}
    
	// Update is called once per frame
	void Update () {
      //  l.color = new Color(Mathf.Lerp(l.color.r, i.r, Time.deltaTime * speed),
     //       Mathf.Lerp(l.color.g, i.g, Time.deltaTime * speed),
    //        Mathf.Lerp(l.color.b, i.b, Time.deltaTime * speed));
		l.color = Color.Lerp(c1, c2, Mathf.PingPong(Time.time * speed, 1));
        if (l.color == c1)
         {
            i = c2;
         }else if(l.color == c2)
        {
            i = c1;
        }
	}
}
