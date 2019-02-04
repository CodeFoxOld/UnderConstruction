using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using UnityEngine;
using Random = UnityEngine.Random;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ClassifiedTilesStacks {
        private Dictionary<int, ClassifiedTiles> tiles = new Dictionary<int, ClassifiedTiles>();
        private List<Tile> unclassified = new List<Tile>();

        public void AddClassified(Tile tile) {
            if (tiles.ContainsKey(tile.StepValue)) {
                tiles[tile.StepValue].Add(tile);
            } else {
                tiles.Add(tile.StepValue, new ClassifiedTiles(tile));
            }
        }

        public void AddUnclassified(Tile tile) {
            unclassified.Add(tile);
        }

        public Tile DrawClassified(int score) {
            if (!tiles.ContainsKey(score - 1)) {
                throw new ArgumentOutOfRangeException("Score is " + score + " and Tiles Count is " + tiles.Count);
            }

            if (score - 1 == 0) {
                return tiles[1].Draw();
            }
            Debug.Log("Try to get index: " + (score));
            return tiles[score - 1].Draw();
        }

        public int Count() { return tiles.Count; }

        public Tile DrawUnclassified() {
            Tile temp = unclassified[Random.Range(0, unclassified.Count)];
            List<Tile> tiles = new List<Tile>();
            foreach (var obj in unclassified) {
                if (!obj.SpawnInProgress) {
                    tiles.Add(temp);
                }
            }
            return tiles[Random.Range(0, tiles.Count)];
        }
    }
}