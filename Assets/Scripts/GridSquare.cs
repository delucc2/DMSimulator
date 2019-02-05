using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    private void OnMouseOver()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = this.transform.position;
        }
    }
}
