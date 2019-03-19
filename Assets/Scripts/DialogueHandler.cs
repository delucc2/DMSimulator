using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{

    public string[] intro_dialogue;
    public string[] firsttrap_dialogue;
    public string[] skeleton_dialogue;
    public string[] wraith_dialogue;
    public string[] ending_dialogue;
    private string[] dialogue;
    private int dialogue_counter;
    private bool hidden;
    private Grid grid;
    private bool level_complete;
    public int level;

    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        hidden = false;
        dialogue_counter = 0;
        dialogue = intro_dialogue;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
        level_complete = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && level_complete)
        {
            hidden = true;
            if (level == 1) {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelTwo");
            } else if (level == 2) {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelThree");
            } else if (level == 3)
            {
                if (dialogue_counter == dialogue.Length) {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                }
                GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
                dialogue_counter++;
            }
        }

        if (Input.GetMouseButtonDown(0) && !hidden)
        {
            if (dialogue_counter == dialogue.Length)
            {
                hidden = true;
                GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().alpha = 0f;
                GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().blocksRaycasts = false;
                GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = "";
                grid.enableInput();
                return;
            }
            GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
            dialogue_counter++;
        }
    }

    public void FirstTrapDialogue()
    {
        ShowDialogueBox();
        dialogue = firsttrap_dialogue;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }

    public void SkeletonDialogue()
    {
        ShowDialogueBox();
        dialogue = skeleton_dialogue;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }

    public void WraithDialogue()
    {
        ShowDialogueBox();
        dialogue = wraith_dialogue;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }

    public void EndDialogue()
    {
        ShowDialogueBox();
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = "Congratulations! You cleared the floor!";
        if (level == 3)
        {
            dialogue = ending_dialogue;
        }
        level_complete = true;
    }

    public void FailDialogue() {
        ShowDialogueBox();
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = "You failed to meet the requirements of this level, try again!";
    }

    private void ShowDialogueBox()
    {
        grid.disableInput();
        hidden = false;
        GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().alpha = 1f;
        GameObject.Find("PlotWindow").GetComponent<CanvasGroup>().blocksRaycasts = true;
        dialogue_counter = 0;
    }
}
