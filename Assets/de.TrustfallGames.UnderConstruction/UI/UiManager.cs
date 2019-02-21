using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI {
    public class UiManager : MonoBehaviour {
        [SerializeField] private GameObject pauseMenuCanvas;
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject ui;
        
        private GameManager _gamemanager;
        private bool gamePaused;
        private GameUIBehaviour _gameUI;
        
        public bool GamePaused => gamePaused;

        private void Start() {
            _gamemanager = GameManager.GetManager().RegisterUiManager(this);

            pauseMenuCanvas = Instantiate(pauseMenuCanvas);
            gameOverCanvas = Instantiate(gameOverCanvas);
            ui = Instantiate(ui);
            
            _gameUI = ui.GetComponent<GameUIBehaviour>();
            

            pauseMenuCanvas.SetActive(false);
            gameOverCanvas.SetActive(false);
        }

        public void OnGamePaused() {
            gamePaused = true;
            ui.SetActive(false);
            pauseMenuCanvas.SetActive(true);
            SoundHandler.GetInstance().PlaySound(SoundName.FadeIn);
        }

        public void OnGameContinue() {
            gamePaused = false;
            ui.SetActive(true);
            pauseMenuCanvas.SetActive(false);
            SoundHandler.GetInstance().PlaySound(SoundName.FadeOut);
        }

        public UiManager OnHighscoreCalc(int highscoreMultiplicator, ApartmentColorType apartmentColorType, int highscore, int height)
        {
            _gameUI.ChangeScore(highscore);
            _gameUI.ChangeComboCounter(highscoreMultiplicator, apartmentColorType);
            _gameUI.GetComponent<GameUIBehaviour>().PopScoreWithMultiplier(highscoreMultiplicator);
            return this;
        }

        public void OnDeconstructorChange(int count) {
            _gameUI.ChangeDestructorCount(count);
        }

        public void OnDesctructibleButtonPressed(DestructibleDirection direction) {
            _gamemanager.MapManager.SpawnDesctructible(direction);
        }

        public void OnGameLost()
        {
            SoundHandler.GetInstance().PlaySound(SoundName.GameOver);
            gamePaused = true;
            ui.SetActive(false);
            gameOverCanvas.SetActive(true);
        }

    }

    public enum DestructibleDirection {
        horizontal,
        vertical
    }
}