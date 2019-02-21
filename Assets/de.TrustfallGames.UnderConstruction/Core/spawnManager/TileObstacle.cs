using de.TrustfallGames.UnderConstruction.Core.tilemap;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class TileObstacle {
        private int stage = 1;
        private ObstacleType obstacleType;
        private Mesh upperMesh;
        private Material upperMeshMaterial;
        private Mesh lowerMesh;
        private Material lowerMeshMaterial;
        private Mesh doorMesh;
        private Material doorMeshMaterial;

        public TileObstacle(ObstacleData data) {
            obstacleType = data.ObstacleType;
            upperMesh = data.UpperMesh;
            upperMeshMaterial = data.UpperMeshMaterial;
            if (obstacleType == ObstacleType.House) {
                lowerMesh = data.LowerMesh;
                lowerMeshMaterial = data.LowerMeshMaterial;
                doorMesh = data.DoorMesh;
                doorMeshMaterial = data.DoorMeshMaterial;
            }
        }

        public void AddStage() {
            stage++; 
        }

        public void TakeStage() { stage--; }

        public int Stage => stage;

        public override string ToString() {
            return ("ObstacleType: " + obstacleType + "Upper Mesh " + upperMesh.name + " Upper Material "
                    + upperMeshMaterial.name);
        }

        public ObstacleObjectData? GetObstacleObjectData(ObstacleObjectDataType type) {
            switch (type) {
                case ObstacleObjectDataType.upper:
                    return new ObstacleObjectData(upperMesh, upperMeshMaterial);
                case ObstacleObjectDataType.lower:
                    return new ObstacleObjectData(lowerMesh, lowerMeshMaterial);
                case ObstacleObjectDataType.door:
                    return new ObstacleObjectData(doorMesh, doorMeshMaterial);
            }

            return null;
        }

        public ObstacleType ObstacleType => obstacleType;

        public ObstacleObjectData GetObstacleObjectDataStaged() {
            if (stage == 1) {
                return new ObstacleObjectData(upperMesh, upperMeshMaterial);
            }

            return new ObstacleObjectData(lowerMesh, lowerMeshMaterial);
        }

        public struct ObstacleObjectData {
            private Mesh mesh;
            private Material material;

            public Mesh Mesh => mesh;
            public Material Material => material;

            public ObstacleObjectData(Mesh mesh, Material material) {
                this.mesh = mesh;
                this.material = material;
            }
        }

        public enum ObstacleObjectDataType { upper, lower, door }
    }
}