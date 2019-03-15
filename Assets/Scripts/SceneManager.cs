using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

	public void LoadFreeMode() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DemoArea");
    }

    public void LoadCampaign() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelOne");
    }
}
