using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI {
    public class UiManager : MonoBehaviour {
        [SerializeField] private GameObject pauseMenuCanvas;
        [SerializeField] private GameObject ui;

        private GameManager _gamemanager;
        private bool gamePaused;
        public bool GamePaused => gamePaused;

        private void Start() {
            _gamemanager = GameManager.GetManager().RegisterUiManager(this);

            pauseMenuCanvas = Instantiate(pauseMenuCanvas);
            ui = Instantiate(ui);

            pauseMenuCanvas.SetActive(false);
        }

        public void OnGamePaused() {
            gamePaused = true;
            ui.SetActive(false);
            pauseMenuCanvas.SetActive(true);
        }

        public void OnGameContinue() {
            gamePaused = false;
            ui.SetActive(true);
            pauseMenuCanvas.SetActive(false);
        }

        public UiManager OnHighscoreCalc(int highscoreMultiplicator, int highscore) { return this; }

        public void OnDeconstructorChange(int count) {
            
        }
    }
}