using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.ui.components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using MoveDirection = de.TrustfallGames.UnderConstruction.Util.MoveDirection;

namespace de.TrustfallGames.UnderConstruction.ui.components {
    [RequireComponent(typeof(Image))]
    public class MobileButton : InteractableButton {
        [SerializeField] private MoveDirection moveDirection;

        [SerializeField] private Controller controller;

        // Start is called before the first frame update
        void Start() { controller = GameManager.GetManager().Controller; }

        protected override void OnButtonPressed(PointerEventData eventData) {
            controller.ButtonToggle(moveDirection, true);
        }

        protected override void OnButtonReleased(PointerEventData eventData) {
            controller.ButtonToggle(moveDirection, false);
        }
    }
}