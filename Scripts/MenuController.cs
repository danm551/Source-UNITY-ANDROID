using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loads scene from main menu
public class MenuController : MonoBehaviour {

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
