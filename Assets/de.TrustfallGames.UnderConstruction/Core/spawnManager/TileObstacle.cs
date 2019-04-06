using de.TrustfallGames.UnderConstruction.Core.tilemap;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    /// <summary>
    /// Contains data about one obstacle
    /// </summary>
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

        /// <summary>
        /// Adds a stage to the obstacle.
        /// </summary>
        public void AddStage() {
            stage++; 
        }

        /// <summary>
        /// removes a stage from the obstacle
        /// </summary>
        public void TakeStage() { stage--; }

        public int Stage => stage;

        /// <summary>
        /// Extract meta data from the object
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return ("ObstacleType: " + obstacleType + "Upper Mesh " + upperMesh.name + " Upper Material "
                    + upperMeshMaterial.name);
        }

        /// <summary>
        /// Returns a obstacle data set. Returns null id not valid
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the right obstacle part depending on the stage
        /// </summary>
        /// <returns></returns>
        public ObstacleObjectData GetObstacleObjectDataStaged() {
            if (stage == 1) {
                return new ObstacleObjectData(upperMesh, upperMeshMaterial);
            }

            return new ObstacleObjectData(lowerMesh, lowerMeshMaterial);
        }

        /// <summary>
        /// Struct do define a obstacle data part
        /// </summary>
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

        /// <summary>
        /// Enum to define obstacle data part
        /// </summary>
        public enum ObstacleObjectDataType { upper, lower, door }
    }
}