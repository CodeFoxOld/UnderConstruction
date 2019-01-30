﻿using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.core.tilemap;
using UnityEngine;

public class TileCoordComparer : IEqualityComparer<TileCoord> {
    private readonly IEqualityComparer<int> _baseComparer;

    public bool Equals(TileCoord tileA, TileCoord tileB) {
        return tileA.X == tileB.X && tileA.Z == tileB.Z;
    }

    public int GetHashCode(TileCoord obj) { return 0; }
}