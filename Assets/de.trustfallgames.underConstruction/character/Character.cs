using System;
using de.trustfallgames.underConstruction.core.tilemap;
using de.trustfallgames.underConstruction.util;
using UnityEngine;

namespace de.trustfallgames.underConstruction.character {
    [RequireComponent(typeof(Movement))]
    public class Character : MonoBehaviour {
        [Header("Whole Player Object")]
        [SerializeField]
        private Transform _player;

        [Header("Character Model")]
        [SerializeField]
        private Transform _character;

        private TileCoord     _currentCoord;
        private MoveDirection _moveDirection = MoveDirection.left;
        private Controller    _controller;
        private Movement      _movement;

        private void Start() {
            GameManager.GetManager().RegisterCharacter(this);
            _controller = GameManager.GetManager().Controller;
            _movement   = GetComponent<Movement>();
        }

        public Controller Controller { get { return _controller; } }

        private Character() { }

        public float GetCurrentRotation() {
            return Movement.GetRotationValue(_moveDirection);
        }

        public MoveDirection MoveDirection {
            get { return _moveDirection; }
            set { _moveDirection = value; }
        }

        public TileCoord CurrentCoord {
            get { return _currentCoord; }
            set { _currentCoord = value; }
        }

        public Transform CharacterTransform { get { return _character; } }

        public MoveDirection CurrentMoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

        public Movement Movement {
            get {
                if (_movement == null)
                    _movement = GetComponent<Movement>();
                return _movement;
            }
        }

        public Transform Player { get { return _player; } }
    }
}