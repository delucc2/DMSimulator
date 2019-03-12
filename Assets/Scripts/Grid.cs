using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    static public int x_size = 20;
    [SerializeField]
    static public int y_size = 20;

    public GameObject wall;
    public GameObject pit;
    public GameObject crushing_wall;
    public GameObject spikes;
    public GameObject boulder;
    public GameObject arrow_wall;
    public GameObject enemy;
    public GameObject party;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            selection = '0';
            GameObject.Find("ObjectStats").GetComponent<UnityEngine.UI.Text>().text = "";
        } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selection = '1';
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selection = '2';
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selection = '3';
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            selection = '4';
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            selection = '5';
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            selection = '6';
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) { 
            selection = '9';
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            selection = '7';
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            selection = 'e';
        } else if (Input.GetKeyDown(KeyCode.R)) {
            selection = 'r';
        }

        GameObject.Find("Gold").GetComponent<UnityEngine.UI.Text>().text = gold + "g";
    }

    private void DrawGrid()
    {
        // Create grid squares for visualization
        for (int i = 0; i < x_size; i++) {
            for (int j = 0; j < y_size; j++) {
                GameObject square = GameObject.CreatePrimitive(PrimitiveType.Plane);
                square.transform.position = new Vector3(i, 0, j);
                square.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
                if (i == 10 && j == 19) {
                    square.GetComponent<Renderer>().material.color = Color.blue;
                } else {
                    square.GetComponent<Renderer>().material.color = Color.green;
                }
                square.AddComponent<GridSquare>();
                squares[i, j] = square.GetComponent<GridSquare>();
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
        GameObject.Find("ObjectStats").GetComponent<UnityEngine.UI.Text>().text = enemy.name + "\n\nHP: " + enemy.GetComponent<EnemyStats>().GetHealth().ToString() + "\nDAM: " + enemy.GetComponent<EnemyStats>().GetDamage().ToString();
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
