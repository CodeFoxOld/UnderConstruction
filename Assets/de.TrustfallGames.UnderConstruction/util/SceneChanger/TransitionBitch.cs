using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.util.SceneChanger {
    /// <summary>
    /// Class to save transition image between scenes
    /// </summary>
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

        /// <summary>
        /// Returns the sprite from the last scene transition
        /// </summary>
        /// <returns></returns>
        public Sprite GetSprite() {
            var a = sprite;
            sprite = null;
            return a;
        }

        /// <summary>
        /// Sets if the next scene should start paused
        /// </summary>
        /// <returns></returns>
        public bool StartGamePaused() { return startGamePaused; }

        /// <summary>
        /// sets the game paused state
        /// </summary>
        /// <param name="b"></param>
        public void StartGamePaused(bool b) { startGamePaused = b; }

        /// <summary>
        /// Resets the game paused bool
        /// </summary>
        public void ResetGamePaused() { startGamePaused = false; }

        /// <summary>
        /// Returns true if the sprite is not null
        /// </summary>
        /// <returns></returns>
        public bool SpriteValid() { return sprite != null; }

        /// <summary>
        /// Sets the sprite for the transition in the next scene
        /// </summary>
        /// <param name="sprite"></param>
        public void SetSprite(Sprite sprite) { this.sprite = sprite; }

        public static TransitionBitch GetInstance() { return _instance; }

        private void FixedUpdate() {
            updateValues.Insert(0, Time.fixedDeltaTime);

            averageFixedDeltaTime = updateValues.Count > 0 ? 1f / 30f : updateValues.Average();

            if (updateValues.Count > 200) {
                updateValues.RemoveAt(200);
            }
        }

        /// <summary>
        /// Returns the fixed update time average from the last 200 frames
        /// </summary>
        public float AverageFixedDeltaTime => averageFixedDeltaTime;
    }
}
