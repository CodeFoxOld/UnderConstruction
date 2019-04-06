using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI.Core {
    public class UiManager : MonoBehaviour {
        [SerializeField] private GameObject pauseMenuCanvas;
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject ui;

        private GameManager _gamemanager;
        private GameUIBehaviour _gameUI;

        
        private void Start() {
            _gamemanager = GameManager.GetManager().RegisterUiManager(this);

            pauseMenuCanvas = Instantiate(pauseMenuCanvas);
            gameOverCanvas = Instantiate(gameOverCanvas);
            ui = Instantiate(ui);

            _gameUI = ui.GetComponent<GameUIBehaviour>();


            pauseMenuCanvas.SetActive(false);
            gameOverCanvas.SetActive(false);
        }

        /// <summary>
        /// Activate the game freeze
        /// </summary>
        public void OnGamePaused() {
            _gamemanager.GamePaused = true;
            ui.SetActive(false);
            pauseMenuCanvas.SetActive(true);
            SoundHandler.GetInstance().PlaySound(SoundName.FadeIn);
        }

        /// <summary>
        /// Abort the game freeze
        /// </summary>
        public void OnGameContinue() {
            _gamemanager.GamePaused = false;
            ui.SetActive(true);
            pauseMenuCanvas.SetActive(false);
            SoundHandler.GetInstance().PlaySound(SoundName.FadeOut);
        }

        /// <summary>
        /// Changes the Highscore to the number
        /// </summary>
        /// <param name="highscore"></param>
        /// <returns></returns>
        public UiManager ChangeHighscore(int highscore) {
            _gameUI.ChangeScore(highscore);
            return this;
        }

        /// <summary>
        /// Inits a score popup at a Position with a defined text. The Position ist the World Position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        public void ShowPopUpAtPosition(Vector3 position, string text) {
            _gameUI.ShowPopUpAtPosition(position, text);
        }
        
        /// <summary>
        /// Method called, when the highscore changes as reaction on a player pickup
        /// </summary>
        /// <param name="highscoreMultiplicator"></param>
        /// <param name="apartmentColorType"></param>
        /// <param name="highscore"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public UiManager OnHighscoreCalc(int highscoreMultiplicator, ApartmentColorType apartmentColorType,
            int highscore, int height) {
            _gameUI.ChangeScore(highscore);
            _gameUI.ChangeComboCounter(highscoreMultiplicator, apartmentColorType);
            _gameUI.PopScoreWithMultiplier(highscoreMultiplicator);
            return this;
        }

        /// <summary>
        /// Called, when the Destruction Count is changed. Updates the UI element
        /// </summary>
        /// <param name="count"></param>
        public void OnDeconstructorChange(int count) { _gameUI.ChangeDestructorCount(count); }

        /// <summary>
        /// Toggles a destructible Spawn
        /// </summary>
        /// <param name="direction"></param>
        public void OnDesctructibleButtonPressed(DestructibleDirection direction) {
            _gamemanager.MapManager.SpawnDesctructible(direction);
        }

        /// <summary>
        /// Called, when the game is lost.
        /// </summary>
        public void OnGameLost() {
            _gamemanager.GamePaused = true;
            ui.SetActive(false);
            gameOverCanvas.SetActive(true);
        }
    }

    public enum DestructibleDirection { horizontal, vertical }
}