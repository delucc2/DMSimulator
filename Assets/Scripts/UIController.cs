using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UIController : MonoBehaviour {

    public void Hide(GameObject canvas) {
        canvas.GetComponent<CanvasGroup>().alpha = 0f;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvas.GetComponent<CanvasGroup>().interactable = false;
    }

    public void Show(GameObject canvas) {
        canvas.GetComponent<CanvasGroup>().alpha = 1f;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        canvas.GetComponent<CanvasGroup>().interactable = true;
    }

    public void goToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void AreYouSure(string option)
    {
        Time.timeScale = 0;
        if (option == "main menu") {
            GameObject.Find("Are You Sure?").transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Are you sure you want to quit?";
            Show(GameObject.Find("Are You Sure?"));
            Show(GameObject.Find("Quit"));
        } else if (option == "restart") {
            GameObject.Find("Are You Sure?").transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Are you sure you want to restart?";
            Show(GameObject.Find("Are You Sure?"));
            Show(GameObject.Find("Restart"));
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }
}
