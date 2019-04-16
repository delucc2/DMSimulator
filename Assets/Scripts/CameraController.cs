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
            if (transform.position.z > 16f && Input.GetAxis("Vertical") > 0) {
                return;
            } else if (transform.position.z < 1f && Input.GetAxis("Vertical") < 0) {
                return;
            }
            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * 0.25f);
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            if (transform.position.x < 5f && Input.GetAxis("Horizontal") < 0) {
                return;
            } else if (transform.position.x > 15f && Input.GetAxis("Horizontal") > 0) {
                return;
            }
            transform.Translate(this.transform.right * Input.GetAxis("Horizontal") * 0.25f);
        }

        if(Input.GetKey(KeyCode.E))
        {
            if (transform.position.y > 43f) {
                return;
            }
            transform.Translate(-Vector3.forward * 0.5f);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            if (transform.position.y < 16f) {
                return;
            }
            transform.Translate(Vector3.forward * 0.5f);
        }
	}
}
