using System;
using de.trustfallgames.underConstruction.util;
using UnityEngine;

namespace de.trustfallgames.underConstruction.core.tilemap {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshCollider))]
    public class Tile : MonoBehaviour {
        private GameManager gameManager;

        private ObstacleData obstacleData;
        private GameObject house;
        private GameObject obstacleBlueprint;

        [SerializeField] private bool blocked;
        public bool Blocked => blocked;
        public TileCoord Coords { get; set; }
        private BoxCollider collider;

        private Counter _spawnCounter;

        private bool moving;

        private Tile() { }

        public Tile(int x, int z) {
            Coords = new TileCoord(x, z);
        }

        // Start is called before the first frame update
        void Start() {
            gameManager = GameManager.GetManager();
        }

        // Update is called once per frame
        void Update() {
            if (_spawnCounter != null && _spawnCounter.Next()) {
                SpawnObject();
            }

            if (moving) {
                Move();
            }
        }

        private void Move() {
            gameObject.transform.Translate(0, 1 / (GameManager.GetManager().Settings.MoveUpSpeed * 60), 0);
            if (Math.Abs(gameObject.transform.position.y) < 0.01) {
                moving = false;
                gameObject.transform.position = new Vector3(Coords.X, 0, Coords.Z);
            }
        }

        private void SpawnObject() {
            if (GameManager.GetManager().Character.CurrentCoord.Equals(Coords)
            ) /*Player is on Field. concat new object on Player*/ {
                //Concat object on Player
            } else /*Player is not on Field. Create or Concat new object*/ {
                var b = house;
                house = Instantiate(obstacleBlueprint);
                house.transform.position = new Vector3(Coords.X, -1, Coords.Z);
                house.GetComponent<MeshFilter>().mesh = obstacleData.Mesh;
                house.GetComponent<Material>().mainTexture = obstacleData.Material.mainTexture;
                if (b != null) {
                    b.transform.SetParent(house.transform);
                }

                moving = true;
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

        public void InitialiseSpawnObject(ObstacleData obstacleData, GameObject obstacleBlueprint) {
            this.obstacleBlueprint = obstacleBlueprint;
            _spawnCounter = new Counter(gameManager.Settings.SpawnDuration, false);
            var a = Instantiate(obstacleBlueprint);
            if (this.obstacleData == null) {
                this.obstacleData = obstacleData;
            }
        }

        public ObstacleData ObstacleData => obstacleData;
    }

    public enum ObstacleType {
        House,
        NotHouse
    }
}