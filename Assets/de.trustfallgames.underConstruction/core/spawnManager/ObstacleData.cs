using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace de.trustfallgames.underConstruction.core.tilemap {
    public class ObstacleData {
        private int id;
        private Material material;
        private Mesh mesh;
        private ObstacleType obstacleType;
        
        public ObstacleData(int id, Material material, Mesh mes, ObstacleType obstacleType) {
            this.id = id;
            this.material = material;
            this.mesh = mesh;
            this.obstacleType = obstacleType;
        }

        public int Id => id;
        public Material Material => material;
        public Mesh Mesh => mesh;

        public ObstacleType ObstacleType => obstacleType;
    }
}