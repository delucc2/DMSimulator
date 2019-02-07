using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour {
    private float x_pos;
    private float z_pos;
    private float y_pos;

	// Use this for initialization
	void Start () {
        x_pos = this.transform.position.x;
        z_pos = this.transform.position.z;
        y_pos = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
            z_pos += 1f;
            Move();
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            z_pos -= 1f;
            Move();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            x_pos -= 1f;
            Move();
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            x_pos += 1f;
            Move();
        }
	}

    private void Move()
    {
        this.transform.position = new Vector3(x_pos, y_pos, z_pos);
    }
}
