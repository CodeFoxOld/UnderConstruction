using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace de.TrustfallGames.UnderConstruction.UI.menu {
    /// <summary>
    /// Game Menu behaviour
    /// </summary>
    public class MenuBehaviour : MonoBehaviour {
        private GameManager _gamemanager;

        private void Start() { _gamemanager = GameManager.GetManager(); }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <param name="buildIndex"></param>
        [Obsolete]
        public void StartGame(int buildIndex) {
            SoundHandler.GetInstance().PlaySound(SoundName.Click);
            SceneManager.LoadScene(buildIndex);
        }

        /// <summary>
        /// Restarts the game
        /// </summary>
        [Obsolete]
        public void RestartGame() {
            SoundHandler.GetInstance().PlaySound(SoundName.Click);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Pause the current game
        /// </summary>
        public void PauseGame() { _gamemanager.UiManager.OnGamePaused(); }

        /// <summary>
        /// Unpause the current game
        /// </summary>
        public void UnpauseGame() { _gamemanager.UiManager.OnGameContinue(); }
    }
}