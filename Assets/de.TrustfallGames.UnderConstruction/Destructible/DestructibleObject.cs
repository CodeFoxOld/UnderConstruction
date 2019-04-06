using System;
using System.Collections;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.tilemap;
using de.TrustfallGames.UnderConstruction.Core.Util;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.UI;
using de.TrustfallGames.UnderConstruction.UI.Core;
using de.TrustfallGames.UnderConstruction.UI.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Destructible {
    public class DestructibleObject : MonoBehaviour, IInternUpdate {
        private GameManager _gameManager;
        private MapManager _mapManager;
        private Character.Character _character;
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
        private int destructionCount = 1;

        private void Start() { RegisterInternUpdate(); }

        /// <summary>
        /// Checks if the object is at the end of the lane
        /// </summary>
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

        /// <summary>
        /// Checks if the object passed a tile.
        /// </summary>
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

        /// <summary>
        /// destroys the current tile if it can be destroyed
        /// </summary>
        private void DestroyCurrentTile() {
            if (_mapManager.GetTile(destructibleArray[position]) != null) {
                var tile = _mapManager.GetTile(destructibleArray[position]);
                if (tile.ObstacleData != null) {
                    tile.Destruct(destructionCount, out bool success);
                    if(success) destructionCount++;
                }

                destroyed[position] = true;
                position++;
            }
        }

        /// <summary>
        /// Starts the destroy coroutine, which removes the destructible 
        /// </summary>
        private void StartDestroy() {
            start = false;
            destructionInProgress = true;
            GetComponent<MeshFilter>().mesh = null;
            var emission = GetComponentInChildren<ParticleSystem>().emission;
            emission.enabled = false;
            StartCoroutine(DestroyAfterTime(3));
        }

        /// <summary>
        /// Plays the car Sound after a specified time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator PlayCarSound(float time) {
            yield return new WaitForSeconds(time);
            start = true;
            SoundHandler.GetInstance().PlaySound(SoundName.BulldozerMove, false, GetInstanceID());
        }

        /// <summary>
        /// Setup Method for destructible. Is needed to be called, otherwise the object will not work
        /// </summary>
        /// <param name="gameManager"></param>
        /// <param name="mapManager"></param>
        /// <param name="character"></param>
        /// <param name="direction"></param>
        /// <param name="tileCoord"></param>
        /// <returns></returns>
        public DestructibleObject Setup(GameManager gameManager, MapManager mapManager, Character.Character character,
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

        /// <summary>
        /// Builds the borders of the field. Calculate start and end pos
        /// </summary>
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

        /// <summary>
        /// Sets the position of the object. Build Array must be executed before.
        /// </summary>
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

        /// <summary>
        /// Init of the object. Must be executed first
        /// </summary>
        public void Init() {
            SoundHandler.GetInstance().PlaySound(SoundName.Horn, false, GetInstanceID(), out AudioClip clip);
            StartCoroutine(PlayCarSound(clip.length - 0.2f));
        }

        /// <summary>
        /// Builds the destruction check array. required to remember the current positon
        /// </summary>
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

        /// <summary>
        /// Sets the direction Vector
        /// </summary>
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

        /// <summary>
        /// Destroys the object after an specified time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator DestroyAfterTime(int time) {
            WaitForSeconds wait = new WaitForSeconds(time);
            yield return wait;

            Destroy(gameObject);
        }
        
        /// <summary>
        /// Executed on object destroy
        /// </summary>
        public void OnDestroy() {
            _gameManager.InternTick.UnregisterTickObject(this);
        }

    }
}