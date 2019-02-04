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

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void StartGameIsometric()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
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
