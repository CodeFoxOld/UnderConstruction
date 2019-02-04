using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;


[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Image))]
public abstract class InteractableButton : MonoBehaviour, ISubmitHandler,IEventHandler,ISelectHandler {
    // Start is called before the first frame update
    private bool pressed = false;

    private bool pressedLastFrame = false;

    private void Start() { }

    // Update is called once per frame
    private void Update() {
        Touch[] touches = Input.touches;

        if (Input.touchCount <= 0) return;
        pressed = false;
        if (Input.touches.Any(touch => EventSystem.current.IsPointerOverGameObject(touch.fingerId))) {
            pressed = true;
            if (!pressedLastFrame) {
                OnTouchStart();
                pressedLastFrame = true;
            }
        }

        if (pressed || !pressedLastFrame) return;

        OnTouchEnd();
        pressedLastFrame = false;
    }

    public abstract void OnTouchStart();

    public abstract void OnTouchEnd();

    public bool Pressed() {
        return pressed;
    }

    public void OnSubmit(BaseEventData eventData) { throw new System.NotImplementedException(); }

    public void SendEvent(EventBase e) { throw new System.NotImplementedException(); }

    public void HandleEvent(EventBase evt) { throw new System.NotImplementedException(); }

    public bool HasTrickleDownHandlers() { throw new System.NotImplementedException(); }

    public bool HasBubbleUpHandlers() { throw new System.NotImplementedException(); }

    public bool HasCaptureHandlers() { throw new System.NotImplementedException(); }

    public bool HasBubbleHandlers() { throw new System.NotImplementedException(); }

    public void OnSelect(BaseEventData eventData) { throw new System.NotImplementedException(); }
}