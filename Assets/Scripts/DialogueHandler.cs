using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{

    public string[] dialogue;
    public int[] stop_points;
    private int dialogue_counter;
    private int stop_counter;
    private bool hidden;
    private Grid grid;

    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        hidden = false;
        dialogue_counter = 0;
        stop_counter = 0;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hidden)
        {
            if (dialogue_counter == stop_points[stop_counter])
            {
                hidden = true;
                GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().alpha = 0f;
                GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().blocksRaycasts = false;
                GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = "";
                grid.enableInput();
                stop_counter++;
                return;
            }
            GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
            dialogue_counter++;
        }
    }

    public void BeginDialogue()
    {
        grid.disableInput();
        hidden = false;
        GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }
}
