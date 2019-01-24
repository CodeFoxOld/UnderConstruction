using System;
using UnityEngine;
using de.trustfallgames.targetsenior.util;

namespace de.trustfallgames.targetsenior.character {
    public class Controller : MonoBehaviour {
        private bool      test;
        private Character _character;
        private Movement  _movement;

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
                    _movement.StartMove(MoveDirection.UpLeft);
                    return;
                case "upright":
                    _movement.StartMove(MoveDirection.UpRight);
                    return;
                case "downleft":
                    _movement.StartMove(MoveDirection.DownLeft);
                    return;
                case "downright":
                    _movement.StartMove(MoveDirection.DownRight);
                    return;
            }

            throw new ArgumentException(moveDirection + " is not a valid move direction.");

            //_movement.StartMove(moveDirection);
        }
    }
}