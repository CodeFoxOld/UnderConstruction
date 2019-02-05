using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.Tilemap {
    public class ObstacleData {
        private Material material;
        private Mesh mesh;
        private ObstacleType obstacleType;

        public ObstacleData(Material material, Mesh mesh, ObstacleType obstacleType) {
            if (material == null)
                throw new NullReferenceException();
            if (mesh == null)
                throw new NullReferenceException();
            this.material = material;
            this.mesh = mesh;
            this.obstacleType = obstacleType;
        }

        public Material Material => material;
        public Mesh Mesh => mesh;

        public ObstacleType ObstacleType => obstacleType;

        public override string ToString() {
            return (" Materialname: " + material.name + " Meshname: " + mesh.name + " ObstacleType: "
                    + obstacleType);
        }


    }
}