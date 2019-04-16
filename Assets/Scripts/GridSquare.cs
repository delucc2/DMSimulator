using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {
    private Grid grid;      // Grid manager object
    public GameObject item; // Item occupying this square
    public string item_name;
    private int x_pos;      // x coordinate of square
    private int y_pos;      // y coordinate of square
    public char facing;
    private List<GridSquare> triggers;
    public int range;
    private PartyMovement party;
    public Camera cam;
    private GameObject enemy;
    public bool isTrigger;
    public bool triggered;
    public bool running;
    public bool deletable;

    public void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        x_pos = (int)this.transform.position.x;
        y_pos = (int)this.transform.position.z;
        triggers = new List<GridSquare>();
        item = null;
        if (item_name == null) { item_name = "empty"; }
        facing = '0';
        range = 0;
        cam = Camera.main;
        isTrigger = false;
        triggered = false;
        running = false;
        deletable = true;

        if (x_pos == grid.start_x && y_pos == grid.start_y)
        {
            item = Instantiate<GameObject>(grid.party);
            item.transform.localScale = new Vector3(2f, 2f, 2f);
            item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            item.gameObject.AddComponent<PartyMovement>();
        }
        else if (x_pos == grid.boss_x && y_pos == grid.boss_y)
        {
            item = Instantiate<GameObject>(grid.boss);
            item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
            item.transform.localScale = new Vector3(6f, 6f, 6f);
            item_name = "enemy";
            if (grid.boss.GetComponent<EnemyStats>().GetRanged())
            {
                range = 3;
            }
            else
            {
                range = 1;
            }
            facing = 'n';
            FindTriggerSquares(true);
        }
    }

    private void OnMouseExit()
    {
        if (x_pos != grid.end_x || (y_pos + 1) != grid.end_y) {
            if (isTrigger) {
                this.GetComponent<Renderer>().material.color = Color.yellow;
            } else if (!running) {
            this.GetComponent<Renderer>().material.color = Color.green;
            }
        } else {
            this.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    // Highlights grid square, and allows placement of object
    private void OnMouseOver()
    {
        int party_x = -1;
        int party_y = -1;
        if (party != null) {
            party.GetPos(ref party_x, ref party_y);
        }
        if (grid.getPause() || running || (x_pos == party_x && y_pos == party_y)) { return; }

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
            if (item_name != "empty" && grid.GetSelection() != 'r' && grid.GetSelection() != '9') {
                print(grid.GetSelection());
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
                case '9':
                    if (!deletable) {
                        break;
                    }

                    if (item_name == "enemy") {
                        grid.refund(item.gameObject.GetComponent<EnemyStats>().GetCost());
                    } else {
                        grid.refund(item.gameObject.GetComponent<TrapStats>().cost);
                    }
                    resetSquare();
                    break;
                case 'e':
                    if (!(grid.spendGold(grid.enemy.GetComponent<EnemyStats>().GetCost()))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.enemy);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    item_name = "enemy";
                    if (grid.enemy.GetComponent<EnemyStats>().GetRanged()) {
                        range = 3;
                    } else {
                        range = 1;
                    }
                    facing = 'n';
                    FindTriggerSquares(true);
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
                    item = Instantiate<GameObject>(grid.party);
                    item.transform.localScale = new Vector3(8f, 8f, 8f);
                    item.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z);
                    item.gameObject.AddComponent<PartyMovement>();
                    //item_name = "party";
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if(!deletable) {
                return;
            }

            if (item_name == "enemy")
            {
                grid.refund(item.gameObject.GetComponent<EnemyStats>().GetCost());
            }
            else
            {
                grid.refund(item.gameObject.GetComponent<TrapStats>().cost);
            }
            resetSquare();
        }
    }

    public void partyStop()
    {
        party.rb.velocity = new Vector3(0, 0, 0);
        if (item_name == "enemy")
        {
            item.gameObject.transform.LookAt(party.gameObject.transform);
            party.prev_facing = 180 * party.gameObject.transform.forward;
            party.gameObject.transform.LookAt(item.gameObject.transform);
        } else
        {
            for (int i = 0; i < 4; i++)
            {
                party.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("stop");
                if (i == 2 || i == 0 || i == 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        party.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("stop");
                    }
                }
                else if (i == 1)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        party.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("stop");
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !running) {
            if (party.Pathfind(grid.end_x, 19 - grid.end_y) == null) { return; }
            foreach (var square in grid.squares)
            {
                square.GetComponent<Renderer>().material = grid.floor_material;
            }
            running = true;
        }

        if (item_name == "enemy") {
            FindTriggerSquares(true);
        } else if (item_name != "empty" && !triggered) {
            FindTriggerSquares(false);
        }

        if (party == null) {
            party = GameObject.Find("Party(Clone)").GetComponent<PartyMovement>();
        } else {
            int party_x = 0;
            int party_y = 0;
            party.GetPos(ref party_x, ref party_y);
            foreach (var square in triggers)
            {
                //print(running);
                //Debug.Log(party_x + "," + party_y + " | " + square.x_pos + "," + square.y_pos);
                if (square.x_pos == party_x && square.y_pos == party_y && !party.damaged && running) {
                    //print("Triggered");
                    if (item_name != "enemy") {
                        triggered = true;
                        party.running = false;
                        party.damaged = true;
                        party.fighting = true;
                        Invoke("partyStop", 0.5f);
                        SFXHandler sfx = GameObject.Find("SFX Source").GetComponent<SFXHandler>();
                        if (!party.NoticeCheck(this)) {
                            party.takeDamage(item.GetComponent<TrapStats>().GetDamage());
                            party.LogPrint("> The party takes " + item.GetComponent<TrapStats>().GetDamage() + " damage!\n");
                            sfx.playSound(item.GetComponent<TrapStats>().GetFailureSound());
                            party.GetComponent<Renderer>().material.color = Color.red;
                            GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + party.getHealth();
                        } else if (!party.AvoidCheck(this)) {
                            party.takeDamage(item.GetComponent<TrapStats>().GetDamage());
                            party.LogPrint("> The party takes " + item.GetComponent<TrapStats>().GetDamage() + " damage!\n");
                            sfx.playSound(item.GetComponent<TrapStats>().GetFailureSound());
                            party.GetComponent<Renderer>().material.color = Color.red;
                            GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + party.getHealth();
                        } else {
                            sfx.playSound(item.GetComponent<TrapStats>().GetSuccessSound());
                            //party.GetComponent<Renderer>().material.color = Color.green;
                        }
                    } else {
                        party.damaged = true;
                        Invoke("partyStop", 0.5f);
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
    public void rotateSquare()
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

    public void FindTriggerSquares(bool omnidirectional)
    {
        foreach (var trigger in triggers)
        {
            if (trigger.x_pos != grid.end_x && trigger.y_pos != grid.end_y && !running) { trigger.GetComponent<Renderer>().material.color = Color.green; }
            trigger.isTrigger = false;
        }
        triggers.Clear();
        bool skip_n = false;
        bool skip_e = false;
        bool skip_w = false;
        bool skip_s = false;
        for (int i = 1; i <= range; i++)
        {
            GridSquare square = null;
            if (facing == 'n' || (omnidirectional && !skip_n)) {
                if (y_pos + i > 19)
                {
                    skip_n = true;
                    if (!omnidirectional) { break; }
                }
                else
                {
                    square = grid.squares[x_pos, y_pos + i];
                    if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                    {
                        if (square.x_pos != grid.end_x || square.y_pos != grid.end_y) { square.GetComponent<Renderer>().material.color = Color.yellow; }
                        triggers.Add(square);
                        square.isTrigger = true;
                    }
                    else
                    {
                        skip_n = true;
                        if (!omnidirectional) { break; }
                    }
                }
            }
            if (facing == 'e' || (omnidirectional && !skip_e)) {
                if (x_pos + i > 19)
                {
                    skip_e = true;
                    if (!omnidirectional) { break; }
                }
                else
                {
                    square = grid.squares[x_pos + i, y_pos];
                    if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                    {
                        if (square.x_pos != grid.end_x || square.y_pos != grid.end_y) { square.GetComponent<Renderer>().material.color = Color.yellow; }
                        triggers.Add(square);
                        square.isTrigger = true;
                    }
                    else
                    {
                        skip_e = true;
                        if (!omnidirectional) { break; }
                    }
                }
            }
            if (facing == 's' || (omnidirectional && !skip_s)) {
                if (y_pos - i < 0)
                {
                    skip_s = true;
                    if (!omnidirectional) { break; }
                }
                else
                {
                    square = grid.squares[x_pos, y_pos - i];
                    if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                    {
                        if (square.x_pos != grid.end_x || square.y_pos != grid.end_y) { square.GetComponent<Renderer>().material.color = Color.yellow; }
                        triggers.Add(square);
                        square.isTrigger = true;
                    }
                    else
                    {
                        skip_s = true;
                        if (!omnidirectional) { break; }
                    }
                }
            }
            if (facing == 'w' || (omnidirectional && !skip_w)) {
                if (x_pos - i < 0)
                {
                    skip_w = true;
                    if (!omnidirectional) { break; }
                }
                else
                {
                    square = grid.squares[x_pos - i, y_pos];
                    if (square.getItem() == "empty" || square.getItem() == "pit" || square.getItem() == "spikes")
                    {
                        if (square.x_pos != grid.end_x || square.y_pos != grid.end_y) { square.GetComponent<Renderer>().material.color = Color.yellow; }
                        triggers.Add(square);
                        square.isTrigger = true;
                    }
                    else
                    {
                        skip_w = true;
                        if (!omnidirectional) { break; }
                    }
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
        item_name = "empty";
        facing = '0';
        foreach (var trigger in triggers)
        {
            trigger.GetComponent<Renderer>().material.color = Color.green;
            trigger.isTrigger = false;
        }
        triggers.Clear();
        range = 0;
        if (item != null)
        {
            Destroy(item.gameObject);
        }
        item = null;
    }
}
