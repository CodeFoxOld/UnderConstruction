﻿using System;
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
                gameObject.transform.position = gameObject.transform.position + _directionVector3;

                DestroyRoutine();

                CheckDestroy();
            }

            if (!start && destructionInProgress) { }
        }

        private void CheckDestroy() {
            if (_direction == DestructibleDirection.vertical) {
                if (gameObject.transform.position.z > _endCoord.Z) {
                    StartDestroy();
                }
            } else {
                if (gameObject.transform.position.x > _endCoord.X) {
                    StartDestroy();
                }
            }
        }

        private void DestroyRoutine() {
            if (position >= destructibleArray.Length) return;
            if (_direction == DestructibleDirection.vertical) {
                if (gameObject.transform.position.z > destructibleArray[position].Z) {
                    Debug.Log("Trying to destroy tile at " + destructibleArray[position] + "on Z Axis");
                    DestroyCurrentTile();
                }
            } else {
                if (gameObject.transform.position.x > destructibleArray[position].X) {
                    Debug.Log("Trying to destroy tile at " + destructibleArray[position] + "on X Axis");
                    DestroyCurrentTile();
                }
            }
        }

        private void DestroyCurrentTile() {
            if (_mapManager.GetTile(destructibleArray[position]) != null) {
                var tile = _mapManager.GetTile(destructibleArray[position]);
                if (tile.ObstacleData != null) {
                    tile.Destruct();
                }

                destroyed[position] = true;
                position++;
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
            BuildArrays();
            SetPosition();
            return this;
        }

        private void BuildArrays() {
            destroyed = _direction == DestructibleDirection.vertical ? new bool[_mapManager.ZDimension] :
                            new bool[_mapManager.YDimension];
            if (_direction == DestructibleDirection.vertical) {
                _startCoord = new TileCoord(_charPos.X, 0 - (_mapManager.YDimension / 2) - 1);
                _endCoord = new TileCoord(_startCoord.X, _startCoord.Z + _mapManager.YDimension + 1);
            } else {
                _startCoord = new TileCoord(0 - (_mapManager.ZDimension / 2) - 1, _charPos.Z);
                _endCoord = new TileCoord(_startCoord.X + _mapManager.ZDimension + 1, _startCoord.Z);
            }

            BuildDestructibleArray();
        }

        private void SetPosition() {
            if (_direction == DestructibleDirection.vertical) {
                gameObject.transform.position = new Vector3(_startCoord.X, 0, _startCoord.Z );
                gameObject.transform.localEulerAngles = new Vector3(0,90,0);
            } else {
                gameObject.transform.position = new Vector3(_startCoord.X, 0, _startCoord.Z);
            }
        }

        public void Init() { start = true; }

        private void BuildDestructibleArray() {
            destructibleArray = new TileCoord[_direction == DestructibleDirection.vertical ? _mapManager.YDimension :
                                                  _mapManager.ZDimension];
            for (int i = 0; i < destructibleArray.Length; i++) {
                if (i == 0) {
                    destructibleArray[i] = _direction == DestructibleDirection.vertical ?
                                               _startCoord.NextTileCoord(MoveDirection.up) :
                                               _startCoord.NextTileCoord(MoveDirection.right);
                    continue;
                }

                destructibleArray[i] = _direction == DestructibleDirection.vertical ?
                                           destructibleArray[i - 1].NextTileCoord(MoveDirection.up) :
                                           destructibleArray[i - 1].NextTileCoord(MoveDirection.right);
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

            _directionVector3 = dirVector / (_gameManager.Settings.DestructibleMoveSpeed * (1 / Time.fixedDeltaTime));
        }
    }
}