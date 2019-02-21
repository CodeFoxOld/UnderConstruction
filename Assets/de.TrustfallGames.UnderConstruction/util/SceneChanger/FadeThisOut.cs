using System;
using de.TrustfallGames.UnderConstruction.UI.title_screen;
using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.util.SceneChanger {
    public class FadeThisOut : MonoBehaviour {
        private Image image;

        [Range(0.01f, 1)]
        [SerializeField]
        private float fadeDuration = 0.05f;

        // Start is called before the first frame update
        void Start() { image = GetComponent<Image>(); }

        // Update is called once per frame
        void Update() {
            TitleBehaviour.FadeAlpha(ref image, -(fadeDuration));
            if (Math.Abs(image.color.a) < 0.001) {
                Destroy(gameObject);
            }
        }
    }
}