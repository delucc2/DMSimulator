using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

    public GameObject item;
    private Grid grid;

	// Use this for initialization
	void Start () {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        item = null;
	}

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0))
        {
            if (grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item_name != "wall" && grid.GetSelection() != 'r' && grid.GetSelection() != '9')
            {
                //print(grid.squares[(int)this.transform.position.x, 19 - (int)this.transform.position.z].item_name);
                return;
            }

            switch (grid.GetSelection())
            {
                case '3':
                    if (!(grid.spendGold(200))) {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.crushing_wall);
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    item.transform.position = this.transform.position;
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 's';
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item_name = "crushing wall";
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item = item;
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].range = 2;
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].FindTriggerSquares(false);
                    break;
                case '6':
                    if (!(grid.spendGold(200)))
                    {
                        break;
                    }
                    item = Instantiate<GameObject>(grid.arrow_wall);
                    item.transform.position = this.transform.position;
                    item.transform.Rotate(new Vector3(0, 180, 0));
                    item.transform.localScale = new Vector3(3f, 3f, 3f);
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 'n';
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item_name = "arrow wall";
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item = item;
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].range = 3;
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].FindTriggerSquares(false);
                    break;
                case 'r':
                    if (grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing == 'n') {
                        grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 'e';
                    }
                    else if (grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing == 'e') {
                        grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 's';
                    }
                    else if (grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing == 's') {
                        grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 'w';
                    }
                    else if (grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing == 'w') {
                        grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 'n';
                    }
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].rotateSquare();
                    item.transform.Rotate(new Vector3(0, 90, 0));
                    break;
                case '9':
                    if (!grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].deletable) {
                        if (grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item_name != "wall") {
                            grid.refund(item.gameObject.GetComponent<TrapStats>().cost);
                            grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].facing = 'w';
                            grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].item_name = "wall";
                            Destroy(item.gameObject);
                            item = null;
                        }
                        break;
                    }

                    //grid.refund(item.gameObject.GetComponent<TrapStats>().cost);
                    grid.squares[(int)this.transform.position.x, (int)this.transform.position.z].resetSquare();
                    //Destroy(item.gameObject);
                    //item = null;
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            GridSquare square = grid.squares[(int)this.transform.position.x, (int)this.transform.position.z];
            if (!square.deletable)
            {
                return;
            }

            if (square.item_name == "enemy")
            {
                grid.refund(square.item.gameObject.GetComponent<EnemyStats>().GetCost());
            }
            else
            {
                grid.refund(square.item.gameObject.GetComponent<TrapStats>().cost);
            }
            square.resetSquare();
        }
    }

    // Update is called once per frame
    void Update () {

	}
}
