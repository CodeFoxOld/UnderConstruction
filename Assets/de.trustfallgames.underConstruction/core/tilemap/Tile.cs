﻿using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Core.tilemap;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using Random = System.Random;

namespace de.TrustfallGames.UnderConstruction.Core.Tilemap {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshCollider))]
    public class Tile : MonoBehaviour, IInternUpdate {
        [SerializeField] private GameObject[] indicator;
        [SerializeField] private GameObject topIndicatorPrefab;
        [SerializeField] private float topInidicatorInterval;

        [SerializeField] private Material blockedTile;
        [SerializeField] private Material unblockedTile;

        [SerializeField] private GameObject door;

        private GameManager gameManager;

        private TileObstacle tileObstacle;
        private GameObject house;
        private GameObject obstacleBlueprint;
        private ApartmentPart apartmentPart;

        [SerializeField] private bool blocked;
        public bool Blocked => blocked;
        public TileCoord Coords { get; set; }
        private BoxCollider collider;

        private Counter _spawnCounter;
        private Counter _stackCounter;
        private Counter _earlySpawnCounter;

        private bool moving;
        private bool movingDown;
        private int destructQueue;

        private Tile() { }

        public Tile(int x, int z) { Coords = new TileCoord(x, z); }

        void Start() {
            gameManager = GameManager.GetManager();
            foreach (var obj in indicator) {
                obj.SetActive(false);
            }

            RegisterInternUpdate();
        }

        public void InternUpdate() {
            if (destructQueue != 0 && !moving && !movingDown) {
                Destruct();
                destructQueue--;
            }

            EarlySpawnRoutine();

            if (_spawnCounter != null && _spawnCounter.CheckMarker(0)) {
                if (!GameManager.GetManager().Character.CurrentCoord.Equals(Coords)) {
                    SetBlocked(true);
                }
            }

            if (_spawnCounter != null && _spawnCounter.Check()) {
                SpawnObject();
            }

            if (moving) {
                Move();
            }

            if (blocked) {
                if (_stackCounter.Check()) {
                    if (tileObstacle.ObstacleType == ObstacleType.House)
                        Stack();
                }
            }

            if (movingDown) {
                MoveDown();
            }
        }

        //MOVEMENT
        private void Move() {
            house.transform.Translate(0, 1 / (GameManager.GetManager().Settings.MoveUpSpeed * 60), 0);
            if (ObstacleData.Stage == 1 && ObstacleData.ObstacleType == ObstacleType.House) {
                door.transform.Translate(0, 1 / (GameManager.GetManager().Settings.MoveUpSpeed * 60), 0);
            }

            if (Math.Abs(house.transform.position.y) < 0.01) {
                moving = false;
                house.transform.position = new Vector3(Coords.X, 0, Coords.Z);
                if (ObstacleData.Stage == 1 && ObstacleData.ObstacleType == ObstacleType.House) {
                    door.transform.position = new Vector3(Coords.X, 0, Coords.Z);
                }
            }
        }

        private void MoveDown() {
            house.transform.Translate(
                                      0,
                                      -1 / (GameManager.GetManager().Settings.MoveUpSpeed * (1 / Time.fixedDeltaTime)),
                                      0);

            if (ObstacleData.Stage == 0 && ObstacleData.ObstacleType == ObstacleType.House) {
                door.transform.Translate(
                                         0,
                                         -1 / (GameManager.GetManager().Settings.MoveUpSpeed
                                               * (1 / Time.fixedDeltaTime)), 0);
            }

            if (house.transform.childCount != 0 && house.transform.GetChild(0).transform.childCount != 0) {
                if (Math.Abs(house.transform.GetChild(0).position.y) < 0.01) {
                    movingDown = false;
                    var a = house;
                    if (house.transform.childCount > 0) {
                        house = house.transform.GetChild(0).gameObject;
                        house.transform.parent = null;
                        Destroy(a);
                        house.transform.position = new Vector3(Coords.X, 0, Coords.Z);
                    } else {
                        SetBlocked(false);
                    }
                }
            } else {
                if (house.transform.position.y < -1) {
                    movingDown = false;
                    door.transform.position = new Vector3(Coords.X, -1, Coords.Z);
                    Destroy(house);
                    SetBlocked(false);
                }
            }
        }

        //SPAWN ROUTINES

        private void EarlySpawnRoutine() {
            if (!SpawnInProgress) return;

            if (gameManager.Character.CurrentCoord.Equals(Coords) && _earlySpawnCounter == null
                && !gameManager.Character.Moving) {
                _earlySpawnCounter = new Counter(gameManager.Settings.EarlySpawnTime, false);
            }

            if (_earlySpawnCounter != null && _earlySpawnCounter.Check()
                && gameManager.Character.CurrentCoord.Equals(Coords) && !gameManager.Character.Moving) {
                _spawnCounter = null;
                _earlySpawnCounter = null;
                SpawnObject();
            }

            if (_earlySpawnCounter != null && !gameManager.Character.CurrentCoord.Equals(Coords)) {
                _earlySpawnCounter = null;
            }
        }

        public void InitialiseSpawnObject(ObstacleData obstacleData, GameObject obstacleBlueprint,
            ApartmentStack apartmentStack) {
            _stackCounter = new Counter(gameManager.Settings.GetGrowInterval());
            apartmentPart = apartmentStack.draw();
            ShowIndicator();
            Debug.Log(obstacleData.ToString());
            this.obstacleBlueprint = obstacleBlueprint;
            _spawnCounter = new Counter(
                                        gameManager.Settings.GetSpawnDuration(), false,
                                        gameManager.Settings.MoveDuration + gameManager.Settings.RotationDuration
                                        + (Time.fixedDeltaTime * 2));
            if (tileObstacle == null) {
                tileObstacle = new TileObstacle(obstacleData);
            }
        }

        private void SpawnObject() {
            HideIndicator();
            if (gameManager.Character.CurrentCoord.Equals(Coords)) {
                /*Player is on Field. concat new object on Player*/
                //Concat object on Player
                gameManager.Character.Stack(apartmentPart);
                tileObstacle = null;
            } else /*Player is not on Field. Create or Concat new object*/ {
                Stack();
                SetBlocked(true); //Field is now blocked
                CheckCharacterMoveAbility();
            }
        }

        private void Stack() {
            SoundHandler.GetInstance().PlaySound(SoundName.HouseStack);
            GameObject b = house;
            if (b != null) {
                tileObstacle.AddStage(); //Count Stage 1 up
            }

            house = Instantiate(obstacleBlueprint);                                                  //Create Blueprint
            house.transform.position = new Vector3(Coords.X, -1, Coords.Z);                          //Assign under tile
            house.GetComponent<MeshFilter>().mesh = tileObstacle.GetObstacleObjectDataStaged().Mesh; //Assign mesh
            house.GetComponent<MeshRenderer>().material =
                tileObstacle.GetObstacleObjectDataStaged().Material; //Assign material
            if (ObstacleData.ObstacleType == ObstacleType.House && ObstacleData.Stage == 1) {
                var go = Instantiate(topIndicatorPrefab, house.transform, true);
                go.transform.position = new Vector3(Coords.X, -1, Coords.Z);
                go.GetComponent<TopIndicator>().Init(this);
            }

            if (b != null) {
                b.transform.SetParent(house.transform); //set old parent as Child
            }

            _stackCounter = new Counter(gameManager.Settings.GetGrowInterval());

            moving = true; //Start moving
            CheckBuildingHeight();
        }

        public void Destruct() {
            if (movingDown || moving) {
                destructQueue++;
                return;
            }

            if (house == null) return;
            Debug.Log("Destruct Triggered at " + Coords.ToString());
            if (movingDown) return;
            tileObstacle.TakeStage();
            _stackCounter.Reset();
            movingDown = true;
        }

        private void SetBlocked(bool state) {
            blocked = state;
            if (!state) {
                gameObject.GetComponent<MeshRenderer>().material = unblockedTile;
                tileObstacle = null;
                return;
            }

            gameObject.GetComponent<MeshRenderer>().material = blockedTile;
        }

        //SPAWN INDICATOR
        private void ShowIndicator() {
            SpawnInProgress = true;
            foreach (var obj in indicator) {
                obj.SetActive(transform);
                obj.GetComponent<MeshRenderer>().material.color =
                    gameManager.MapManager.ApartmentColor.GetColor(apartmentPart.ApartmentColorType);
                obj.GetComponent<MeshRenderer>()
                   .material.SetColor(
                                      "_EmissionColor",
                                      gameManager.MapManager.ApartmentColor.GetColor(
                                                                                     apartmentPart
                                                                                         .ApartmentColorType));
            }
        }

        private void HideIndicator() {
            SpawnInProgress = false;
            foreach (var obj in indicator) {
                obj.SetActive(false);
            }
        }

        //LOSE CONDITIONS
        private void CheckBuildingHeight() {
            if (tileObstacle.Stage > GameManager.GetManager().Settings.BuildingHeight) {
                Debug.Log("Lose triggered by " + gameObject.name + " height: " + tileObstacle.Stage);
                GameManager.GetManager().Lose();
            }
        }

        private void CheckCharacterMoveAbility() {
            TileCoord coord = gameManager.Character.CurrentCoord;

            List<TileCoord> directions = new List<TileCoord> {
                                                                 coord.NextTileCoord(MoveDirection.up),
                                                                 coord.NextTileCoord(MoveDirection.right),
                                                                 coord.NextTileCoord(MoveDirection.down),
                                                                 coord.NextTileCoord(MoveDirection.left)
                                                             };
            bool check = true;
            foreach (var obj in directions) {
                if (!check) break;
                if (obj == null) continue;
                Tile tile = gameManager.MapManager.GetTile(obj);
                if (tile == null) continue;
                check = gameManager.MapManager.GetTile(obj).blocked;
            }

            if (!check) return;
            if (gameManager.Character.DestructibleCount == 0) {
                Debug.Log("You Lose because you are no more able to move!");
                gameManager.Lose();
            }
        }

        private void OnValidate() {
            GetComponent<BoxCollider>().enabled = blocked;
            GetComponent<MeshCollider>().enabled = !blocked;
        }

        public void CheckCollider() {
            collider = GetComponent<BoxCollider>();
            collider.size = new Vector3(10, 5, 10);
        }

        public void RegisterInternUpdate() { gameManager.InternTick.RegisterTickObject(this, 50); }

        public void Init() { }

        public bool IsCounterInProgress() { return _spawnCounter != null && _spawnCounter.Current > 0; }

        public TileObstacle ObstacleData => tileObstacle;
        public int StepValue { get; set; }
        public bool Visited { get; set; }
        public bool SpawnInProgress { get; private set; }
        public float TopInidicatorInterval => topInidicatorInterval;
        
        public void OnDestroy() {
            gameManager.InternTick.RemoveTickObject(this);
        }

    }

    public enum ObstacleType { House, NotHouse }
}