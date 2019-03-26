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
    public string[] boss_dialogue;
    public string[] middle_dialogue;
    public string[] near_end_dialogue;
    private string[] dialogue;
    private int dialogue_counter;
    private bool hidden;
    private Grid grid;
    private bool level_complete;
    public int level;

    public int boss_trigger_x = -1;
    public int boss_trigger_y = -1;
    public bool boss_triggered;

    public int middle_trigger_x = -1;
    public int middle_trigger_y = -1;
    public bool middle_triggered;

    public int end_trigger_x = -1;
    public int end_trigger_y = -1;
    public bool end_triggered;

    private PartyMovement party;

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
        int party_x = -1;
        int party_y = -1;
        if (party != null) {
            party.GetPos(ref party_x, ref party_y);
        } else {
            party = GameObject.Find("Party(Clone)").GetComponent<PartyMovement>();
        }
        //print("(" + party_x + ", " + party_y + ")");
        if (party_x == boss_trigger_x && party_y == boss_trigger_y && !boss_triggered) {
            boss_triggered = true;
            BossDialogue();
            party.running = false;
            party.fighting = true;
        } else if (party_x == middle_trigger_x && party_y == middle_trigger_y && !middle_triggered) {
            middle_triggered = true;
            MidDialogue();
            party.running = false;
            party.fighting = true;
        } else if (party_x == end_trigger_x && party_y == end_trigger_y && !end_triggered) {
            end_triggered = true;
            NearEndDialogue();
            party.running = false;
            party.fighting = true;
        }

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

    public void BossDialogue() {
        ShowDialogueBox();
        dialogue = boss_dialogue;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }

    public void MidDialogue() {
        ShowDialogueBox();
        dialogue = middle_dialogue;
        GameObject.Find("Dialogue").GetComponent<UnityEngine.UI.Text>().text = dialogue[dialogue_counter];
        dialogue_counter++;
    }

    public void NearEndDialogue() {
        ShowDialogueBox();
        dialogue = near_end_dialogue;
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
