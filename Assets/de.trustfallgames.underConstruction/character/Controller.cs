using System;
using de.trustfallgames.underConstruction.util;
using UnityEngine;

namespace de.trustfallgames.underConstruction.character {
    public class Controller : MonoBehaviour {
        private bool      test;
        private Character _character;
        private Movement  _movement;
        
        private bool movingUp;
        private bool movingRight;
        private bool movingDown;
        private bool movingLeft;

        // Start is called before the first frame update
        void Start() {
            _character = gameObject.GetComponent<Character>();
            _movement  = gameObject.GetComponent<Movement>();
        }

        // Update is called once per frame
        void Update() { }

        public void OnButtonClick(String moveDirection) {
            Debug.Log("Input for direction " + moveDirection);
            switch (moveDirection) {
                case "upleft":
                    _movement.StartMove(MoveDirection.up);
                    return;
                case "upright":
                    _movement.StartMove(MoveDirection.right);
                    return;
                case "downleft":
                    _movement.StartMove(MoveDirection.left);
                    return;
                case "downright":
                    _movement.StartMove(MoveDirection.down);
                    return;
            }

            throw new ArgumentException(moveDirection + " is not a valid move direction.");

            //_movement.StartMove(moveDirection);
        }
        
        public void ButtonToggle(MoveDirection direction, bool state) {
            switch (direction) {
                case MoveDirection.up:
                    movingUp = state;
                    break;
                case MoveDirection.right:
                    movingRight = state;
                    break;
                case MoveDirection.down:
                    movingDown = state;
                    break;
                case MoveDirection.left:
                    movingLeft = state;
                    break;
                default: throw new ArgumentOutOfRangeException("direction", direction, null);
            }

    }
}