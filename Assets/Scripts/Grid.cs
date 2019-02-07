using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    static public int x_size = 20;
    [SerializeField]
    static public int y_size = 20;

    public Transform wall;
    public Transform pit;
    public Transform crushing_wall;
    public Transform spikes;
    public Transform boulder;
    public Transform arrow_wall;
    public Transform generic_enemy;
    public Transform party;

    public GameObject[,] squares = new GameObject[x_size, y_size];

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

    // Initialize grid
    private void Start()
    {
        DrawGrid();
    }

    // Reads for hotkeys to switch selection
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            selection = '0';
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
        } else if (Input.GetKeyDown(KeyCode.E)) {
            selection = 'e';
        } else if (Input.GetKeyDown(KeyCode.R)) {
            selection = 'r';
        }
    }

    private void DrawGrid()
    {
        // Create grid squares for visualization
        for (int i = 0; i < x_size; i++) {
            for (int j = 0; j < y_size; j++) {
                GameObject square = GameObject.CreatePrimitive(PrimitiveType.Plane);
                square.transform.position = new Vector3(i, 0, j);
                square.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
                square.GetComponent<Renderer>().material.color = Color.green;
                square.AddComponent<GridSquare>();
                squares[i, j] = square;
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
}
