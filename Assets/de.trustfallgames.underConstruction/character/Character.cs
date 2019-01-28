using de.trustfallgames.underConstruction.tilemap;
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

        public TileCoord CurrentCoord {
            get { return _currentCoord; }
            set { _currentCoord = value; }
        }

        public MoveDirection MoveDirection {
            get { return _moveDirection; }
            set { _moveDirection = value; }
        }

        public float GetCurrentRotation() {
            return Movement.GetRotationValue(_moveDirection);
        }

        public MoveDirection CurrentMoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

        public Transform CharacterTransform { get { return _character; } }

        public Transform Player { get { return _player; } }

        private void Start() {
            _controller = gameObject.GetComponent<Controller>();
        }

        public Controller Controller { get { return _controller; } }

        private Character() { }

        public static Character getCharacter() {
            return GameObject.Find("Character").GetComponent<Character>();
        }
    }
}

