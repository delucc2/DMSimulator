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
    private bool fighting;

    private int DEX;
    private int WIS;
    private int HEALTH;
    private int DAMAGE;

    private int log_lines;

    // Use this for initialization
    void Start () {
        x_pos = this.transform.position.x;
        z_pos = this.transform.position.z;
        y_pos = this.transform.position.y;
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        curr_pos = grid.squares[(int)x_pos, (int)z_pos].GetComponent<GridSquare>();
        fighting = false;

        DEX = 15;
        WIS = 15;
        HEALTH = 112;
        DAMAGE = 52;

        GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
        GameObject.Find("Damage").GetComponent<UnityEngine.UI.Text>().text = "DAM: " + DAMAGE;
        GameObject.Find("Dexterity").GetComponent<UnityEngine.UI.Text>().text = "DEX: " + DEX;
        GameObject.Find("Wisdom").GetComponent<UnityEngine.UI.Text>().text = "WIS: " + WIS;
        GameObject.Find("Strength").GetComponent<UnityEngine.UI.Text>().text = "STR: 8";
        GameObject.Find("Intelligence").GetComponent<UnityEngine.UI.Text>().text = "INT: 15";

        log_lines = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (fighting) { return; }

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
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
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            char[] path = Pathfind(10, 19);
            StartCoroutine(SlowMove(path));
        }
	}

    private IEnumerator SlowMove(char[] path) {
        foreach (var dir in path)
        {
            if (fighting) { break; }

            if (dir == 'e') {
                x_pos++;
            } else if (dir == 'w') {
                x_pos--;
            } else if (dir == 'n') {
                z_pos++;
            } else if (dir == 's') {
                z_pos--;
            }

            Move(dir, false, (int)x_pos, (int)z_pos);
            yield return new WaitForSeconds(1f);
        }
    }

    private bool Move(char direction, bool check, int x, int y)
    {
        GridSquare new_pos = grid.squares[x, y].GetComponent<GridSquare>();
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

        if (!check)
        {
            damaged = false;
            this.transform.position = new Vector3(x_pos, y_pos, z_pos);
            curr_pos = new_pos;

            if (curr_pos.getItem() == "pit" || curr_pos.getItem() == "spikes")
            {
                if (!NoticeCheck(curr_pos)) {
                    takeDamage(curr_pos.item.GetComponent<TrapStats>().GetDamage());
                    this.GetComponent<Renderer>().material.color = Color.red;
                } else if (!AvoidCheck(curr_pos)) {
                    // Pass Notice Check!
                    takeDamage(curr_pos.item.GetComponent<TrapStats>().GetDamage());
                    this.GetComponent<Renderer>().material.color = Color.red;
                } else {
                    // Pass Avoid Check + successfully dodge
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
        if (log_lines < 9)
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
        fighting = true;
        LogPrint("> The party has encountered a zombie!\n");
        while (HEALTH > 0 && enemy.item.GetComponent<EnemyStats>().GetHealth() > 0)
        {
            // Party attacks
            if (Random.Range(0f, 1f) <= 0.67f) {
                enemy.item.GetComponent<EnemyStats>().TakeDamage(DAMAGE);
                LogPrint("> The party deals " + DAMAGE + " damage!\n");
                LogPrint("> The enemy now has " + enemy.item.GetComponent<EnemyStats>().GetHealth() + " HP.\n");
            }

            yield return new WaitForSeconds(2.5f);

            // Enemy attacks
            if (Random.Range(0f, 1f) < 0.5f) {
                takeDamage(enemy.item.GetComponent<EnemyStats>().GetDamage());
                LogPrint("> The party takes " + enemy.item.GetComponent<EnemyStats>().GetDamage() + " damage!\n");
                GameObject.Find("Health").GetComponent<UnityEngine.UI.Text>().text = "HP: " + HEALTH;
            }
        }

        if (HEALTH <= 0) {
            LogPrint("> The party has died!\n");
            Destroy(this.gameObject);
        } else if (enemy.item.GetComponent<EnemyStats>().GetHealth() <= 0) {
            LogPrint("> The enemy has been slain!\n");
            Destroy(enemy.item.gameObject);
            enemy.resetSquare();
        }

        fighting = false;
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
            LogPrint("> The party fails the notice check");
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
    }

    private char[] Pathfind(int dest_x, int dest_y)
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
            //Debug.Log(x + ", " + y + ", " + paths[x, y]);

            y += 1;
            if (y >= 0 && y < 20 && Move('n', true, x, y)) {
                if (paths[x, y] == 0) {
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            y -= 1;

            y -= 1;
            if (y >= 0 && y < 20 && Move('s', true, x, y)) {
                if (paths[x, y] == 0) {
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            y += 1;

            x -= 1;
            if (x >= 0 && x < 20 && Move('w', true, x, y)) {
                if (paths[x, y] == 0) {
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
                    paths[x, y] = step + 1;
                    queue.Enqueue(grid.squares[x, y]);
                    if (x == dest_x && y == dest_y) { found = true; continue; }
                }
            }
            x -= 1;
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
}
