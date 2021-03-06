﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour {
    private float x_pos;
    private float z_pos;
    private float y_pos;
    private GridSquare curr_pos;
    private Grid grid;
    private bool found;
    public bool damaged;
    public bool fighting;
    private bool levelComplete;
    public bool running;
    private Camera camera;
    private char facing;
    private bool blocked;
    private string[] attacks = { "play", "swing", "cast", "stab" };
    public Vector3 prev_facing;
    private bool key_lock;

    private int DEX;
    private int WIS;
    private int HEALTH;
    private int DAMAGE;
    public int EXP;

    private int log_lines;
    public Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        x_pos = this.transform.position.x;
        z_pos = this.transform.position.z;
        y_pos = this.transform.position.y;
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        curr_pos = grid.squares[(int)x_pos, (int)z_pos].GetComponent<GridSquare>();
        fighting = false;
        running = false;
        facing = 'n';
        key_lock = false;

        DEX = 15;
        WIS = 15;
        HEALTH = 187;
        DAMAGE = 22;
        EXP = 0;

        GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
        GameObject.Find("Damage").GetComponent<UnityEngine.UI.Text>().text = "DAM: " + DAMAGE;
        GameObject.Find("Dexterity").GetComponent<UnityEngine.UI.Text>().text = "DEX: " + DEX;
        GameObject.Find("Wisdom").GetComponent<UnityEngine.UI.Text>().text = "WIS: " + WIS;
        GameObject.Find("Strength").GetComponent<UnityEngine.UI.Text>().text = "EXP: " + EXP;
        GameObject.Find("Intelligence").GetComponent<UnityEngine.UI.Text>().text = "INT: 15";

        log_lines = 0;
        levelComplete = false;
    }

    void partyStop()
    {
        rb.velocity = new Vector3(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("stop");
            if (i == 2 || i == 0 || i == 3)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("stop");
                }
            }
            else if (i == 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("stop");
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (fighting && Input.GetKeyDown(KeyCode.Space) && !key_lock) {
            fighting = false;
        } else if (fighting) { return; }

        /*if (Input.GetKeyDown(KeyCode.UpArrow)) {
            z_pos += 1f;
            if (!Move('n', false, (int)x_pos, (int)z_pos)) { z_pos -= 1f; }
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            z_pos -= 1f;
            if (!Move('s', false, (int)x_pos, (int)z_pos)) { z_pos += 1f; }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            x_pos -= 1f;
            if (!Move('w', false, (int)x_pos, (int)z_pos)) { x_pos += 1f; }
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            x_pos += 1f;
            if (!Move('e', false, (int)x_pos, (int)z_pos)) { x_pos -= 1f; }
        }*/
        
        if (x_pos == grid.end_x && z_pos == 19 - grid.end_y && !grid.freeMode && !levelComplete) {
            if (HEALTH <= grid.HP_goal && EXP >= grid.EXP_goal) {
                GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().EndDialogue();
                levelComplete = true;
                Invoke("partyStop", 0.5f);
            } else {
                GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().FailDialogue();
                Invoke("partyStop", 0.5f);
            }
        } else if (x_pos == grid.end_x && z_pos == 19 - grid.end_y)
        {
            Invoke("partyStop", 0.5f);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !running) {
            GameObject.Find("ObjectStats").GetComponent<UnityEngine.UI.Text>().text = "";
            char[] path = Pathfind(grid.end_x, 19 - grid.end_y);
            if (path == null) {
                GameObject.Find("ObjectStats").GetComponent<UnityEngine.UI.Text>().text = "Make sure there's a clear path to the exit!";
                return;
            }
            running = true;
            this.gameObject.transform.GetChild(4).GetComponent<Camera>().enabled = true;
            for (int i = 0; i < 4; i++) {
                this.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("move");
                if (i == 2 || i == 0 || i == 3)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("move");
                    }
                }
                else if (i == 1)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("move");
                    }
                }
            }
            GameObject.Find("ObjectMenu").GetComponent<UIController>().Hide(GameObject.Find("ObjectMenu"));
            GameObject.Find("MenuButton").GetComponent<UIController>().Hide(GameObject.Find("MenuButton"));
            StartCoroutine(SlowMove(path));
        }
	}

    private IEnumerator SlowMove(char[] path) {
        //yield return new WaitForSeconds(0.5f);
        char prev_dir = facing;
        float offset = 0;
        foreach (var dir in path)
        {
            if (fighting && Input.GetKeyDown(KeyCode.Space) && !key_lock) {
                fighting = false;
            } else if (fighting) { break; }
            
            if (dir == 'e') {
                if (prev_dir == 'n') {
                    this.transform.Rotate(new Vector3(0, 90, 0));
                } else if (prev_dir == 's') {
                    this.transform.Rotate(new Vector3(0, -90, 0));
                } else {
                    offset += 0.00025f;
                }
                x_pos++;
            } else if (dir == 'w') {
                if (prev_dir == 'n') {
                    this.transform.Rotate(new Vector3(0, -90, 0));
                } else if (prev_dir == 's') {
                    this.transform.Rotate(new Vector3(0, 90, 0));
                } else {
                    offset += 0.00025f;
                }
                x_pos--;
            } else if (dir == 'n') {
                if (prev_dir == 'e') {
                    this.transform.Rotate(new Vector3(0, -90, 0));
                } else if (prev_dir == 'w') {
                    this.transform.Rotate(new Vector3(0, 90, 0));
                } else {
                    offset += 0.00025f;
                }
                z_pos++;
            } else if (dir == 's') {
                if (prev_dir == 'e') {
                    this.transform.Rotate(new Vector3(0, 90, 0));
                } else if (prev_dir == 'w') {
                    this.transform.Rotate(new Vector3(0, -90, 0));
                } else {
                    offset += 0.00025f;
                }
                z_pos--;
            }

            facing = dir;
            if (dir != prev_dir & (dir == 'e' || dir == 'w' || dir == 'n' || dir == 's'))
            {
                //Invoke("Snap", 0.3f);
                offset = 0;
            }
            Move(dir, false, (int)x_pos, (int)z_pos);
            if (dir == 'e' || dir == 'w' || dir == 'n' || dir == 's') { prev_dir = dir; }
            yield return new WaitForSeconds(.5f - offset);
        }
    }

    private void Snap()
    {
        this.transform.position = new Vector3(x_pos, y_pos, z_pos);
    }

    private bool Move(char direction, bool check, int x, int y)
    {
        GridSquare new_pos = grid.squares[x, y].GetComponent<GridSquare>();
        if (new_pos.getItem() != "empty" && new_pos.getItem() != "pit" && new_pos.getItem() != "spikes")
        {
                return false;
        }

        if (!check)
        {
            damaged = false;

            // Slow Movements
            //print("(" + tempx + ", " + tempz + ") | (" + x_pos + ", " + z_pos + ")");
            if (x_pos - this.transform.position.x > 0.5f){
                rb.velocity = new Vector3(1.95f, 0, 0);
            } else if (this.transform.position.x - x_pos > 0.5f) {
                rb.velocity = new Vector3(-1.95f, 0, 0);
            }

            if (z_pos - this.transform.position.z > 0.5f) {
                rb.velocity = new Vector3(0, 0, 1.95f);
            } else if (this.transform.position.z - z_pos > 0.5f) {
                rb.velocity = new Vector3(0, 0, -1.95f);
            }

            //this.transform.Translate(Vector3.forward * Time.deltaTime);

            //this.transform.position = new Vector3(x_pos, y_pos, z_pos);
            curr_pos = new_pos;

            if ((curr_pos.getItem() == "pit" || curr_pos.getItem() == "spikes") && !curr_pos.triggered)
            {
                running = false;
                fighting = true;
                Invoke("partyStop", 0.5f);
                curr_pos.triggered = true;
                SFXHandler sfx = GameObject.Find("SFX Source").GetComponent<SFXHandler>();
                if (!NoticeCheck(curr_pos)) {
                    takeDamage(curr_pos.item.GetComponent<TrapStats>().GetDamage());
                    LogPrint("> The party takess " + curr_pos.item.GetComponent<TrapStats>().GetDamage() + " damage!\n");
                    sfx.playSound(curr_pos.item.GetComponent<TrapStats>().GetFailureSound());
                    this.GetComponent<Renderer>().material.color = Color.red;
                    GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
                    curr_pos.item.GetComponent<Animator>().SetTrigger("start");
                } else if (!AvoidCheck(curr_pos)) {
                    // Pass Notice Check!
                    takeDamage(curr_pos.item.GetComponent<TrapStats>().GetDamage());
                    LogPrint("> The party takes " + curr_pos.item.GetComponent<TrapStats>().GetDamage() + " damage!\n");
                    sfx.playSound(curr_pos.item.GetComponent<TrapStats>().GetFailureSound());
                    this.GetComponent<Renderer>().material.color = Color.red;
                    GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
                    curr_pos.item.GetComponent<Animator>().SetTrigger("start");
                } else {
                    // Pass Avoid Check + successfully dodge
                    sfx.playSound(curr_pos.item.GetComponent<TrapStats>().GetSuccessSound());
                    this.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else { this.GetComponent<Renderer>().material.color = Color.white; }
        }

        return true;
    }

    public void LogPrint(string line)
    {
        string log = GameObject.Find("Log").GetComponent<UnityEngine.UI.Text>().text;
        if (log_lines < 8)
        {
            GameObject.Find("Log").GetComponent<UnityEngine.UI.Text>().text += line;
            log_lines++;
        } else
        {
            GameObject.Find("Log").GetComponent<UnityEngine.UI.Text>().text = log.Substring(log.IndexOf("\n") + 1) + line;
        }
    }

    public IEnumerator Fight(GridSquare enemy)
    {
        SFXHandler sfx = GameObject.Find("SFX Source").GetComponent<SFXHandler>();
        fighting = true;
        running = false;
        key_lock = true;
        yield return new WaitForSeconds(0.5f);
        sfx.playCombat();
        for (int i = 0; i < 4; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("fight");
            if (i == 2 || i == 0 || i == 3)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("fight");
                }
            }
            else if (i == 1)
            {
                for (int j = 0; j < 5; j++)
                {
                    this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger("fight");
                }
            }
        }
        string attacker_name = (enemy.item.gameObject.name.Split('('))[0].ToLower();
        enemy.item.gameObject.GetComponent<Animator>().SetTrigger("fight");
        if (attacker_name == "wraith")
        {
            for (int i = 0; i < 3; i++)
            {
                enemy.item.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("fight");
            }
        } else if (attacker_name == "lich")
        {
            for (int i = 0; i < 4; i++)
            {
                enemy.item.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("fight");
            }
        }
        LogPrint("> The party has encountered a " + attacker_name + "!\n");
        while (HEALTH > 0 && enemy.item.GetComponent<EnemyStats>().GetHealth() > 0)
        {
            // Enemy attacks
            if (Random.Range(0f, 1f) <= enemy.item.GetComponent<EnemyStats>().GetHitrate())
            {
                if (attacker_name == "zombie" || attacker_name == "ghost") {
                    enemy.item.gameObject.GetComponent<Animator>().SetTrigger("slap");
                } else if (attacker_name == "skeleton") {
                    enemy.item.gameObject.GetComponent<Animator>().SetTrigger("slap");
                } else if (attacker_name == "wraith") {
                    enemy.item.gameObject.GetComponent<Animator>().SetTrigger("cast");
                    for (int i = 0; i < 3; i++)
                    {
                        enemy.item.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("cast");
                    }
                } else if (attacker_name == "lich") {
                    enemy.item.gameObject.GetComponent<Animator>().SetTrigger("cast");
                    for (int i = 0; i < 4; i++)
                    {
                        enemy.item.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger("cast");
                    }
                }

                if (Mathf.Abs(x_pos - enemy.item.GetComponent<EnemyStats>().gameObject.transform.position.x) > 1 || Mathf.Abs(z_pos - enemy.item.GetComponent<EnemyStats>().gameObject.transform.position.z) > 1)
                {
                    takeDamage(enemy.item.GetComponent<EnemyStats>().GetRangedDamage());
                    LogPrint("> The party takes " + enemy.item.GetComponent<EnemyStats>().GetRangedDamage() + " damage!\n");
                    GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
                }
                else
                {
                    takeDamage(enemy.item.GetComponent<EnemyStats>().GetMeleeDamage());
                    LogPrint("> The party takes " + enemy.item.GetComponent<EnemyStats>().GetMeleeDamage() + " damage!\n");
                    GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
                }

            }

            yield return new WaitForSeconds(1.5f);

            // Party attacks
            if (Random.Range(0f, 1f) <= 0.67f) {
                enemy.item.GetComponent<EnemyStats>().TakeDamage(DAMAGE);
                LogPrint("> The party deals " + DAMAGE + " damage!\n");
                for (int i = 0; i < 4; i++)
                {
                    this.gameObject.transform.GetChild(i).GetComponent<Animator>().SetTrigger(attacks[i]);
                    if (i == 2 || i == 0 || i == 3)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger(attacks[i]);
                        }
                    }
                    else if (i == 1)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            this.gameObject.transform.GetChild(i).GetChild(j).GetComponent<Animator>().SetTrigger(attacks[i]);
                        }
                    }
                }
                LogPrint("> The enemy now has " + enemy.item.GetComponent<EnemyStats>().GetHealth() + " HP.\n");
            }

            yield return new WaitForSeconds(2.5f);
        }

        string enemy_name = enemy.item.name;
        if (enemy.item.GetComponent<EnemyStats>().GetHealth() <= 0 || HEALTH <= 0)
        {
            if (!grid.firstSkeleton && enemy_name == "Skeleton(Clone)")
            {
                GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().SkeletonDialogue();
                grid.firstSkeleton = true;
            }

            if (!grid.firstZombie && enemy_name == "Zombie(Clone)")
            {
                GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().ZombieDialogue();
                grid.firstZombie = true;
            }

            if (!grid.firstGhost && enemy_name == "Ghost(Clone)")
            {
                GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().GhostDialogue();
                grid.firstGhost = true;
            }

            if (!grid.firstWraith && enemy_name == "Wraith(Clone)")
            {
                GameObject.Find("PlotWindow").GetComponent<DialogueHandler>().WraithDialogue();
                grid.firstWraith = true;
            }
        }

        if (HEALTH <= 0) {
            LogPrint("> The party has died!\n");
            Destroy(this.gameObject);
            sfx.stopCombat();
        } else if (enemy.item.GetComponent<EnemyStats>().GetHealth() <= 0) {
            LogPrint("> The enemy has been slain!\n");
            EXP += enemy.item.GetComponent<EnemyStats>().GetEXP();
            GameObject.Find("Strength").GetComponent<UnityEngine.UI.Text>().text = "EXP: " + EXP;
            Destroy(enemy.item.gameObject);
            enemy.resetSquare();
            this.gameObject.transform.LookAt(prev_facing);
            sfx.stopCombat();
        }
        key_lock = false;
    }

    public void GetPos(ref int x, ref int y)
    {
        x = (int)x_pos;
        y = (int)z_pos;
    }

    public bool NoticeCheck(GridSquare square)
    {
        LogPrint("> A trap has been sprung!\n");
        if (RollDie() + getModifier(WIS) > square.item.GetComponent<TrapStats>().GetNoticeCheck()) {
            LogPrint("> The party passes the notice check!\n");
            return true;
        } else {
            LogPrint("> The party fails the notice check\n");
            return false;
        }
    }

    public bool AvoidCheck(GridSquare square)
    {
        if (RollDie() + getModifier(DEX) > square.item.GetComponent<TrapStats>().GetAvoidCheck()) {
            LogPrint("> The party passes the avoid check!\n");
            return true;
        } else {
            LogPrint("> The party fails the avoid check\n");
            return false;
        }
    }

    private int getModifier(int score) {
        int[] modifiers = new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        return modifiers[score / 2];
    }

    public int RollDie()
    {
        int roll = (int)Random.Range(0f, 20f);
        LogPrint("> The party rolled a " + roll + "!\n");
        return (int)Random.Range(0f, 20f);
    }

    public void takeDamage(int damage)
    {
        HEALTH -= damage;
        if (HEALTH < 0)
        {
            HEALTH = 0;
        }
    }

    public char[] Pathfind(int dest_x, int dest_y)
    {
        int[,] paths = new int[grid.GetXSize(), grid.GetYSize()];
        Queue<GridSquare> queue = new Queue<GridSquare>();

        paths[(int)x_pos, (int)z_pos] = 1;
        queue.Enqueue(curr_pos);
        int x = (int)x_pos;
        int y = (int)z_pos;
        bool found = false;

        int step = 0;
        while(!found)
        {
            GridSquare curr_square = queue.Dequeue();
            curr_square.GetPos(ref x, ref y);
            step = paths[x, y];
            bool no_moves = true;
            //Debug.Log(x + ", " + y + ", " + paths[x, y]);

            y += 1;
            if (y >= 0 && y < 20 && Move('n', true, x, y)) {
                if (paths[x, y] == 0) {
                    no_moves = false;
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            y -= 1;

            y -= 1;
            if (y >= 0 && y < 20 && Move('s', true, x, y)) {
                if (paths[x, y] == 0) {
                    no_moves = false;
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            y += 1;

            x -= 1;
            if (x >= 0 && x < 20 && Move('w', true, x, y)) {
                if (paths[x, y] == 0) {
                    no_moves = false;
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            x += 1;

            x += 1;
            if (x >= 0 && x < 20 && Move('e', true, x, y))
            {
                if (paths[x, y] == 0) {
                    no_moves = false;
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            x -= 1;

            if (no_moves && queue.Count == 0) {
                return null;
            }
        }

        /*for (int i = 5; i >= 0; i--) {
            Debug.Log(paths[0, i] + ", " + paths[1, i] + ", " + paths[2, i] + ", " + paths[3, i] + ", " + paths[4, i] + ", " + paths[5, i]);
        }*/

        char[] directions = new char[paths[dest_x, dest_y]];
        //Debug.Log(paths[dest_x, dest_y]);
        step = paths[dest_x, dest_y];
        x = dest_x;
        y = dest_y;
        int min = 10000000;
        char direction = '_';
        while (x != (int)x_pos || y != (int)z_pos)
        {
            if (x - 1 >= 0 && min >= paths[x - 1, y] && paths[x-1, y] != 0)
            {
                min = paths[x - 1, y];
                direction = 'e';
            }

            if (x + 1 < 20 && min >= paths[x + 1, y] && paths[x+1, y] != 0)
            {
                min = paths[x + 1, y];
                direction = 'w';
            }

            if (y - 1 >= 0 && min >= paths[x, y - 1] && paths[x, y-1] != 0)
            {
                min = paths[x, y - 1];
                direction = 'n';
            }

            if (y + 1 < 20 && min >= paths[x, y + 1] && paths[x, y+1] != 0)
            {
                min = paths[x, y + 1];
                direction = 's';
            }

            step--;
            directions[step] = direction;
            if (direction == 'e') {
                x--;
            } else if (direction == 'w') {
                x++;
            } else if (direction == 'n') {
                y--;
            } else if (direction == 's') {
                y++;
            }
            //Debug.Log("(" + x + ", " + y + ")" + ", " + direction);
        }
        return directions;
    }

    public int getHealth()
    {
        return HEALTH;
    }
}
