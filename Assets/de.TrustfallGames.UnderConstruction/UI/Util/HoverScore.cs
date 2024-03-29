using System;
using de.TrustfallGames.UnderConstruction.util.SceneChanger;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.UI.Util {
    /// <summary>
    /// A Hover score behaviour, which moves in a direction with a specified speed and text
    /// </summary>
    public class HoverScore : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private float speed = 1;
        [SerializeField] private float duration = 1.5f;
        [SerializeField] private float fadeOutTime = 0.01f;
        private float currentDuration;
        private bool _move;

        /// <summary>
        /// Inits the object. Sets the Text and starts moving
        /// </summary>
        /// <param name="text"></param>
        public void Init(string text) {
            Debug.Log($"Init with text: {text}");
            textMeshPro.text = text;
            _move = true;
        }

        private void Update() {
            if(!_move) return;
            
            currentDuration += Time.deltaTime;
            if (currentDuration < duration) {
                var pos = transform.position;
                pos.y += speed;
                transform.position = pos;
            } else {
                var col = textMeshPro.color;
                var time = 1 / (fadeOutTime * (1 / Time.deltaTime));
                col.a = Mathf.Clamp(col.a - time, 0, 1);
                textMeshPro.color = col;
                if (Math.Abs(col.a) < 0.001) {
                    Destroy(gameObject);
                }
            }
        }
    }
}