using System;
using de.trustfallGames.underConstruction.core.tilemap;
using de.trustfallGames.underConstruction.util;
using UnityEngine;

namespace de.trustfallGames.underConstruction.character {
    [RequireComponent(typeof(Character))]
    public class Movement : MonoBehaviour {
        [SerializeField] private Character _character;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private MapManager _mapManager;
        private Transform _charTransform;
        private Vector3 startPosition;
        private Vector3 targetPosition;

        [Range(0.01f, 1)]
        [SerializeField]
        private float rotationDuration = 0.1f;

        [Range(0.01f, 1)]
        [SerializeField]
        private float moveDuration = 0.1f;

        private bool moveInProgress;
        private bool turned;
        private bool moved;

        //Rotation params
        private float rotationPerFrame;
        private float currentRotation;
        private float targetRotation;

        // Start is called before the first frame update
        void Start() {
            _character = gameObject.GetComponent<Character>();
            _charTransform = _character.CharacterTransform.transform;
            _gameManager = GameManager.GetManager();
            _mapManager = _gameManager.MapManager;
        }

        // Update is called once per frame
        void Update() {
            if (!moveInProgress) return;
            if (!turned) {
                Turn();
                return;
            }

            if (!moved) {
                Move();
                return;
            }

            moveInProgress = false;
        }

        /// <summary>
        /// Moves Character forward
        /// </summary>
        /// <param name="moveDirection"></param>
        private void Move() {
            _character.Player.Translate(GetDirectionVector(_character.CurrentMoveDirection) / (moveDuration * 60));
            if (Math.Abs(targetPosition.x - _character.transform.position.x) < 0.01
                && Math.Abs(targetPosition.z - _character.transform.position.z) < 0.01) {
                moved = true;
                moveInProgress = false;
                _character.transform.position = targetPosition;
            }
        }

        /// <summary>
        /// Turns Character in the right direction
        /// </summary>
        /// <param name="moveDirection"></param>
        private void Turn() {
            if (Math.Abs(targetRotation - currentRotation) < 0.1) {
                _charTransform.localEulerAngles = new Vector3(0, _character.GetCurrentRotation(), 0);
                turned = true;
                return;
            }

            currentRotation += rotationPerFrame;

            _charTransform.localEulerAngles = new Vector3(0, currentRotation, 0);
        }

        /// <summary>
        /// Initialise the move
        /// </summary>
        /// <param name="moveDirection"></param>
        public void StartMove(MoveDirection moveDirection) {
            if (_mapManager.FieldBlocked(_character.CurrentCoord, moveDirection) ||_character.Moving) {
                return;
            }

            if (moveInProgress) return;
            _character.CurrentCoord = _character.CurrentCoord.NextTileCoord(moveDirection);
            CalcRot(moveDirection);
            startPosition = _character.transform.position;
            targetPosition = startPosition + GetDirectionVector(moveDirection);
            moveInProgress = true;
            turned = false;
            moved = false;
        }

        /// <summary>
        /// Returns a TileCord which contains the coords of the tile in the desired direction
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <returns></returns>
        private TileCoord GetTileCoordForDirection(MoveDirection moveDirection) {
            TileCoord coord = _character.CurrentCoord;
            switch (moveDirection) {
                case MoveDirection.up:
                    return new TileCoord(coord.X , coord.Z +1);
                case MoveDirection.right:
                    return new TileCoord(coord.X +1 , coord.Z);
                case MoveDirection.left:
                    return new TileCoord(coord.X - 1, coord.Z);
                case MoveDirection.down:
                    return new TileCoord(coord.X, coord.Z -1);
            }

            return new TileCoord(coord.X, coord.Z);
        }

        /// <summary>
        /// Calculates the rotation and cache the rotation data
        /// </summary>
        private void CalcRot(MoveDirection moveDirection) {
            float turnDegree = GetTurnDegree(_character.CurrentMoveDirection, moveDirection);
            currentRotation = _character.GetCurrentRotation();
            targetRotation = currentRotation + turnDegree;

            if (turnDegree == 0) {
                return;
            }

            rotationPerFrame = turnDegree / (rotationDuration * 60);
            _character.CurrentMoveDirection = moveDirection;
        }

        public static float GetRotationValue(MoveDirection moveDirection) {
            switch (moveDirection) {
                case MoveDirection.up: return 90;
                case MoveDirection.right: return 180;
                case MoveDirection.down: return 270;
                case MoveDirection.left: return 360;
            }

            return 0;
        }

        public static Vector3 GetDirectionVector(MoveDirection moveDirection) {
            switch (moveDirection) {
                case MoveDirection.up: return new Vector3(0, 0, 1);
                case MoveDirection.right: return new Vector3(1, 0, 0);
                case MoveDirection.down: return new Vector3(0, 0, -1);
                case MoveDirection.left: return new Vector3(-1, 0, 0);
            }

            return new Vector3(0, 0, 0);
        }

        public static float GetTurnDegree(MoveDirection current, MoveDirection target) {
            if (current == MoveDirection.up) {
                switch (target) {
                    case MoveDirection.left:
                        return -90;
                    case MoveDirection.right:
                        return 90;
                    case MoveDirection.down:
                        return 180;
                }
            } else if (current == MoveDirection.right) {
                switch (target) {
                    case MoveDirection.up:
                        return -90;
                    case MoveDirection.down:
                        return 90;
                    case MoveDirection.left:
                        return 180;
                }
            } else if (current == MoveDirection.down) {
                switch (target) {
                    case MoveDirection.right:
                        return -90;
                    case MoveDirection.left:
                        return 90;
                    case MoveDirection.up:
                        return 180;
                }
            } else if (current == MoveDirection.left) {
                switch (target) {
                    case MoveDirection.down:
                        return -90;
                    case MoveDirection.up:
                        return 90;
                    case MoveDirection.right:
                        return 180;
                }
            }

            return 0;
        }

        public Transform CharTransform { get => _charTransform; set => _charTransform = value; }

    }
    
    
    
    
}