using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour {
    private GameManager _gamemanager;

    private void Start() { _gamemanager = GameManager.GetManager(); }

    public void StartGame(int buildIndex) {
        SoundHandler.GetInstance().PlaySound(SoundName.Click);
        SceneManager.LoadScene(buildIndex);
    }

    public void RestartGame() {
        SoundHandler.GetInstance().PlaySound(SoundName.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame() { _gamemanager.UiManager.OnGamePaused(); }

    public void UnpauseGame() { _gamemanager.UiManager.OnGameContinue(); }
}