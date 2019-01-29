using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoveDirection = de.trustfallgames.underConstruction.util.MoveDirection;

[RequireComponent(typeof(Image))]
public class MobileButton : InteractableButton {
    [SerializeField] private MoveDirection moveDirection;

    [SerializeField] private Controller _controller;

    // Start is called before the first frame update
    void Start() {
        _controller = GameManager.GetManager().Controller;
    }

    protected override void OnButtonPressed(PointerEventData eventData) {
        _controller.ButtonToggle(moveDirection, true);
    }

    protected override void OnButtonReleased(PointerEventData eventData) {
        _controller.ButtonToggle(moveDirection, false);
    }
}