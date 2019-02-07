using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Util;

namespace de.TrustfallGames.UnderConstruction.Core.Tilemap {
    public class TileCoord {
        private TileCoord() {
            up = new TileCoord(x, z + 1);
            right = new TileCoord(x + 1, z);
            down = new TileCoord(x, z - 1);
            left = new TileCoord(x - 1, z);
            neighbours = new List<TileCoord>() {up, right, down, left};
        }

        public TileCoord(int x, int z) {
            this.x = x;
            this.z = z;
        }

        private int x;
        private int z;
        private TileCoord up;
        private TileCoord right;
        private TileCoord down;
        private TileCoord left;
        private List<TileCoord> neighbours;

        public int X { get { return x; } set { x = value; } }

        public int Z { get { return z; } set { z = value; } }

        public bool Equals(TileCoord a) { return a.X == x && a.Z == z; }

        public TileCoord NextTileCoord(MoveDirection moveDirection) {
            switch (moveDirection) {
                case MoveDirection.up:
                    return new TileCoord(x, z + 1);
                case MoveDirection.right:
                    return new TileCoord(x + 1, z);
                case MoveDirection.down:
                    return new TileCoord(x, z - 1);
                case MoveDirection.left:
                    return new TileCoord(x - 1, z);
                default: throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
            }
        }

        public List<TileCoord> Neighbours => neighbours;

        public override string ToString() { return "X:" + X + " | Z:" + Z; }
    }
}