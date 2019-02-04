using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ClassifiedTiles {
        private List<Tile> tiles = new List<Tile>();

        public Tile Draw() { return tiles[Random.Range(0, tiles.Count)]; }

        public void Add(Tile tile) { tiles.Add(tile); }

        public ClassifiedTiles(Tile tile) { tiles.Add(tile); }
    }
}