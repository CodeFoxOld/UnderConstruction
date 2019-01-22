using System;
using de.trustfallgames.targetsenior.tilemap;
using de.trustfallgames.targetsenior.util;
using UnityEngine;

namespace de.trustfallgames.targetsenior.character {
    [RequireComponent(typeof(Character))]
    public class Movement : MonoBehaviour {
        private Character _character;

        private bool moveInProgress = false;
        private bool turned = false;
        private bool moved = false;

        // Start is called before the first frame update
        void Start() {
            _character = gameObject.GetComponent<Character>();
        }

        // Update is called once per frame
        void Update() {
        }

        /// <summary>
        /// Moves Character forward
        /// </summary>
        /// <param name="moveDirection"></param>
        private void Move(MoveDirection moveDirection) {
        }

        /// <summary>
        /// Turns Character in the right direction
        /// </summary>
        /// <param name="moveDirection"></param>
        private void Turn(MoveDirection moveDirection) {
        }

        /// <summary>
        /// Initialise the move
        /// </summary>
        /// <param name="moveDirection"></param>
        public void StartMove(MoveDirection moveDirection) {
        }

        private bool IsWayBlocked(MoveDirection moveDirection) {
            return true;
        }

        private TileCoord GetTileCoordForDirection(MoveDirection moveDirection) {
            TileCoord coord = _character.CurrentCoord;
            switch(moveDirection) {
                case MoveDirection.Up:
                    return new TileCoord(coord.X, coord.Z + 1);
                case MoveDirection.Down:
                    return new TileCoord(coord.X, coord.Z - 1);
                case MoveDirection.Left:
                    return new TileCoord(coord.X - 1, coord.Z);
                case MoveDirection.Right:
                    return new TileCoord(coord.X + 1, coord.Z);
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
            }
        }
    }
}
