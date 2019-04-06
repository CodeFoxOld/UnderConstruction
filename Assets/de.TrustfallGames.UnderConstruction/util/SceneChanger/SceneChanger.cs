using System;
using de.TrustfallGames.UnderConstruction.SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.util.SceneChanger {
    /// <summary>
    /// Util to change scenes.
    /// </summary>
    public class SceneChanger : MonoBehaviour {
        [SerializeField] private SceneEnum sceneEnum;
        [SerializeField] private bool LoadAsync;
        [SerializeField] private bool FadeOver;
        [SerializeField] private Image FadeImage;
        [SerializeField] private Sprite spriteFade;
        [SerializeField] private bool startGamePaused;

        [Range(0.01f, 1)]
        [SerializeField]
        private float fadeSpeed = 0.05f;

        private bool fadeInProgress;
        private bool fadeDone;
        private bool restart;

        private void Awake() {
            if (FadeImage != null) {
                var a = FadeImage.color;
            }

            if (FadeImage != null && FadeImage.GetComponent<TransitionImage>() == null) {
                FadeImage.enabled = false;
            }
        }

        private void FixedUpdate() {
            if (!fadeInProgress) return;
            var a = FadeImage.color;
            FadeImage.color = new Color(a.r, a.g, a.b, Mathf.Clamp(a.a + fadeSpeed, 0, 1));
            if (Math.Abs(FadeImage.color.a - 1) < 0.0001) {
                fadeDone = true;
                ChangeScene();
            }
        }

        /// <summary>
        /// Inits scene. Inits scene change or fade.
        /// </summary>
        public void ChangeScene() {
            Debug.Log("Change Scene to " + sceneEnum);
            if (FadeOver && !fadeDone) {
                StartFade();
                return;
            }

            if (FadeImage != null) {
                TransitionBitch.GetInstance().SetSprite(FadeImage.sprite);
            } else {
                TransitionBitch.GetInstance().SetSprite(spriteFade);
            }

            TransitionBitch.GetInstance().StartGamePaused(startGamePaused);

            if (LoadAsync) {
                if (restart) {
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                } else {
                    SceneManager.LoadSceneAsync((int) sceneEnum);
                }
            } else {
                if (restart) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                } else {
                    SceneManager.LoadScene((int) sceneEnum);
                }
            }
        }

        /// <summary>
        /// Starts the face process
        /// </summary>
        private void StartFade() {
            SoundHandler.GetInstance().PlaySound(SoundName.Click);
            fadeInProgress = true;
            FadeImage.enabled = true;
            if (spriteFade != null) {
                FadeImage.sprite = spriteFade;
            }
        }

        public void QuitApplication() { Application.Quit(); }

        /// <summary>
        /// Reloads the current loaded scene
        /// </summary>
        public void ReloadScene() {
            if (FadeImage != null) {
                TransitionBitch.GetInstance().SetSprite(FadeImage.sprite);
            } else {
                TransitionBitch.GetInstance().SetSprite(spriteFade);
            }

            restart = true;
            if (FadeOver) {
                StartFade();
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}