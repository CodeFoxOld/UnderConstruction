using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.Tilemap {
    public class ObstacleData {
        private int id;
        private Material material;
        private Mesh mesh;
        private ObstacleType obstacleType;
        private int stage = 1;

        public ObstacleData(int id, Material material, Mesh mesh, ObstacleType obstacleType) {
            if (material == null)
                throw new NullReferenceException();
            if (mesh == null)
                throw new NullReferenceException();
            this.id = id;
            this.material = material;
            this.mesh = mesh;
            this.obstacleType = obstacleType;
        }

        public int Id => id;
        public Material Material => material;
        public Mesh Mesh => mesh;

        public ObstacleType ObstacleType => obstacleType;

        public override string ToString() {
            return ("ID: " + id + " Materialname: " + material.name + " Meshname: " + mesh.name + " ObstacleType: "
                    + obstacleType);
        }

        public void AddStage() {
            stage++;
        }

        public void TakeStage() {
            stage--;
        }

        public int Stage => stage;
    }
}