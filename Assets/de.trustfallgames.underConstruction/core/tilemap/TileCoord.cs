namespace de.trustfallgames.underConstruction.tilemap {
    public class TileCoord {

        private TileCoord() {
        }

        public TileCoord(int x, int z) {
            this.x = x;
            this.z = z;
        }

        private int x;
        private int z;

        public int X {
            get {return x;}
            set {x = value;}
        }

        public int Z {
            get {return z;}
            set {z = value;}
        }

        public override bool Equals(object obj) {
            var a =(TileCoord)obj;
            return a.X == x && a.Z == z;
        }
    }
}
