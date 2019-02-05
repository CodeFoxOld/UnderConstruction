using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace de.TrustfallGames.UnderConstruction.Core.SpawnManager {
    public class TilesStack {
        private Dictionary<int, RatedTiles> ratedTiles = new Dictionary<int, RatedTiles>();
        private List<Tile> unratedTiles = new List<Tile>();

        public void AddRated(Tile tile) {
            if (ratedTiles.ContainsKey(tile.StepValue)) {
                ratedTiles[tile.StepValue].Add(tile);
            } else {
                ratedTiles.Add(tile.StepValue, new RatedTiles(tile));
            }
        }

        public void AddUnrated(Tile tile) { unratedTiles.Add(tile); }

        /// <summary>
        /// Draws a random unrated tile. Returns null if no unrated tile is available for spawning
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [CanBeNull] public Tile DrawRated(int score) {
            if (!ratedTiles.ContainsKey(score - 1)) {
                throw new ArgumentOutOfRangeException("Score is " + score + " and Tiles Count is " + ratedTiles.Count);
            }

            if (score - 1 == 0) {
                return ratedTiles[1].Draw();
            }

            Tile temp = ratedTiles[score - 1].Draw();
            if (temp == null) {
                if (ratedTiles[score].Draw() != null) {
                    return ratedTiles[score].Draw();
                }

                if ((score - 2) != 0 && ratedTiles[score - 2].Draw() != null) {
                    return ratedTiles[score - 2].Draw();
                }
            }

            return temp;
        }

        public int CountRated() { return ratedTiles.Count; }

        public int CountUnrated() { return unratedTiles.Count; }

        /// <summary>
        /// Draws a random unrated tile. Returns null if no unrated tile is available for spawning
        /// </summary>
        /// <returns></returns>
        [CanBeNull] public Tile DrawUnrated() {
            List<Tile> spawnableTiles = new List<Tile>();
            foreach (var obj in unratedTiles) {
                if (obj.SpawnInProgress) continue;

                spawnableTiles.Add(obj);
                break;
            }

            return spawnableTiles.Count == 0 ? null : spawnableTiles[Random.Range(0, spawnableTiles.Count)];
        }

        [CanBeNull] public Tile DrawAny() {
            foreach (RatedTiles tilesValue in ratedTiles.Values) {
                foreach (Tile tile in tilesValue.Tiles) {
                    if (tile.SpawnInProgress) continue;
                    return tile;
                }
            }

            foreach (Tile tile in unratedTiles) {
                if (tile.SpawnInProgress) continue;
                return tile;
            }

            return null;
        }
    }
}