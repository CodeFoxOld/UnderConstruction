using System;
using de.trustfallgames.targetsenior.tilemap;
using de.trustfallgames.targetsenior.util;
using UnityEngine;

namespace de.trustfallgames.targetsenior.character {
    [RequireComponent(typeof(Character))]
    public class Movement : MonoBehaviour {
        private Character _character;
        private Transform _charTransform;
        private Vector3   startPosition;
        private Vector3   targetPosition;

        [SerializeField] private float rotationDuration;

        private bool moveInProgress;
        private bool turned;
        private bool moved;

        //Rotation params
        private float rotationPerFrame;
        private float currentRotation;
        private float targetRotation;

        // Start is called before the first frame update
        void Start() {
            _character     = gameObject.GetComponent<Character>();
            _charTransform = _character.CharacterTransform.transform;
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
            _character.Player.Translate(GetDirectionVector(_character.CurrentMoveDirection) / 60);
            if (Math.Abs(targetPosition.x - _character.transform.position.x) < 0.01
                && Math.Abs(targetPosition.z - _character.transform.position.z) < 0.01) {
                moved          = true;
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
                _charTransform.localEulerAngles = new Vector3(-90, 0, _character.GetCurrentRotation());
                turned                          = true;
                Debug.Log("Turn complete");
                return;
            }

            currentRotation += rotationPerFrame;

            _charTransform.localEulerAngles = new Vector3(-90, 0, currentRotation);


            //IDEA: Rotation with MODULO 360 to keep range for calculation when rotation is done
        }

        /// <summary>
        /// Initialise the move
        /// </summary>
        /// <param name="moveDirection"></param>
        public void StartMove(MoveDirection moveDirection) {
            if (moveInProgress) return;
            CalcRot(moveDirection);
            startPosition  = _character.transform.position;
            targetPosition = startPosition + GetDirectionVector(moveDirection);
            moveInProgress = true;
            turned         = false;
            moved          = false;
        }

        /// <summary>
        /// Returns true if the field in the desired direction is valid and not blocked
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <returns></returns>
        public bool IsWayValid(MoveDirection moveDirection) {
            return true;
        }

        /// <summary>
        /// Returns a TileCord which contains the coords of the tile in the desired direction
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <returns></returns>
        private TileCoord GetTileCoordForDirection(MoveDirection moveDirection) {
            TileCoord coord = _character.CurrentCoord;
            switch (moveDirection) {
                case MoveDirection.UpLeft:
                    return new TileCoord(coord.X, coord.Z + 1);
                case MoveDirection.UpRight:
                    return new TileCoord(coord.X, coord.Z - 1);
                case MoveDirection.DownLeft:
                    return new TileCoord(coord.X - 1, coord.Z);
                case MoveDirection.DownRight:
                    return new TileCoord(coord.X + 1, coord.Z);
            }

            return new TileCoord(coord.X, coord.Z);
        }

        /// <summary>
        /// Calculates the rotation and cache the rotation data
        /// </summary>
        private void CalcRot(MoveDirection moveDirection) {
            float turnDegree = GetTurnDegree(_character.CurrentMoveDirection, moveDirection);
            currentRotation = _character.GetCurrentRotation();
            targetRotation  = currentRotation + turnDegree;

            if (turnDegree == 0) {
                return;
            }

            Debug.Log("targetRotation - currentRotation = " + (targetRotation - currentRotation));
            Debug.Log("Target Rotation: " + targetRotation + " Current Rotation: " + currentRotation);


            rotationPerFrame = turnDegree / (rotationDuration * 60);
            Debug.Log("Rotating by " + rotationPerFrame + " Degree per Frame");
            _character.CurrentMoveDirection = moveDirection;
        }

        public static float GetRotationValue(MoveDirection moveDirection) {
            switch (moveDirection) {
                case MoveDirection.UpLeft:    return 90;
                case MoveDirection.UpRight:   return 180;
                case MoveDirection.DownRight: return 270;
                case MoveDirection.DownLeft:  return 360;
            }

            return 0;
        }

        public static Vector3 GetDirectionVector(MoveDirection moveDirection) {
            switch (moveDirection) {
                case MoveDirection.UpLeft:    return new Vector3(0, 0, 1);
                case MoveDirection.UpRight:   return new Vector3(1, 0, 0);
                case MoveDirection.DownRight: return new Vector3(0, 0, -1);
                case MoveDirection.DownLeft:  return new Vector3(-1, 0, 0);
            }

            return new Vector3(0, 0, 0);
        }

        public static float GetTurnDegree(MoveDirection current, MoveDirection target) {
            if (current == MoveDirection.UpLeft) {
                switch (target) {
                    case MoveDirection.DownLeft:
                        return -90;
                    case MoveDirection.UpRight:
                        return 90;
                    case MoveDirection.DownRight:
                        return 180;
                }
            } else if (current == MoveDirection.UpRight) {
                switch (target) {
                    case MoveDirection.UpLeft:
                        return -90;
                    case MoveDirection.DownRight:
                        return 90;
                    case MoveDirection.DownLeft:
                        return 180;
                }
            } else if (current == MoveDirection.DownRight) {
                switch (target) {
                    case MoveDirection.UpRight:
                        return -90;
                    case MoveDirection.DownLeft:
                        return 90;
                    case MoveDirection.UpLeft:
                        return 180;
                }
            } else if (current == MoveDirection.DownLeft) {
                switch (target) {
                    case MoveDirection.DownRight:
                        return -90;
                    case MoveDirection.UpLeft:
                        return 90;
                    case MoveDirection.UpRight:
                        return 180;
                }
            }

            return 0;
        }
    }
}