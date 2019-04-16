using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
