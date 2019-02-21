using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.Util {
    public class TransitionBitch : MonoBehaviour {
        private Sprite sprite;

        private static TransitionBitch _instance;
        private bool startGamePaused;

        private List<float> updateValues = new List<float>();

        private float averageFixedDeltaTime;

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public Sprite GetSprite() {
            var a = sprite;
            sprite = null;
            return a;
        }

        public bool StartGamePaused() { return startGamePaused; }

        public void StartGamePaused(bool b) { startGamePaused = b; }

        public void ResetGamePaused() { startGamePaused = false; }

        public bool SpriteValid() { return sprite != null; }

        public void SetSprite(Sprite sprite) { this.sprite = sprite; }

        public static TransitionBitch GetInstance() { return _instance; }

        private void FixedUpdate() {
            updateValues.Insert(0, Time.fixedDeltaTime);

            averageFixedDeltaTime = updateValues.Count > 0 ? 1f / 30f : updateValues.Average();

            if (updateValues.Count > 200) {
                updateValues.RemoveAt(200);
            }
        }

        public float AverageFixedDeltaTime => averageFixedDeltaTime;
    }
}
