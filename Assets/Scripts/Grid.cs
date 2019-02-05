using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    public int x_size = 5;
    [SerializeField]
    public int y_size = 5;

    // Initialize grip
    private void Start()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        // Create gird squares for visualization
        for (int i = 0; i < x_size; i++)
        {
            for (int j = 0; j < y_size; j++)
            {
                GameObject square = GameObject.CreatePrimitive(PrimitiveType.Plane);
                square.transform.position = new Vector3(i, 0, j);
                square.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
                square.GetComponent<Renderer>().material.color = Color.green;
                square.AddComponent<GridSquare>();
            }
        }
    }
}
