using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Grid : MonoBehaviour {

    [SerializeField]
    static public int x_size = 20;
    [SerializeField]
    static public int y_size = 20;

    public GameObject wall;
    public GameObject torch_wall;
    public GameObject pit;
    public GameObject crushing_wall;
    public GameObject spikes;
    public GameObject boulder;
    public GameObject arrow_wall;
    public GameObject enemy;
    public GameObject party;
    public GameObject boss;
    public GameObject floor;
    public Material floor_material;

    public string level_file;
    public int end_x;
    public int end_y;
    public int start_x;
    public int start_y;
    public int boss_x;
    public int boss_y;

    public int HP_goal;
    public int EXP_goal;

    public GridSquare[,] squares = new GridSquare[x_size, y_size];

    /* Selection Key
     * --------------
     * 0 - Nothhing
     * 1 - Wall
     * 2 - Pit
     * 3 - Crushing Wall
     * 4 - Spikes
     * 5 - Boulder
     * 6 - Arrow Wall
     * 7 - Party
     * e - Generic enemy
     * 9 - Delete
     */
    private char selection = '0';
    public int gold;
    public bool pause;
    public bool firstTrap = false;
    public bool firstSkeleton = false;
    public bool firstWraith = false;
    public bool freeMode = false;

    // Initialize grid
    private void Start()
    {
        DrawGrid();
    }

    // Reads for hotkeys to switch selection
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            selection = 'r';
        }

        GameObject.Find("Gold").GetComponent<UnityEngine.UI.Text>().text = gold + "g";
    }

    private void DrawGrid()
    {
        StreamReader reader = new StreamReader("Assets/LevelBlueprints/" + level_file);

        // Create grid squares for visualization
        for (int i = 0; i < x_size; i++) {
            string line = reader.ReadLine();
            for (int j = 0; j < y_size; j++) {
                GameObject square = Instantiate<GameObject>(floor); //GameObject.CreatePrimitive(PrimitiveType.Plane);
                square.transform.position = new Vector3(j, 0, 19 - i);
                square.transform.localScale = new Vector3(4.25f, 4f, 4.25f);
                if (j == end_x && i == end_y) {
                    square.GetComponent<Renderer>().material.color = Color.blue;
                } else {
                    square.GetComponent<Renderer>().material.color = Color.green;
                }
                square.AddComponent<GridSquare>();
                
                if (line[j] == 'x' || line[j] == 's' || line[j] == 'n' || line[j] == 'w' || line[j] == 'e') {
                    if (line[j] == 'x') { square.GetComponent<GridSquare>().item = Instantiate<GameObject>(wall); }
                    else { square.GetComponent<GridSquare>().item = Instantiate<GameObject>(torch_wall); }
                    square.GetComponent<GridSquare>().item.transform.position = new Vector3(j, 0.5f, 19 - i);
                    square.GetComponent<GridSquare>().item.transform.localScale = new Vector3(4.25f, 4.25f, 4.25f);
                    if (line[j] == 'n') {
                        square.GetComponent<GridSquare>().item.transform.Rotate(new Vector3(0, 180));
                    } else if (line[j] == 'e') {
                        square.GetComponent<GridSquare>().item.transform.Rotate(new Vector3(0, 270));
                    } else if (line[j] == 'w') {
                        square.GetComponent<GridSquare>().item.transform.Rotate(new Vector3(0, 90));
                    }
                    square.GetComponent<GridSquare>().facing = 'w';
                    square.GetComponent<GridSquare>().item_name = "wall";
                    square.GetComponent<GridSquare>().deletable = false;
                }

                squares[j, 19 - i] = square.GetComponent<GridSquare>();

                /*GameObject square = GameObject.CreatePrimitive(PrimitiveType.Plane);
                square.transform.position = new Vector3(i, 0, j);
                square.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
                if (i == 10 && j == 19) {
                    square.GetComponent<Renderer>().material.color = Color.blue;
                } else {
                    square.GetComponent<Renderer>().material.color = Color.green;
                }
                square.AddComponent<GridSquare>();
                squares[i, j] = square.GetComponent<GridSquare>();*/
            }
        }
    }

    public int GetSelection()
    {
        return selection;
    }

    public int GetXSize()
    {
        return x_size;
    }

    public int GetYSize()
    {
        return y_size;
    }

    public void changeSelection(string input)
    {
        char[] buffer = input.ToCharArray();
        selection = buffer[0];

        string item_name = "";
        string notice_check = "";
        string avoid_check = "";
        string damage = "";
        switch(selection)
        {
            case '1':
                item_name = "Wall";
                notice_check = "10";
                avoid_check = "15";
                damage = "22";
                break;
            case '2':
                item_name = "Pit";
                notice_check = "10";
                avoid_check = "10";
                damage = "5";
                break;
            case '3':
                item_name = "Crushing Wall";
                notice_check = "10";
                avoid_check = "15";
                damage = "22";
                break;
            case '4':
                item_name = "Spikes";
                notice_check = "12";
                avoid_check = "12";
                damage = "11";
                break;
            case '5':
                item_name = "Boulder";
                notice_check = "15";
                avoid_check = "15";
                damage = "35";
                break;
            case '6':
                item_name = "Arrow Wall";
                notice_check = "12";
                avoid_check = "12";
                damage = "11";
                break;
        }
        if (selection != 'e')
        {
            GameObject.Find("ObjectStats").GetComponent<UnityEngine.UI.Text>().text = item_name + "\n\nNotice Check: " + notice_check + "\nAvoid Check: " + avoid_check + "\nDAM: " + damage;
        }
    }

    public void changeEnemy(GameObject input)
    {
        enemy = input;
        GameObject.Find("ObjectStats").GetComponent<UnityEngine.UI.Text>().text = enemy.name + "\n\nHP: " + enemy.GetComponent<EnemyStats>().GetHealth().ToString() + "\nMELEE DAM: " + enemy.GetComponent<EnemyStats>().GetMeleeDamage().ToString() + "\nRANGED DAM: " + enemy.GetComponent<EnemyStats>().GetRangedDamage().ToString() + "\nEXP: " + enemy.GetComponent<EnemyStats>().GetEXP().ToString();
    }

    public int getGold()
    {
        return gold;
    }

    public bool spendGold(int price)
    {
        if (price > gold) {
            return false;
        } else {
            gold -= price;
            return true;
        }
    }

    public void refund(int price)
    {
        gold += price;
    }

    public void disableInput()
    {
        pause = true;
    }

    public void enableInput()
    {
        pause = false;
    }

    public bool getPause()
    {
        return pause;
    }
}
