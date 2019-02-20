using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.Util {
    public class TransitionBitch :MonoBehaviour {
        private Sprite sprite;

        private static TransitionBitch _instance;

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public Sprite GetSprite() {
            Debug.Log("Sprite send!");
            var a = sprite;
            sprite = null;
            return a;
        }

        public bool SpriteValid() { return sprite != null; }

        public void SetSprite(Sprite sprite) { this.sprite = sprite; }

        public static TransitionBitch GetInstance() { return _instance; }
    }
}