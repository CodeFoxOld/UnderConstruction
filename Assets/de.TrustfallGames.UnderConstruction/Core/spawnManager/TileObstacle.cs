using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.SpawnManager {
    public class TileObstacle {
        private int stage = 1;
        private Material material;
        private Mesh mesh;
        private ObstacleType obstacleType;

        public TileObstacle(ObstacleData data) {
            material = data.Material;
            mesh = data.Mesh;
            obstacleType = data.ObstacleType;
        }

        public void AddStage() { stage++; }

        public void TakeStage() { stage--; }

        public int Stage => stage;

        public override string ToString() {
            return (" Materialname: " + material.name + " Meshname: " + mesh.name + " ObstacleType: " + obstacleType);
        }

        public Material Material => material;
        public Mesh Mesh => mesh;
        public ObstacleType ObstacleType => obstacleType;
    }
}