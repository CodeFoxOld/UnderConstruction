using de.TrustfallGames.UnderConstruction.util.SceneChanger;
using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.UI.Util {
    /// <summary>
    /// Handler to make tutorials. Skips thought slides as images
    /// </summary>
    public class TutorialHandler : MonoBehaviour {
        [SerializeField] private Image display;

        [SerializeField] private Sprite[] images;
        [SerializeField] private Sprite home;
        [SerializeField] private Sprite arrowForward;
        [SerializeField] private Sprite arrowBack;

        [SerializeField] private Button back;
        [SerializeField] private Button forward;

        private int index = 0;

        void Start() {
            display.sprite = images[index];
            ChangeButtonSprite(back, home);
        }

        public void ImageForward() {
            index++;
            if (index < images.Length) {
                ChangePicture();
            } else {
                GetComponent<SceneChanger>().ChangeScene();
            }
            CheckButtonImages();
        }

        public void ImageBack() {
            index--;
            if (index >= 0) {
                ChangePicture();
            } else {
                GetComponent<SceneChanger>().ChangeScene();
            }
            CheckButtonImages();
        }

        void CheckButtonImages() {
            ChangeButtonSprite(back, index <= 0 ? home : arrowBack);
            ChangeButtonSprite(forward, index >= images.Length - 1 ? home : arrowForward);
        }

        public void ChangePicture() { display.sprite = images[index]; }

        private void ChangeButtonSprite(Button b, Sprite s) { b.image.sprite = s; }
    }
}