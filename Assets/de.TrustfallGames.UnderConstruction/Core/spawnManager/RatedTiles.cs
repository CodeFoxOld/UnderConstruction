using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.tilemap;
using JetBrains.Annotations;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class RatedTiles {
        private List<Tile> tiles = new List<Tile>();

        [CanBeNull] public Tile Draw() {
            if (tiles.Count == 0) return null;
            return tiles[Random.Range(0, tiles.Count)];
        }

        public void Add(Tile tile) { tiles.Add(tile); }

        public RatedTiles(Tile tile) { tiles.Add(tile); }

        public List<Tile> Tiles => tiles;
    }
    
    
}