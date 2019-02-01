using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI {
    public class UiManager :MonoBehaviour{
        private GameManager _gamemanager;
        private bool gamePaused;
        public bool GamePaused => gamePaused;

        private void Start() { _gamemanager= GameManager.GetManager().RegisterUiManager(this); }

        public void OnGamePaused() { gamePaused = true; }

        public void OnGameContinue() { gamePaused = false; }
    }
}