using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using UnityEngine;

public class TileCoordComparer : IEqualityComparer<TileCoord> {
    private readonly IEqualityComparer<int> _baseComparer;

    public bool Equals(TileCoord tileA, TileCoord tileB) {
        if (tileA == null || tileB == null) return false;
        return tileA.X == tileB.X && tileA.Z == tileB.Z;
    }

    public int GetHashCode(TileCoord obj) { return 0; }
}