using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {
    private Grid grid;      // Grid manager object
    public GameObject item; // Item occupying this square
    private string item_name;
    private int x_pos;      // x coordinate of square
    private int y_pos;      // y coordinate of square
    private char facing;
    private List<GridSquare> triggers;
    private int range;
    private PartyMovement party;
    public Camera cam;
    private GameObject enemy;
    public bool isTrigger;
    public bool triggered;
    public bool running;

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
        cam = Camera.main;
        isTrigger = false;
        triggered = false;
        running = false;

        if (x_pos == 0 && y_pos == 0)
        {
            item = Instantiate<GameObject>(grid.party);
            item.transform.localScale = new Vector3(8f, 8f, 8f);
            item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z);
            item.gameObject.AddComponent<PartyMovement>();
        }
    }

    private void OnMouseExit()
    {
        if (x_pos != 10 || y_pos != 19) {
            if (isTrigger) {
                this.GetComponent<Renderer>().material.color = Color.yellow;
            } else {
            this.GetComponent<Renderer>().material.color = Color.green;
            }
        } else {
            this.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    // Highlights grid square, and allows placement of object
    private void OnMouseOver()
    {
        if (grid.getPause() || running) { return; }

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
            if (item_name != "empty" && grid.GetSelection() != 'r')
            {
                return;
            }

            // Places object based on current selection
            switch (grid.GetSelection()) {
                case '1' :
                    item = Instantiate<GameObject>(grid.wall);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                    item.transform.localScale = new Vector3(4.25f, 4.25f, 4.25f);
                    facing = 'w';
                    item_name = "wall";
                    break;
                case '2' :
                    if (!(grid.spendGold(100))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.pit);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.67f, this.transform.position.z);
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    item_name = "pit";
                    break;
                case '3':
                    if (!(grid.spendGold(200))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.crushing_wall);
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z - 0.25f);
                    facing = 's';
                    item_name = "crushing wall";
                    range = 2;
                    FindTriggerSquares(false);
                    break;
                case '4':
                    if (!(grid.spendGold(100))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.spikes);
                    item.transform.position = this.transform.position;
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    item_name = "spikes";
                    break;
                case '5':
                    if (!(grid.spendGold(400))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.boulder);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    facing = 's';
                    item_name = "boulder";
                    range = 4;
                    FindTriggerSquares(false);
                    break;
                case '6':
                    if (!(grid.spendGold(200))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.arrow_wall);
                    item.transform.position = this.transform.position;
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    facing = 'n';
                    item_name = "arrow wall";
                    range = 3;
                    FindTriggerSquares(false);
                    break;
                case '9':
                    // TO-DO: Code for deleting objects
                    Destroy(item.gameObject);
                    item = null;
                    item_name = "empty";
                    range = 0;
                    FindTriggerSquares(false);
                    break;
                case 'e':
                    if (!(grid.spendGold(grid.enemy.GetComponent<EnemyStats>().GetCost()))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.enemy);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                    item.transform.localScale = new Vector3(6f, 6f, 6f);
                    item_name = "enemy";
                    range = 1;
                    facing = 'n';
                    FindTriggerSquares(true);
                    break;
                case 'r':
                    print("rotating");
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
                    item = Instantiate<GameObject>(grid.party);
                    item.transform.localScale = new Vector3(8f, 8f, 8f);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z);
                    item.gameObject.AddComponent<PartyMovement>();
                    //item_name = "party";
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            cam.transform.position = new Vector3(this.transform.position.x + 2f, this.transform.position.y + 5f, this.transform.position.z - 2.5f);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            running = true;
        }

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
                        triggered = true;
                        party.damaged = true;
                        party.fighting = true;
                        if (!party.NoticeCheck(this)) {
                            party.takeDamage(item.GetComponent<TrapStats>().GetDamage());
                            party.GetComponent<Renderer>().material.color = Color.red;
                            GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + party.getHealth();
                        } else if (!party.AvoidCheck(this)) {
                            party.takeDamage(item.GetComponent<TrapStats>().GetDamage());
                            party.GetComponent<Renderer>().material.color = Color.red;
                            GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + party.getHealth();
                        } else {
                            party.GetComponent<Renderer>().material.color = Color.green;
                        }
                    } else {
                        party.damaged = true;
                        StartCoroutine(party.Fight(this));
                    }
                }
            }
            if (triggered) {
                if (!grid.firstTrap) {
                    GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().FirstTrapDialogue();
                    grid.firstTrap = true;
                }
                foreach(var trigger in triggers)
                {
                    trigger.GetComponent<Renderer>().material.color = Color.green;
                    trigger.isTrigger = false;
                }
                triggers.Clear();
            }
        }
        if (item_name == "enemy") {
            FindTriggerSquares(true);
        } else if (item_name != "empty" && !triggered) {
            FindTriggerSquares(false);
        }
    }

    // Rotates item in square
    private void rotateSquare()
    {
        if (item != null) {
            if (item_name == "enemy")
            {
                FindTriggerSquares(true);
            }
            else
            {
                FindTriggerSquares(false);
            }
            item.transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    private void FindTriggerSquares(bool omnidirectional)
    {
        print("Finding triggers");
        foreach (var trigger in triggers)
        {
            trigger.GetComponent<Renderer>().material.color = Color.green;
            trigger.isTrigger = false;
        }
        triggers.Clear();
        for (int i = 1; i <= range; i++)
        {
            GridSquare square = null;
            if (facing == 'n' || omnidirectional) {
                square = grid.squares[x_pos, y_pos + i];
                if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                {
                    square.GetComponent<Renderer>().material.color = Color.yellow;
                    triggers.Add(square);
                    square.isTrigger = true;
                }
            }
            if (facing == 'e' || omnidirectional) {
                square = grid.squares[x_pos + i, y_pos];
                if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                {
                    square.GetComponent<Renderer>().material.color = Color.yellow;
                    triggers.Add(square);
                    square.isTrigger = true;
                }
            }
            if (facing == 's' || omnidirectional) {
                square = grid.squares[x_pos, y_pos - i];
                if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                {
                    square.GetComponent<Renderer>().material.color = Color.yellow;
                    triggers.Add(square);
                    square.isTrigger = true;
                }
            }
            if (facing == 'w' || omnidirectional) {
                square = grid.squares[x_pos - i, y_pos];
                if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                {
                    square.GetComponent<Renderer>().material.color = Color.yellow;
                    triggers.Add(square);
                    square.isTrigger = true;
                }
            }

            /*if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes") {
                square.GetComponent<Renderer>().material.color = Color.yellow;
                triggers.Add(square);
            } else {
                break;
            }*/
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
