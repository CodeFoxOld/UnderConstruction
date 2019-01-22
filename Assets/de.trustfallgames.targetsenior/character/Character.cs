using de.trustfallgames.targetsenior.tilemap;
using de.trustfallgames.targetsenior.util;
using UnityEngine;

namespace de.trustfallgames.targetsenior.character {
    [RequireComponent(typeof(Movement))]
    public class Character {
        private TileCoord _currentCoord;
        private MoveDirection _moveDirection;

        public TileCoord CurrentCoord {
            get {return _currentCoord;}
            set {_currentCoord = value;}
        }

        public MoveDirection MoveDirection {
            get {return _moveDirection;}
            set {_moveDirection = value;}
        }
    }
}
