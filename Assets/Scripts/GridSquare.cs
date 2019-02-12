using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {
    private Grid grid;      // Grid manager object
    public Transform item; // Item occupying this square
    private string item_name;
    private int x_pos;      // x coordinate of square
    private int y_pos;      // y coordinate of square
    private char facing;
    private List<GridSquare> triggers;
    private int range;
    private PartyMovement party;

    public void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        x_pos = (int)this.transform.position.x;
        y_pos = (int)this.transform.position.z;
        triggers = new List<GridSquare>();
        item = null;
        item_name = "empty";
        facing = '0';
        range = 0;
    }

    private void OnMouseExit()
    {
        if (x_pos != 10 || y_pos != 19) {
            this.GetComponent<Renderer>().material.color = Color.green;
        } else {
            this.GetComponent<Renderer>().material.color = Color.blue;
        }
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
                    item_name = "wall";
                    break;
                case '2' :
                    item = Instantiate(grid.pit, this.transform);
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    item_name = "pit";
                    break;
                case '3':
                    item = Instantiate(grid.crushing_wall, this.transform);
                    item.transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1f);
                    facing = 's';
                    item_name = "crushing wall";
                    range = 2;
                    FindTriggerSquares(true);
                    break;
                case '4':
                    item = Instantiate(grid.spikes, this.transform);
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    item_name = "spikes";
                    break;
                case '5':
                    item = Instantiate(grid.boulder, this.transform);
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    facing = 's';
                    item_name = "boulder";
                    range = 4;
                    FindTriggerSquares(true);
                    break;
                case '6':
                    item = Instantiate(grid.arrow_wall, this.transform);
                    item.transform.localScale = new Vector3(0.57f, 0.57f, 0.57f);
                    facing = 'n';
                    item_name = "arrow wall";
                    range = 3;
                    FindTriggerSquares(true);
                    break;
                case '9':
                    // TO-DO: Code for deleting objects
                    break;
                case 'e':
                    // TO-DO: Enemy detection
                    item = Instantiate(grid.generic_enemy, this.transform);
                    item_name = "enemy";
                    range = 1;
                    facing = 'n';
                    FindTriggerSquares(true);
                    facing = 's';
                    FindTriggerSquares(false);
                    facing = 'e';
                    FindTriggerSquares(false);
                    facing = 'w';
                    FindTriggerSquares(false);
                    break;
                case 'r':
                    if (facing == 'n') {
                        facing = 'e';
                    } else if (facing == 'e') {
                        facing = 's';
                    } else if (facing == 's') {
                        facing = 'w';
                    } else if (facing == 'w') {
                        facing = 'n';
                    }
                    rotateSquare();
                    break;
                case '7':
                    //if (GameObject.FindGameObjectWithTag("party") != null) { Destroy(GameObject.FindGameObjectWithTag("party")); }
                    item = Instantiate(grid.party, this.transform);
                    item.transform.localScale = new Vector3(8f, 8f, 8f);
                    item.transform.position = new Vector3(item.transform.position.x - 0.3f, item.transform.position.y + 0.65f, item.transform.position.z - 0.7f);
                    item.gameObject.AddComponent<PartyMovement>();
                    //item_name = "party";
                    break;
            }
        }
    }

    public void Update()
    {
        if (party == null) {
            party = GameObject.Find("Party(Clone)").GetComponent<PartyMovement>();
        } else {
            int party_x = 0;
            int party_y = 0;
            party.GetPos(ref party_x, ref party_y);
            foreach (var square in triggers)
            {
                //Debug.Log(party_x + "," + party_y + " | " + square.x_pos + "," + square.y_pos);
                if (square.x_pos == party_x && square.y_pos == party_y && !party.damaged) {
                    if (item_name != "enemy") {
                        party.damaged = true;
                        if (!party.NoticeCheck(this)) {
                            party.takeDamage(item.GetComponent<TrapStats>().GetDamage());
                            party.GetComponent<Renderer>().material.color = Color.red;
                        } else if (!party.AvoidCheck(this)) {
                            party.takeDamage(item.GetComponent<TrapStats>().GetDamage());
                            party.GetComponent<Renderer>().material.color = Color.red;
                        } else {
                            party.GetComponent<Renderer>().material.color = Color.green;
                        }
                    } else {
                        party.damaged = true;
                        StartCoroutine(party.Fight(this));
                    }
                }
            }
        }
        if (item_name == "enemy") {
            facing = 'n';
            FindTriggerSquares(true);
            facing = 's';
            FindTriggerSquares(false);
            facing = 'e';
            FindTriggerSquares(false);
            facing = 'w';
            FindTriggerSquares(false);
        } else if (item_name != "empty") { FindTriggerSquares(true); }
    }

    // Rotates item in square
    private void rotateSquare()
    {
        if (item != null) {
            FindTriggerSquares(true);
            this.transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    private void FindTriggerSquares(bool clear)
    {
        if (clear) { triggers.Clear(); }
        for (int i = 1; i <= range; i++)
        {
            GridSquare square = null;
            if (facing == 'n') {
                square = grid.squares[x_pos, y_pos + i];
            } else if (facing == 'e') {
                square = grid.squares[x_pos + i, y_pos];
            } else if (facing == 's') {
                square = grid.squares[x_pos, y_pos - i];
            } else if (facing == 'w') {
                square = grid.squares[x_pos - i, y_pos];
            }

            if (square.getItem() == "wall" || square.getItem() == "arrow wall")
            {
                if ((facing == 'n' && square.getFacing() == 's') ||
                    (facing == 'e' && square.getFacing() == 'w') ||
                    (facing == 's' && square.getFacing() == 'n') ||
                    (facing == 'w' && square.getFacing() == 'e')) {
                    break;
                } else if (facing == square.getFacing()) {
                    square.GetComponent<Renderer>().material.color = Color.yellow;
                    triggers.Add(square);
                    break;
                } else {
                    square.GetComponent<Renderer>().material.color = Color.yellow;
                    triggers.Add(square);
                }
               
            } else if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes") {
                square.GetComponent<Renderer>().material.color = Color.yellow;
                triggers.Add(square);
            } else {
                break;
            }
        }
    }

    public string getItem()
    {
        return item_name;
    }

    public char getFacing()
    {
        return facing;
    }

    public void addPlayer()
    {
        item = Instantiate(grid.party, this.transform);
        item.transform.localScale = new Vector3(8f, 8f, 8f);
        item.transform.position = new Vector3(item.transform.position.x - 0.3f, item.transform.position.y + 0.65f, item.transform.position.z - 0.7f);
        item.gameObject.AddComponent<PartyMovement>();
    }

    public void GetPos(ref int x, ref int y)
    {
        x = x_pos;
        y = y_pos;
    }

    public void resetSquare()
    {
        item = null;
        item_name = "empty";
        facing = '0';
        triggers.Clear();
        range = 0;
    }
}
