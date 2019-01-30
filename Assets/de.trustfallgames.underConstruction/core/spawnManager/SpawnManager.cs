using System.Collections.Generic;
using de.trustfallgames.underConstruction.core.tilemap;
using de.trustfallgames.underConstruction.util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace de.trustfallgames.underConstruction.core.spawnManager {
    public class SpawnManager : MonoBehaviour {
        [SerializeField] private GameObject[] apartmentParts;
        [SerializeField] private GameObject[] obstacleStacks;
        [SerializeField] private GameObject obstacleBlueprint;

        private readonly Dictionary<ApartmentColor, ApartmentStack> apartmentStacks =
            new Dictionary<ApartmentColor, ApartmentStack>();

        private readonly List<ObstacleData> obstacles = new List<ObstacleData>();

        [Range(1, 60)]
        [SerializeField]
        private float spawnInterval = 10;

        private Counter counter;
        private MapManager _mapManager;

        // Start is called before the first frame update
        private void Start() {
            BuildDictionary();
            BuildObstacleData();
            counter = new Counter(spawnInterval);
            _mapManager = GameManager.GetManager().MapManager;
        }

        private void BuildDictionary() {
            foreach (GameObject part in apartmentParts) {
                if (!apartmentStacks.ContainsKey(part.GetComponent<apartmentPart>().ApartmentColor)
                ) /*create new dictionary entry*/ {
                    ApartmentStack stack = new ApartmentStack(
                                                              apartmentParts,
                                                              part.GetComponent<apartmentPart>().ApartmentColor);
                }
            }
        }

        // Update is called once per frame
        void Update() {
            if (counter.Next()) {
                StartNewSpawnRoutine();
            }
        }

        private void StartNewSpawnRoutine() {
            var x = Random.Range(0, _mapManager.XDimension);
            var y = Random.Range(0, _mapManager.YDimension);

            var a = _mapManager.GetTile(_mapManager.GetCoordForEasyCoord(x, y));
            while (a.ObstacleData != null && a.ObstacleData.ObstacleType == ObstacleType.NotHouse) {
                x = Random.Range(0, _mapManager.XDimension);
                y = Random.Range(0, _mapManager.YDimension);

                a = _mapManager.GetTile(_mapManager.GetCoordForEasyCoord(x, y));
                
            }

            if (a.Blocked) {
                ObstacleData obstacleData = null;
                foreach (ObstacleData VARIABLE in obstacles) {
                    if (VARIABLE.Id == a.ObstacleData.Id)
                        obstacleData = VARIABLE;
                }
                a.InitialiseSpawnObject(obstacleData, obstacleBlueprint);
            }
            
            a.InitialiseSpawnObject(obstacles[Random.Range(0, obstacles.Count)], obstacleBlueprint);
        }

        private void BuildObstacleData() {
            int i = 0;

            foreach (var obstacle in obstacleStacks) {
                ObstacleStack obstacleStack = obstacle.GetComponent<ObstacleStack>();
                foreach (var material in obstacleStack.Materials) {
                    obstacles.Add(new ObstacleData(i, material, obstacleStack.Mesh, obstacleStack.ObstacleType));
                    i++;
                }
            }
        }
    }
}