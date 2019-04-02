using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCamController : MonoBehaviour {
    private GameObject party;
    public Vector3 offset;

    float distance;
    Vector3 playerPrevPos, playerMoveDir;

    // Use this for initialization
    void Start() { 

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.gameObject.GetComponent<Camera>().enabled = true;
        }

        if (party == null) {
            party = GameObject.Find("Party(Clone)");
            if (party != null) {
                distance = offset.magnitude;
                playerPrevPos = party.transform.position;
            }
        } else {
            playerMoveDir = party.transform.position - playerPrevPos;
            if (playerMoveDir != Vector3.zero)
            {
                playerMoveDir.Normalize();
                Vector3 smooth_position = Vector3.Lerp(transform.position, party.transform.position - playerMoveDir * distance, 0.125f);
                transform.position = smooth_position;
                //transform.Translate(party.transform.position - playerMoveDir * distance);

                transform.position = new Vector3(transform.position.x, transform.position.y + 0.125f, transform.position.z);

                transform.LookAt(party.transform.position);

                playerPrevPos = party.transform.position;
            }
        }
	}
}
