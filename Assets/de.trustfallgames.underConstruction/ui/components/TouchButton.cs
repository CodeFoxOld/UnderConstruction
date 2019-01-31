using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace de.trustfallGames.underConstruction.ui.components {
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("CustomUI/TouchButton")]
    [ExecuteInEditMode]
    public class TouchButton : Button {
        private ButtonState _buttonState = ButtonState.NotPressed;

        public void onPress() {
            _buttonState = ButtonState.Pressed;
        }

        public void onRelease() {
            _buttonState = ButtonState.NotPressed;
        }


        public bool isButtonPressed() {
            return _buttonState == ButtonState.Pressed;
        }

        void Start() {
            targetGraphic = gameObject.GetComponent<Image>();
        }

        void Update() { }
    }

    
}