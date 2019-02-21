using System;
using de.TrustfallGames.UnderConstruction.SoundManager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.Util {
    [RequireComponent(typeof(TransitionImage))]
    public class Loading : MonoBehaviour {
        private RectTransform rectTransform;
        private TransitionImage transitionImage;
        [SerializeField] private Image loadingTransitionObject;

        [SerializeField] private bool playSound;
        [SerializeField] private SoundName playSoundName;

        [Range(-4f, 4f)]
        [SerializeField]
        private float soundOffset;

        private float moveDistancePerFrame;

        private float playSoundAtPosition;

        private Vector3 end;
        private bool soundPlayed;

        private void Start() {
            transitionImage = GetComponent<TransitionImage>();
            rectTransform = loadingTransitionObject.GetComponent<RectTransform>();
            var position = rectTransform.localPosition;
            position = new Vector3(-(Screen.width / 2) - 400, position.y, position.z);
            Vector3 start;
            rectTransform.localPosition = start = position;

            end = new Vector3((Screen.width / 2) + 400, position.y, position.z);

            moveDistancePerFrame = (Vector3.Distance(start, end))
                                   / (transitionImage.WaitDuration
                                      / TransitionBitch.GetInstance().AverageFixedDeltaTime);


            if (playSound) {
                var length = SoundHandler.GetInstance().GetSoundLength(playSoundName);
                var a = (0 - (length + soundOffset * 2) / 2);
                playSoundAtPosition =
                    0 - ((a / TransitionBitch.GetInstance().AverageFixedDeltaTime) * moveDistancePerFrame);
            }
        }

        private void FixedUpdate() {
            if (rectTransform == null) return;
            var position = rectTransform.localPosition;
            position = new Vector3(position.x + moveDistancePerFrame, position.y, position.z);
            rectTransform.localPosition = position;

            if (rectTransform.localPosition.x > end.x) {
                Destroy(loadingTransitionObject.gameObject);
            }


            if (soundPlayed) return;

            if (rectTransform.localPosition.x > playSoundAtPosition) {
                SoundHandler.GetInstance().PlaySound(playSoundName);
                soundPlayed = true;
            }
        }
    }
}