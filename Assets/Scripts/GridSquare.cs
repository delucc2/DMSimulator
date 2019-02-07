using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {
    private Grid grid;      // Grid manager object
    private Transform item; // Item occupying this square
    private int x_pos;      // x coordinate of square
    private int y_pos;      // y coordinate of square
    private char facing;
    private bool[,] activate;

    public void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        x_pos = (int)this.transform.position.x;
        y_pos = (int)this.transform.position.z;
        item = null;
        facing = '0';
        activate = new bool[grid.GetXSize(), grid.GetYSize()];
    }

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    // Highlights grid square, and allows placement of object
    private void OnMouseOver()
    {
        // Change grid color to red
        this.GetComponent<Renderer>().material.color = Color.red;

        // TO-DO: Display preview orientation for selection

        // Place selected item
        /* Selection Key
         * --------------
         * 0 - Nothhing
         * 1 - Wall
         * 2 - Pit
         * 3 - Crushing Wall
         * 4 - Spikes
         * 5 - Boulder
         * 6 - Arrow Wall
         * e - Generic Enemy
         * 9 - Delete
         */
        if (Input.GetMouseButtonDown(0))
        {
            // Places object based on current selection
            switch (grid.GetSelection()) {
                case '1' :
                    item = Instantiate(grid.wall, this.transform);
                    item.transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                    facing = 'w';
                    break;
                case '2' :
                    item = Instantiate(grid.pit, this.transform);
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case '3':
                    item = Instantiate(grid.crushing_wall, this.transform);
                    item.transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                    facing = 's';
                    break;
                case '4':
                    item = Instantiate(grid.spikes, this.transform);
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case '5':
                    item = Instantiate(grid.boulder, this.transform);
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    facing = 's';
                    break;
                case '6':
                    item = Instantiate(grid.arrow_wall, this.transform);
                    item.transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                    facing = 'n';
                    break;
                case '9':
                    // TO-DO: Code for deleting objects
                    break;
                case 'e':
                    // TO-DO: Enemy detection
                    item = Instantiate(grid.generic_enemy, this.transform);
                    item.transform.localScale = new Vector3(8f, 8f, 8f);
                    item.transform.position = new Vector3(item.transform.position.x - 0.72f, item.transform.position.y, item.transform.position.z - 0.15f);
                    break;
                case 'r':
                    rotateSquare();
                    break;
                case '7':
                    item = Instantiate(grid.party, this.transform);
                    item.transform.localScale = new Vector3(8f, 8f, 8f);
                    item.transform.position = new Vector3(item.transform.position.x - 0.3f, item.transform.position.y + 0.65f, item.transform.position.z - 0.7f);
                    item.gameObject.AddComponent<PartyMovement>();
                    break;
            }
        }
    }

    // Rotates item in square
    private void rotateSquare()
    {
        if (item != null) {
            this.transform.Rotate(new Vector3(0, 90, 0));
        }
    }
}
