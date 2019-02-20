using System;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.UI.title_screen {
    public class TitleBehaviour : MonoBehaviour {
        [SerializeField] private GameObject title;
        [SerializeField] private GameObject subTitleLeft;
        [SerializeField] private GameObject subTitleRight;
        private Image titleImage;
        private Image subTitleLeftImage;
        private Image subTitleRightImage;

        [SerializeField] private AudioClip titleSound;

        private float counter;

        [Range(0, 1)]
        [SerializeField]
        private float fadeSpeed;

        private bool fadeInComplete;

        private int nextSceneIndex;
        private bool titleProgress;

        // Start is called before the first frame update
        void Start() {
            titleImage = title.GetComponent<Image>();
            subTitleLeftImage = subTitleLeft.GetComponent<Image>();
            subTitleRightImage = subTitleRight.GetComponent<Image>();
        }

        // Update is called once per frame
        private void FixedUpdate() {
            if (!fadeInComplete) {
                FadeAlpha(ref titleImage, fadeSpeed);
                if (Math.Abs(titleImage.color.a - 1) < 0.001) {
                    FadeAlpha(ref subTitleLeftImage, fadeSpeed);
                    FadeAlpha(ref subTitleRightImage, fadeSpeed);
                }
            }

            if (Math.Abs(subTitleLeftImage.color.a - 1) < 0.001 && !fadeInComplete) {
                fadeInComplete = true;

                SoundHandler.GetInstance().PlaySound(SoundName.Title, false, 0, out AudioClip clip);
                counter = clip.length;
            }

            counter -= Time.fixedDeltaTime;

            if (counter < 0 && fadeInComplete) {
                titleProgress = true;
            }

            if (titleProgress) {
                FadeAlpha(ref subTitleLeftImage, -(fadeSpeed));
                FadeAlpha(ref subTitleRightImage, -(fadeSpeed));
                FadeAlpha(ref titleImage, -(fadeSpeed));
            }

            if (titleProgress && Math.Abs(titleImage.color.a) < 0.001) {
                GetComponent<SceneChanger>().ChangeScene();
            }
        }

        public static void FadeAlpha(ref Image image, float amount) {
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, Mathf.Clamp(color.a + amount, 0, 1f));
        }
    }
}