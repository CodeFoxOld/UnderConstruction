using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
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
        SoundHandler.GetInstance().PlaySound(SoundName.Click);
        SceneManager.LoadScene(sceneNumber);
    }

    public void RestartGame()
    {
        SoundHandler.GetInstance().PlaySound(SoundName.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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