using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.UI;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class GameManager : MonoBehaviour {
        private static GameManager _instance = null;
        [SerializeField] private Character character = null;
        [SerializeField] private Controller controller;
        [SerializeField] private MapManager mapManager;
        [SerializeField] private Settings settings;
        private List<Counter> counters = new List<Counter>();
        private UiManager _uiManager;

        private GameManager() { }

        public static GameManager GetManager() { return _instance; }

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }


            controller = GetComponent<Controller>();
            settings = GetComponent<Settings>();
        }

        public void RegisterCounter(Counter counter) { counters.Add(counter); }

        private void LateUpdate() {
            if (!_uiManager.GamePaused)
                foreach (var counter in counters) {
                    counter.Next();
                }
        }

        public void RegisterCharacter(Character character) {
            if (this.character == null) {
                controller.Character = this.character = character;
            } else {
                throw new Exception("Character already set!");
            }
        }

        public void RegisterMapManager(MapManager mapManager) {
            if (this.mapManager == null) {
                this.mapManager = mapManager;
            } else {
                throw new Exception("MapManager already set!");
            }
        }

        public Character Character => character;
        public Controller Controller => controller;
        public MapManager MapManager => mapManager;
        public Settings Settings => settings;

        public GameManager RegisterUiManager(UiManager uiManager) {
            _uiManager = uiManager;
            return this;
        }
    }
}