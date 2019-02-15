using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace de.TrustfallGames.UnderConstruction.Core.SpawnManager {
    public class SpawnManager : MonoBehaviour, IInternUpdate {
        [SerializeField] private GameObject[] apartmentParts;

        [FormerlySerializedAs("obstacleStacks")] [SerializeField]
        private GameObject[] obstacleParts;

        [SerializeField] private GameObject obstacleBlueprint;

        private readonly Dictionary<ApartmentColorType, ApartmentStack> apartmentStacks =
            new Dictionary<ApartmentColorType, ApartmentStack>();

        private List<ObstacleData> notHouse = new List<ObstacleData>();
        private List<ObstacleData> house = new List<ObstacleData>();

        private List<Tile> spawns = new List<Tile>();

        private Counter counter;
        private MapManager _mapManager;
        private GameManager _gameManager;

        private Character _character;

        // Start is called before the first frame update

        private void Start() {
            BuildDictionary();
            BuildObstacleData();
            counter = new Counter(GameManager.GetManager().Settings.GetSpawnInterval());
            _gameManager = GameManager.GetManager();
            _mapManager = _gameManager.MapManager;
            _character = _gameManager.Character;
            RegisterInternUpdate();
        }

        public void InternUpdate() {
            if (counter.Check()) {
                StartNewSpawnRoutine();
                counter.Reset(_gameManager.Settings.GetSpawnInterval());
            }

            CheckSpawns();
        }

        private void CheckSpawns() {
            bool temp = false;
            foreach (Tile tile in spawns) {
                if (!tile.IsCounterInProgress()) continue;
                temp = true;
                break;
            }

            if (temp) return;
            counter.Reset(_gameManager.Settings.GetSpawnInterval());
            StartNewSpawnRoutine();
        }

        private void BuildDictionary() {
            foreach (GameObject part in apartmentParts) {
                if (!apartmentStacks.ContainsKey(part.GetComponent<ApartmentPart>().ApartmentColorType)) {
                    /*create new dictionary entry*/
                    ApartmentStack stack = new ApartmentStack(
                                                              apartmentParts,
                                                              part.GetComponent<ApartmentPart>().ApartmentColorType);
                    apartmentStacks.Add(part.GetComponent<ApartmentPart>().ApartmentColorType, stack);
                }
            }
        }

        private void StartNewSpawnRoutine() {
            TilesStack tilesStack = GetClassifiedTiles();


            Tile[] tiles = GetSpawnTiles(tilesStack, CalculateStages());

            if (tiles.Length < CalculateStages()) return;

            for (int i = tiles.Length - 1; i >= 0; i--) {
                if (tiles[i] == null) continue;
                if (i == tiles.Length - 1 && _character.LatestColorType != ApartmentColorType.None) {
                    //Salted Random. Creates sometimes a color
                    if (Random.Range(0, 101) < _gameManager.Settings.SaltGrains) {
                        //Spawn tile with other color than latest player color
                        tiles[i]
                            .InitialiseSpawnObject(
                                                   GetRandomObstacleData(), obstacleBlueprint,
                                                   apartmentStacks
                                                       [GetRandomColorExceptOne(_character.LatestColorType)]);
                        spawns.Add(tiles[i]);
                    } else /*create field with latest player color*/ {
                        tiles[i]
                            .InitialiseSpawnObject(
                                                   GetRandomObstacleData(), obstacleBlueprint,
                                                   apartmentStacks[_character.LatestColorType]);
                        spawns.Add(tiles[i]);


                        //When the player has no field and/or the tile is not the farest tile
                    }
                } else /*Generates field with random color, which is not the same than the latest player color*/ {
                    tiles[i]
                        .InitialiseSpawnObject(
                                               GetRandomObstacleData(), obstacleBlueprint,
                                               apartmentStacks[GetRandomColorExceptOne(_character.LatestColorType)]);
                    spawns.Add(tiles[i]);
                }
            }
        }

        private int CalculateStages() {
            int maxHs = _gameManager.Settings.HighScoreMax;
            int hs = _character.Highscore;
            int fields = _gameManager.Settings.MaxField - 1;
            int steps = maxHs / fields;
            if (hs == 0) {
                return _gameManager.Settings.MinField;
            }

            int amount = hs / steps;
            if (amount <= _gameManager.Settings.MinField) {
                return _gameManager.Settings.MinField;
            }

            return (hs / steps) + 1;
        }

        private ApartmentColorType GetRandomColorExceptOne(ApartmentColorType color) {
            ApartmentColorType apartmentColorType = (ApartmentColorType) Random.Range(0, 4);
            while (apartmentColorType == color) {
                apartmentColorType = (ApartmentColorType) Random.Range(0, 4);
            }

            return apartmentColorType;
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

            if (stack.CountRated() > stages) {
                float ratio = stack.CountRated() * (1f / stages); //Make ratio for stages
                for (int i = 1; i <= stages; i++) {
                    tiles[i - 1] = stack.DrawRated((int) Math.Ceiling((i * ratio)));
                    while (tiles[i - 1].SpawnInProgress) {
                        tiles[i - 1] = stack.DrawRated((int) Math.Ceiling((i * ratio)));
                    }
                }
            } else {
                for (int i = 1; i <= stages; i++) {
                    tiles[i - 1] = stack.DrawUnrated();
                    if(tiles[i - 1]) break;
                    while (tiles[i - 1].SpawnInProgress) {
                        tiles[i - 1] = stack.DrawUnrated();
                        if(tiles[i - 1] == null) break;
                    }
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

        private ObstacleData GetRandomObstacleData() {
            return Random.Range(0, 101) < _gameManager.Settings.HousePercentage ? house[Random.Range(0, house.Count)] :
                       notHouse[Random.Range(0, notHouse.Count)];
        }

        private void BuildObstacleData() {
            foreach (GameObject obstacle in obstacleParts) {
                if (obstacle.GetComponent<ObstaclePart>().ObstacleType == ObstacleType.House) {
                    List<ObstacleData> tempObstacles = ObstacleData.Builder(obstacle.GetComponent<ObstaclePart>());
                    tempObstacles.ForEach(entry => house.Add(entry));
                }

                if (obstacle.GetComponent<ObstaclePart>().ObstacleType == ObstacleType.NotHouse) {
                    List<ObstacleData> tempObstacles = ObstacleData.Builder(obstacle.GetComponent<ObstaclePart>());
                    tempObstacles.ForEach(entry => notHouse.Add(entry));
                }
            }
        }

        public void RegisterInternUpdate() { _gameManager.InternTick.RegisterTickObject(this, 10); }

        public void Init() { }
    }
}