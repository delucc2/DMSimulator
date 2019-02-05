using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {
    private Grid grid;

    public void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    // Highlights grid square, and allows placement of object
    private void OnMouseOver()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        if (Input.GetMouseButtonDown(0))
        {
            switch (grid.GetSelection()) {
                case '1' :
                    Transform wall = Instantiate(grid.wall, this.transform);
                    wall.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
                    break;
                case '2' :
                    break;
                case '3':
                    break;
                case '4':
                    break;
                case '5':
                    break;
                case '6':
                    break;
                case '9':
                    break;
            }
        }
    }
}
