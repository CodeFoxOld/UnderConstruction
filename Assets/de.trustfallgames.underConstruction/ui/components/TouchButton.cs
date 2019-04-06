using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace de.TrustfallGames.UnderConstruction.UI.components {
    /// <summary>
    /// Class for a touch button
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("CustomUI/TouchButton")]
    [ExecuteInEditMode]
    public class TouchButton : Button {
        private ButtonState _buttonState = ButtonState.NotPressed;

        public void OnPress() {
            _buttonState = ButtonState.Pressed;
        }

        public void OnRelease() {
            _buttonState = ButtonState.NotPressed;
        }


        public bool IsButtonPressed() {
            return _buttonState == ButtonState.Pressed;
        }

        void Start() {
            targetGraphic = gameObject.GetComponent<Image>();
        }

        void Update() { }
    }

    
}