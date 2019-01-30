using System;
using de.trustfallgames.underConstruction.util;

namespace de.trustfallgames.underConstruction.core.tilemap {
    public class TileCoord {
        private TileCoord() { }

        public TileCoord(int x, int z) {
            this.x = x;
            this.z = z;
        }

        private int x;
        private int z;

        public int X {
            get { return x; }
            set { x = value; }
        }

        public int Z {
            get { return z; }
            set { z = value; }
        }

        public bool Equals(TileCoord a) {
            return a.X == x && a.Z == z;
        }

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
        public override string ToString() { return "X:" + X + " | Z:" + Z; }

    }
}