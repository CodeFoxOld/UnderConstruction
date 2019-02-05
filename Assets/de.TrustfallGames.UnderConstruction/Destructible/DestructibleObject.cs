using System;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.UI;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Destructible {
    public class DestructibleObject : MonoBehaviour {
        private GameManager _gameManager;
        private MapManager _mapManager;
        private Character _character;
        private DestructibleDirection _direction;
        private Vector3 _directionVector3;
        private bool[] destroyed;
        private bool start;
        private TileCoord _startCoord;
        private TileCoord _endCoord;
        private TileCoord[] destructibleArray;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() {
            if (!start) return;
            gameObject.transform.Translate(_directionVector3);
            if (_direction == DestructibleDirection.vertical) {
                
                
            } else {
                
                
            }
        }

        public DestructibleObject SetGameManager(GameManager gameManager) {
            _gameManager = gameManager;
            return this;
        }

        public DestructibleObject SetMapManager(MapManager mapManager) {
            _mapManager = mapManager;
            return this;
        }

        public DestructibleObject SetCharacter(Character character) {
            _character = character;
            return this;
        }

        public DestructibleObject SetDirection(DestructibleDirection direction) {
            _direction = direction;
            SetDirectionVector3();
            return this;
        }

        public void Init() {
            start = true;
            destroyed = _direction == DestructibleDirection.vertical ? new bool[_mapManager.XDimension] :
                            new bool[_mapManager.YDimension];
            if (_direction == DestructibleDirection.vertical) {
                _startCoord = new TileCoord(0 - (_mapManager.XDimension / 2), _character.CurrentCoord.Z);
                _endCoord = new TileCoord(_startCoord.X + _mapManager.XDimension + 1, _startCoord.Z);
            } else {
                _startCoord = new TileCoord(_character.CurrentCoord.X, 0 - (_mapManager.YDimension / 2));
                _endCoord = new TileCoord(_startCoord.X, _startCoord.Z + _mapManager.YDimension + 1);
            }

            BuildDestructibleArray();

            //TODO: Transform to start point
        }

        private void BuildDestructibleArray() {
            destructibleArray = new TileCoord[_direction == DestructibleDirection.vertical ? _mapManager.XDimension :
                                                  _mapManager.YDimension];
            for (int i = 0; i < destructibleArray.Length; i++) {
                destructibleArray[i] = _direction == DestructibleDirection.vertical ?
                                           _startCoord.NextTileCoord(MoveDirection.right) :
                                           _startCoord.NextTileCoord(MoveDirection.up);
            }
        }

        private void SetDirectionVector3() {
            Vector3 dirVector = new Vector3();
            switch (_direction) {
                case DestructibleDirection.horizontal:
                    dirVector = new Vector3(1, 0, 0);
                    break;
                case DestructibleDirection.vertical:
                    dirVector = new Vector3(0, 0, 1);
                    break;
            }

            _directionVector3 = (dirVector * _gameManager.Settings.DestructibleMoveSpeed) * Time.fixedDeltaTime;
        }
    }
}