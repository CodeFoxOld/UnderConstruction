﻿using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using Random = System.Random;

namespace de.TrustfallGames.UnderConstruction.Core.Tilemap {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshCollider))]
    public class Tile : MonoBehaviour {
        [SerializeField] private GameObject[] indicator;
        private GameManager gameManager;

        private ObstacleData obstacleData;
        private GameObject house;
        private GameObject obstacleBlueprint;
        private ApartmentPart apartmentPart;

        [SerializeField] private bool blocked;
        public bool Blocked => blocked;
        public TileCoord Coords { get; set; }
        private BoxCollider collider;

        private Counter _spawnCounter;
        private Counter _stackCounter;

        private bool moving;

        private Tile() { }

        public Tile(int x, int z) { Coords = new TileCoord(x, z); }

        // Start is called before the first frame update
        void Start() {
            gameManager = GameManager.GetManager();
            foreach (var obj in indicator) {
                obj.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update() {
            if (_spawnCounter != null && _spawnCounter.CheckMarker(0)) {
                if (!GameManager.GetManager().Character.CurrentCoord.Equals(Coords)) {
                    blocked = true;
                }
            }

            if (_spawnCounter != null && _spawnCounter.Check()) {
                Debug.Log("Execute Object Spawn");
                SpawnObject();
            }

            if (moving) {
                Move();
            }

            if (blocked) {
                if (_stackCounter.Check()) {
                    Stack();
                }
            }
        }

        private void Move() {
            house.transform.Translate(0, 1 / (GameManager.GetManager().Settings.MoveUpSpeed * 60), 0);
            if (Math.Abs(house.transform.position.y) < 0.01) {
                moving = false;
                house.transform.position = new Vector3(Coords.X, 0, Coords.Z);
            }
        }

        private void SpawnObject() {
            HideIndicator();
            if (GameManager.GetManager().Character.CurrentCoord.Equals(Coords)) {
                /*Player is on Field. concat new object on Player*/
                //Concat object on Player
                GameManager.GetManager().Character.Stack(apartmentPart);
                obstacleData = null;
            } else /*Player is not on Field. Create or Concat new object*/ {
                Stack();
                blocked = true; //Field is now blocked
                CheckCharacterMoveAbility();
            }
        }

        private void CheckCharacterMoveAbility() {
            TileCoord coord = gameManager.Character.CurrentCoord;
            List<TileCoord> neighbours = gameManager.MapManager.GetTile(coord.NextTileCoord(MoveDirection.up))
                                                    .Coords.Neighbours;
            if (gameManager.MapManager.GetTile(neighbours[0]).blocked
                && gameManager.MapManager.GetTile(neighbours[0]).blocked
                && gameManager.MapManager.GetTile(neighbours[0]).blocked
                && gameManager.MapManager.GetTile(neighbours[0]).blocked) {
                if (gameManager.Character.DestructibleCount == 0) {
                    gameManager.Lose();
                }
            }
        }

        private void Stack() {
            GameObject b = house;
            house = Instantiate(obstacleBlueprint);                              //Create Blueprint
            house.transform.position = new Vector3(Coords.X, -1, Coords.Z);      //Assign under tile
            house.GetComponent<MeshFilter>().mesh = obstacleData.Mesh;           //Assign mesh
            house.GetComponent<MeshRenderer>().material = obstacleData.Material; //Assign material
            if (b != null) {
                b.transform.SetParent(house.transform); //set old parent as Child
                obstacleData.AddStage();                //Count Stage 1 up
            }

            var a = GameManager.GetManager().Settings.GetGrowSpeed();
            _stackCounter = new Counter(UnityEngine.Random.Range(a.Min, a.Max));

            moving = true; //Start moving
            if (obstacleData.Stage > GameManager.GetManager().Settings.BuildingHight) {
                GameManager.GetManager().Lose();
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

        public void InitialiseSpawnObject(ObstacleData obstacleData, GameObject obstacleBlueprint,
            ApartmentStack apartmentStack) {
            var a = GameManager.GetManager().Settings.GetGrowSpeed();
            _stackCounter = new Counter(UnityEngine.Random.Range(a.Min, a.Max));
            apartmentPart = apartmentStack.draw();
            ShowIndicator();
            Debug.Log(obstacleData.ToString());
            this.obstacleBlueprint = obstacleBlueprint;
            _spawnCounter = new Counter(
                                        gameManager.Settings.SpawnDuration, false,
                                        gameManager.Settings.MoveDuration + gameManager.Settings.RotationDuration
                                        + (Time.fixedDeltaTime * 2));
            if (this.obstacleData == null) {
                this.obstacleData = obstacleData;
            }
        }

        private void ShowIndicator() {
            SpawnInProgress = true;
            foreach (var obj in indicator) {
                obj.SetActive(transform);
                obj.GetComponent<MeshRenderer>().material.color = apartmentPart.Material.color;
                obj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", apartmentPart.Material.color);
            }
        }

        private void HideIndicator() {
            SpawnInProgress = false;
            foreach (var obj in indicator) {
                obj.SetActive(false);
            }
        }

        public ObstacleData ObstacleData => obstacleData;

        public int StepValue { get; set; }
        public bool Visited { get; set; }

        public bool IsCounterInProgress() { return _spawnCounter.Current > 0; }

        public bool SpawnInProgress { get; private set; }
    }

    public enum ObstacleType { House, NotHouse }
}