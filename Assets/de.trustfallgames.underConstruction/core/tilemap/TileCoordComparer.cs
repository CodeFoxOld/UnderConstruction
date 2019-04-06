using System.Collections.Generic;

namespace de.TrustfallGames.UnderConstruction.Core.tilemap {
    /// <summary>
    /// Comparer to compare tile coords
    /// </summary>
    public class TileCoordComparer : IEqualityComparer<TileCoord> {
        private readonly IEqualityComparer<int> _baseComparer;

        public bool Equals(TileCoord tileA, TileCoord tileB) {
            if (tileA == null || tileB == null) return false;
            return tileA.X == tileB.X && tileA.Z == tileB.Z;
        }

        public int GetHashCode(TileCoord obj) { return 0; }
    }
}