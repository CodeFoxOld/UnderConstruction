using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace de.TrustfallGames.UnderConstruction.Core.SpawnManager {
    public class SpawnManager : MonoBehaviour {
        [SerializeField] private GameObject[] apartmentParts;

        [FormerlySerializedAs("obstacleStacks")] [SerializeField]
        private GameObject[] obstacleParts;

        [SerializeField] private GameObject obstacleBlueprint;

        private readonly Dictionary<ApartmentColor, ApartmentStack> apartmentStacks =
            new Dictionary<ApartmentColor, ApartmentStack>();

        [SerializeField] private List<ObstacleData> obstacles = new List<ObstacleData>();

        private Counter counter;
        private MapManager _mapManager;
        private GameManager _gameManager;
        private Character _character;

        private void Awake() { }

        // Start is called before the first frame update
        private void Start() {
            BuildDictionary();
            BuildObstacleData();
            counter = new Counter(GameManager.GetManager().Settings.SpawnInterval);
            _gameManager = GameManager.GetManager();
            _mapManager = _gameManager.MapManager;
            _character = _gameManager.Character;
        }

        private void BuildDictionary() {
            foreach (GameObject part in apartmentParts) {
                if (!apartmentStacks.ContainsKey(part.GetComponent<ApartmentPart>().ApartmentColor)) {
                    /*create new dictionary entry*/
                    ApartmentStack stack = new ApartmentStack(
                                                              apartmentParts,
                                                              part.GetComponent<ApartmentPart>().ApartmentColor);
                    apartmentStacks.Add(part.GetComponent<ApartmentPart>().ApartmentColor, stack);
                }
            }
        }

        // Update is called once per frame
        void Update() {
            if (counter.Check()) {
                StartNewSpawnRoutine();
            }
        }

        private void StartNewSpawnRoutine() {
            TilesStack tilesStack = GetClassifiedTiles();

            Tile[] tiles = GetSpawnTiles(tilesStack, 2);

            if (tiles.Length < 2) return;

            for (int i = tiles.Length - 1; i >= 0; i--) {
                if (tiles[i] == null) continue;
                if (i == tiles.Length - 1) {
                    tiles[i]
                        .InitialiseSpawnObject(
                                               obstacles[Random.Range(0, obstacles.Count)], obstacleBlueprint,
                                               apartmentStacks[_character.LatestColor]);
                } else {
                    ApartmentColor apartmentColor = (ApartmentColor) Random.Range(0, 3);
                    while (apartmentColor == _character.LatestColor) {
                        apartmentColor = (ApartmentColor) Random.Range(0, 3);
                    }

                    tiles[i]
                        .InitialiseSpawnObject(
                                               obstacles[Random.Range(0, obstacles.Count)], obstacleBlueprint,
                                               apartmentStacks[apartmentColor]);
                }
            }
        }

        //TODO: Implement spawning amount with player size
        /// <summary>
        /// Returns an array with the length of the stages. The lowest index is the nearest field
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="stages"></param>
        /// <returns></returns>
        private Tile[] GetSpawnTiles(TilesStack stack, int stages) {
            Tile[] tiles = new Tile[stages]; //Inits Array with stages size

            if (stack.CountRated() > 2) {
                float ratio = stack.CountRated() * (1f / stages); //Make ratio for stages
                for (int i = 1; i <= stages; i++) {
                    tiles[i - 1] = stack.DrawRated((int) Math.Ceiling((i * ratio)));
                }
            } else {
                for (int i = 1; i <= stages; i++) {
                    tiles[i - 1] = stack.DrawUnrated();
                }
            }

            return tiles;
        }

        /// <summary>
        /// Iterates over the field. Returns the tiles classified and sorted for quick access.
        /// </summary>
        /// <returns></returns>
        private TilesStack GetClassifiedTiles() {
            TilesStack stack = new TilesStack();
            Queue<Tile> tiles = new Queue<Tile>();
            var temp = _mapManager.GetTile(_character.CurrentCoord);
            temp.StepValue = 0;
            temp.Visited = true;
            tiles.Enqueue(temp);

            while (tiles.Count != 0) {
                var a = tiles.Dequeue();
                stack.AddRated(a);
                List<TileCoord> directions = new List<TileCoord> {
                                                                     a.Coords.NextTileCoord(MoveDirection.up),
                                                                     a.Coords.NextTileCoord(MoveDirection.right),
                                                                     a.Coords.NextTileCoord(MoveDirection.down),
                                                                     a.Coords.NextTileCoord(MoveDirection.left)
                                                                 };
                foreach (var tileCoord in directions) {
                    Tile tile = _mapManager.GetTile(tileCoord);
                    if (tile == null) continue;
                    if (tile.Visited || tile.Blocked) continue;

                    tile.StepValue = a.StepValue + 1;
                    tile.Visited = true;
                    tiles.Enqueue(tile);
                }
            }

            foreach (var tile in _mapManager.Tiles.Values) {
                if (!tile.Visited && !tile.Blocked) {
                    stack.AddUnrated(tile);
                }

                tile.Visited = false;
            }

            return stack;
        }

        private void BuildObstacleData() {
            foreach (GameObject obstacle in obstacleParts) {
                List<ObstacleData> tempObstacles = ObstacleData.Builder(obstacle.GetComponent<ObstaclePart>());
                tempObstacles.ForEach(entry => obstacles.Add(entry));
            }
        }
    }
}