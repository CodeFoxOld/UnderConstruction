using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.character;
using de.trustfallgames.underConstruction.util;
using UnityEngine;
using UnityEngine.UI;

public class MobileButton : InteractableButton {
    [SerializeField] private MoveDirection moveDirection;

    private Controller _controller;
    // Start is called before the first frame update
    void Start() {
        _controller = Character.getCharacter().Controller;
    }

    // Update is called once per frame
    void Update() { }

    public override void OnTouchStart() {
        _controller.ButtonToggle(moveDirection, true);
    }

    public override void OnTouchEnd() {
        _controller.ButtonToggle(moveDirection,false);
    }
}