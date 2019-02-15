using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.UI;
using de.TrustfallGames.UnderConstruction.Util;
using de.TrustfallGames.UnderConstruction.SocialPlatform.GooglePlay;
using de.TrustfallGames.UnderConstruction.SoundManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    [RequireComponent(typeof(CounterHive))]
    [RequireComponent(typeof(Settings))]
    [RequireComponent(typeof(Controller))]
    [RequireComponent(typeof(InternTick))]
    public class GameManager : MonoBehaviour, IInternUpdate {
        private static GameManager _instance = null;
        [SerializeField] private Character character = null;
        [SerializeField] private Controller controller;
        [SerializeField] private MapManager mapManager;
        [SerializeField] private Settings settings;
        [SerializeField] private SocialPlatformHandler platformHandler;
        private InternTick internTick;
        
        private CounterHive _counterHive;
        private UiManager _uiManager;

        private GameManager() { }

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }


            controller = GetComponent<Controller>();
            settings = GetComponent<Settings>();
            internTick = GetComponent<InternTick>();
            platformHandler = SocialPlatformHandler.GetSocialHandler();
            RegisterInternUpdate();
            SoundHandler.GetInstance().StopSound();
        }

        public GameManager RegisterCharacter(Character character) {
            if (this.character == null) {
                controller.Character = this.character = character;
                mapManager.RegisterCharacter(character);
            } else {
                throw new Exception("Character already set!");
            }

            return this;
        }

        public void RegisterMapManager(MapManager mapManager) {
            if (this.mapManager == null) {
                this.mapManager = mapManager;
            } else {
                throw new Exception("MapManager already set!");
            }
        }

        public static GameManager GetManager() { return _instance; }

        public Character Character => character;
        public Controller Controller => controller;
        public MapManager MapManager => mapManager;
        public Settings Settings => settings;

        public GameManager RegisterUiManager(UiManager uiManager) {
            _uiManager = uiManager;
            return this;
        }

        public UiManager UiManager => _uiManager;

        public void Lose()
        {
            if (PlayerPrefHandler.GetHighScore() < Character.Highscore)
            {
                PlayerPrefHandler.SetHighScore(Character.Highscore);
                platformHandler.SendToLeaderboard(Character.Highscore);
            }

            _uiManager.OnGameLost();
        }

        public GameManager RegisterCounterHive(CounterHive counterHive) {
            _counterHive = counterHive;
            return this;
        }

        public CounterHive CounterHive => _counterHive;

        public void InternUpdate() {  }

        public void RegisterInternUpdate() {
            internTick.RegisterTickObject(this, 1);

        }

        public InternTick InternTick => internTick;

        public void Init() {  }
    }
}