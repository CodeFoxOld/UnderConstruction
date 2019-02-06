using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    private GameManager _gamemanager;

    private void Start()
    {
        _gamemanager = GameManager.GetManager();
    }

    public void StartGame(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        _gamemanager.UiManager.OnGamePaused();
    }

    public void UnpauseGame()
    {
        _gamemanager.UiManager.OnGameContinue();
    }
}
