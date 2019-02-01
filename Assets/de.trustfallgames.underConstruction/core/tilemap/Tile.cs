using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

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
            if (_spawnCounter != null && _spawnCounter.Check()) {
                Debug.Log("Execute Object Spawn");
                SpawnObject();
            }

            if (moving) {
                Move();
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
            Debug.Log("Spawn new Object");
            if (GameManager.GetManager().Character.CurrentCoord.Equals(Coords)) {
                /*Player is on Field. concat new object on Player*/
                //Concat object on Player
                Debug.Log("Trying to Concat to Player");
                GameManager.GetManager().Character.Stack(apartmentPart);
                obstacleData = null;
                return;
            } else /*Player is not on Field. Create or Concat new object*/ {
                Debug.Log("Spawning new obstacle");
                var b = house;
                house = Instantiate(obstacleBlueprint);                              //Create Blueprint
                house.transform.position = new Vector3(Coords.X, -1, Coords.Z);      //Assign under tile
                house.GetComponent<MeshFilter>().mesh = obstacleData.Mesh;           //Assign mesh
                house.GetComponent<MeshRenderer>().material = obstacleData.Material; //Assign material
                if (b != null) {
                    b.transform.SetParent(house.transform); //set old parent as Child
                    obstacleData.AddStage();                //Count Stage 1 up
                }

                blocked = true; //Field is now blocked
                moving = true;  //Start moving
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
            apartmentPart = apartmentStack.draw();
            ShowIndicator();
            Debug.Log(obstacleData.ToString());
            this.obstacleBlueprint = obstacleBlueprint;
            _spawnCounter = new Counter(gameManager.Settings.SpawnDuration, false);
            if (this.obstacleData == null) {
                this.obstacleData = obstacleData;
            }
        }

        public void Stack(ApartmentStack apartmentStack) {
            apartmentPart = apartmentStack.draw();
            _spawnCounter = new Counter(gameManager.Settings.SpawnDuration, false);
        }

        private void ShowIndicator() {
            foreach (var obj in indicator) {
                obj.SetActive(transform);
                obj.GetComponent<MeshRenderer>().material.color = apartmentPart.Material.color;
                obj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", apartmentPart.Material.color);
            }
        }

        private void HideIndicator() {
            foreach (var obj in indicator) {
                obj.SetActive(false);
            }
        }

        public ObstacleData ObstacleData => obstacleData;
    }

    public enum ObstacleType { House, NotHouse }
}