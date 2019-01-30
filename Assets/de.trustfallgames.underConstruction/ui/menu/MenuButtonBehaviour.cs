using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonBehaviour : MonoBehaviour
{
    public void StartSceneLoad()
    {
        SceneManager.LoadScene("camera");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
