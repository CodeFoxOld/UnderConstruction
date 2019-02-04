using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Image))]
public abstract class InteractableButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    // Start is called before the first frame update
    private Image _image;
    private bool pressed;

    private void Start() {
        _image = GetComponent<Image>();
    }

    public void OnPointerUp(PointerEventData eventData) {
        pressed = true;
        OnButtonReleased(eventData);
    }

    public void OnPointerDown(PointerEventData eventData) {
        pressed = false;
        OnButtonPressed(eventData);
    }

    /// <summary>
    /// Returns the state of the button as boolean
    /// </summary>
    /// <returns>True when button is pressed, false when not</returns>
    public bool ButtonState() {
        return pressed;
    }

    protected abstract void OnButtonPressed(PointerEventData eventData);

    protected abstract void OnButtonReleased(PointerEventData eventData);
}