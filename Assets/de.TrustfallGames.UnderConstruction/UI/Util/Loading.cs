using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.Util {
    [RequireComponent(typeof(TransitionImage))]
    public class Loading : MonoBehaviour {
        
        private RectTransform rectTransform;
        private TransitionImage transitionImage;
        [SerializeField] private Image loadingTransitionObject;
        
        [SerializeField] 

        private float moveDistancePerFrame;

        private Vector3 end;
        
        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            transitionImage = GetComponent<TransitionImage>();
            var position = rectTransform.position;
            position = new Vector3(-(Screen.width / 2) - 300, position.y, position.z);
            Vector3 start;
            rectTransform.position = start = position;

            end = new Vector3((Screen.width / 2) + 300, position.y, position.z);

            moveDistancePerFrame = (Vector3.Distance(start, end)) / (transitionImage.WaitDuration / Time.fixedDeltaTime);
        }

        private void FixedUpdate() {
            var position = rectTransform.position;
            position = new Vector3(position.x + moveDistancePerFrame, position.y, position.z);
            rectTransform.position = position;

            if (rectTransform.position.x > end.x) {
                Destroy(gameObject);
            }
            
            //if(rectTransform.position.x > )
        }
    }
}