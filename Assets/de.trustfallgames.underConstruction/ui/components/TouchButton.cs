using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace de.trustfallgames.underConstruction.ui.components {
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("CustomUI/TouchButton")]
    [ExecuteInEditMode]
    public class TouchButton : Button {
        private ButtonState _buttonState = ButtonState.NotPressed;
        private EventTrigger _eventTrigger;

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
            _eventTrigger = gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
        }

        void Update() { }
    }

    
}