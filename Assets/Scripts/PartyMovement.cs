using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour {
    private float x_pos;
    private float z_pos;
    private float y_pos;
    private GridSquare curr_pos;
    private Grid grid;
    private bool found;

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
            yield return new WaitForSeconds(0.5f);
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
            this.transform.position = new Vector3(x_pos, y_pos, z_pos);
            curr_pos = new_pos;

            if (curr_pos.getItem() == "pit" || curr_pos.getItem() == "spikes")
            {
                this.GetComponent<Renderer>().material.color = Color.red;
                // take damage
            }
            else { this.GetComponent<Renderer>().material.color = Color.white; }
        }

        return true;
    }

    public void GetPos(ref int x, ref int y)
    {
        x = (int)x_pos;
        y = (int)z_pos;
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
        while (x != 0 || y != 0)
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
