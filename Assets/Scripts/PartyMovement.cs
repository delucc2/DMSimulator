using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour {
    private float x_pos;
    private float z_pos;
    private float y_pos;
    private GridSquare curr_pos;
    private Grid grid;

	// Use this for initialization
	void Start () {
        x_pos = this.transform.position.x;
        z_pos = this.transform.position.z;
        y_pos = this.transform.position.y;
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        curr_pos = grid.squares[(int)x_pos, (int)z_pos].GetComponent<GridSquare>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
            z_pos += 1f;
            if (!Move('n')) { z_pos -= 1f; }
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            z_pos -= 1f;
            if (!Move('s')) { z_pos += 1f; }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            x_pos -= 1f;
            if (!Move('w')) { x_pos += 1f; }
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            x_pos += 1f;
            if (!Move('e')) { x_pos -= 1f; }
        }
	}

    private bool Move(char direction)
    {
        GridSquare new_pos = grid.squares[(int)x_pos, (int)z_pos].GetComponent<GridSquare>();
        if (new_pos.getItem() == "wall" || new_pos.getItem() == "crushing wall" || new_pos.getItem() == "arrow wall")
        {
            if ((new_pos.getFacing() == 'e' && direction == 'w') ||
                (new_pos.getFacing() == 'w' && direction == 'e') ||
                (new_pos.getFacing() == 'n' && direction == 's') ||
                (new_pos.getFacing() == 's' && direction == 'n')) {
                return false;
            }
        }

        if (curr_pos.getItem() == "wall" || curr_pos.getItem() == "crushing wall" || curr_pos.getItem() == "arrow_wall")
        {
            if (curr_pos.getFacing() == direction) { return false; }
        }

        this.transform.position = new Vector3(x_pos, y_pos, z_pos);
        curr_pos = new_pos;
        return true;
    }
}
