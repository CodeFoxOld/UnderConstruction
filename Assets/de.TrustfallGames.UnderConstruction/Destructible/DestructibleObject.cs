using System;
using System.Collections;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.UI;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Destructible {
    public class DestructibleObject : MonoBehaviour, IInternUpdate {
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

        private void Start() { RegisterInternUpdate(); }

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
                    DestroyCurrentTile();
                }
            } else {
                if (gameObject.transform.position.x > destructibleArray[position].X) {
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
            GetComponent<MeshFilter>().mesh = null;
            var emission = GetComponentInChildren<ParticleSystem>().emission;
            emission.enabled = false;
            StartCoroutine(DestroyAfterTime(3));
        }

        private IEnumerator PlayCarSound(float time) {
            yield return new WaitForSeconds(time);
            start = true;
            SoundHandler.GetInstance().PlaySound(SoundName.BulldozerMove, false, GetInstanceID());
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
                gameObject.transform.position = new Vector3(_startCoord.X, 0, _startCoord.Z);
                gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
            } else {
                gameObject.transform.position = new Vector3(_startCoord.X, 0, _startCoord.Z);
            }
        }

        public void InternUpdate() {
            if (start) {
                gameObject.transform.position = gameObject.transform.position + _directionVector3;

                DestroyRoutine();

                CheckDestroy();
            }

            if (!start && destructionInProgress) { }
        }

        public void RegisterInternUpdate() { _gameManager.InternTick.RegisterTickObject(this, 60); }

        public void Init() {
            SoundHandler.GetInstance().PlaySound(SoundName.Horn, false, GetInstanceID(), out AudioClip clip);
            StartCoroutine(PlayCarSound(clip.length - 0.2f));
        }

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

        private IEnumerator DestroyAfterTime(int time) {
            WaitForSeconds wait = new WaitForSeconds(time);
            yield return wait;

            Destroy(gameObject);
        }
        
        public void OnDestroy() {
            _gameManager.InternTick.RemoveTickObject(this);
        }

    }
}