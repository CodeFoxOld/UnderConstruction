using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager _gamemanager;

    private void Start()
    {
        _gamemanager = GameManager.GetManager();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("camera");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
