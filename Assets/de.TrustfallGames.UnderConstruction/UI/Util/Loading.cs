using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Util {
    public class Loading : MonoBehaviour {
        private RectTransform rectTransform;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            var position = rectTransform.position;
            position = new Vector3(-(Screen.width / 2) - 300, position.y, position.z);
            rectTransform.position = position;
            Screen.width;
        }
    }
}