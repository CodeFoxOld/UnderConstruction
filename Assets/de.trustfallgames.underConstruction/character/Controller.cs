using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Util;
using de.TrustfallGames.UnderConstruction.UI.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Character {
    public class Controller : MonoBehaviour,IInternUpdate {
        private                  bool      test;
        [SerializeField] private Character _character;
        [SerializeField] private Movement  _movement;

        private bool movingUp;
        private bool movingRight;
        private bool movingDown;
        private bool movingLeft;

        private void Start() { RegisterInternUpdate(); }

        /// <summary>
        /// Input interface for movement
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="state"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        public Character Character {
            set {
                _character = value;
                _movement  = value.Movement;
            }
        }

        public void InternUpdate() {             if (movingUp)
                _movement.StartMove(MoveDirection.up);
            if(movingRight)
                _movement.StartMove(MoveDirection.right);
            if(movingDown)
                _movement.StartMove(MoveDirection.down);
            if (movingLeft) 
                _movement.StartMove(MoveDirection.left);
        }

        /// <summary>
        /// Registers intern update object at hive
        /// </summary>
        public void RegisterInternUpdate() { GameManager.GetManager().InternTick.RegisterTickObject(this, 25); }

        public void Init() { throw new NotImplementedException(); }
        
        /// <summary>
        /// Destroys the current instance
        /// </summary>
        public void OnDestroy() {
            GameManager.GetManager().InternTick.UnregisterTickObject(this);
        }

    }
}