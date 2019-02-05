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
        private int position;
        private bool destructionInProgress;
        private TileCoord _charPos;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() {
            if (start) {
                gameObject.transform.Translate(_directionVector3);
                Tile tile = null;
                if (_direction == DestructibleDirection.vertical) {
                    if (gameObject.transform.position.x > destructibleArray[position].X) {
                        tile = _mapManager.GetTile(destructibleArray[position]);
                    }
                } else {
                    if (gameObject.transform.position.z > destructibleArray[position].Z) {
                        tile = _mapManager.GetTile(destructibleArray[position]);
                    }
                }

                if (tile != null) {
                    tile.Destruct();
                    destroyed[position] = true;
                    position++;
                }

                if (position == destructibleArray.Length - 1) {
                    StartDestroy();
                }
            }

            if (!start && destructionInProgress) {
                //TODO: Death animation
            }
        }

        private void StartDestroy() {
            start = false;
            destructionInProgress = true;
            Destroy(gameObject);
            //throw new NotImplementedException();
        }

        public DestructibleObject Setup(GameManager gameManager, MapManager mapManager, Character character,
            DestructibleDirection direction, TileCoord tileCoord) {
            _gameManager = gameManager;
            _mapManager = mapManager;
            _character = character;
            _direction = direction;
            SetDirectionVector3();
            _charPos = tileCoord;
            SetPosition();
            return this;
        }

        private void SetPosition() {
            gameObject.transform.position = _direction == DestructibleDirection.vertical ?
                                                new Vector3(0 - (_mapManager.XDimension / 2), 0, _charPos.Z) :
                                                new Vector3(_charPos.X, 0, 0 - (_mapManager.YDimension / 2));
        }

        public void Init() {
            start = true;

            destroyed = _direction == DestructibleDirection.vertical ? new bool[_mapManager.XDimension] :
                            new bool[_mapManager.YDimension];
            if (_direction == DestructibleDirection.vertical) {
                _startCoord = new TileCoord(0 - (_mapManager.XDimension / 2), _charPos.Z);
                _endCoord = new TileCoord(_startCoord.X + _mapManager.XDimension + 1, _startCoord.Z);
            } else {
                _startCoord = new TileCoord(_charPos.X, 0 - (_mapManager.YDimension / 2));
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