using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(Vector3.up * Input.GetAxis("Vertical"));
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(this.transform.right * Input.GetAxis("Horizontal"));
        }

        if(Input.GetKey(KeyCode.E))
        {
            transform.Translate(-Vector3.forward * 0.5f);
        } else if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.forward * 0.5f);
        }
	}
}
