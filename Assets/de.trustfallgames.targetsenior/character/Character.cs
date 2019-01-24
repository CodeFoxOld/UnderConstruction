using de.trustfallgames.targetsenior.tilemap;
using de.trustfallgames.targetsenior.util;
using UnityEngine;

namespace de.trustfallgames.targetsenior.character {
    [RequireComponent(typeof(Movement))]
    public class Character : MonoBehaviour {
        [Header("Whole Player Object")]
        [SerializeField] private Transform _player;
        [Header("Character Model")]
        [SerializeField] private Transform _character;
        private TileCoord     _currentCoord;
        private MoveDirection _moveDirection = MoveDirection.DownLeft;

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
    }
}