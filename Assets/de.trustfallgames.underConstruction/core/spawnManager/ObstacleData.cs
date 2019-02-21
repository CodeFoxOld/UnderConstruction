using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.tilemap;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ObstacleData {
        private ObstacleType obstacleType;
        private Mesh upperMesh;
        private Material upperMeshMaterial;
        private Mesh lowerMesh;
        private Material lowerMeshMaterial;
        private Mesh doorMesh;
        private Material doorMeshMaterial;

        private ObstacleData() { }

        private ObstacleData(ObstacleType obstacleType, Mesh upperMesh, Material upperMeshMaterial, Mesh lowerMesh,
            Material lowerMeshMaterial, Mesh doorMesh, Material doorMeshMaterial) {
            this.obstacleType = obstacleType;
            this.upperMesh = upperMesh;
            this.upperMeshMaterial = upperMeshMaterial;
            this.lowerMesh = lowerMesh;
            this.lowerMeshMaterial = lowerMeshMaterial;
            this.doorMesh = doorMesh;
            this.doorMeshMaterial = doorMeshMaterial;
        }

        public ObstacleData(ObstacleType obstacleType, Mesh upperMesh, Material upperMeshMaterial) {
            this.obstacleType = obstacleType;
            this.upperMesh = upperMesh;
            this.upperMeshMaterial = upperMeshMaterial;
        }

        public ObstacleType ObstacleType => obstacleType;

        public override string ToString() {
            return ("Obstacle Type: " + obstacleType + "with Upper Mesh " + upperMesh.name + " with Material "
                    + upperMeshMaterial.name + " ObstacleType: " + obstacleType);
        }

        public static List<ObstacleData> Builder(ObstaclePart parts) {
            List<ObstacleData> list = new List<ObstacleData>();

            for (int i = 0; i < parts.UpperMeshMaterials.Length; i++) {
                if (parts.ObstacleType == ObstacleType.House) {
                    list.Add(
                             new ObstacleData(
                                              parts.ObstacleType, parts.UpperMesh, parts.UpperMeshMaterials[i],
                                              parts.LowerMesh, parts.LowerMeshMaterials[i], parts.DoorMesh,
                                              parts.DoorMeshMaterial[i]));
                } else {
                    list.Add(new ObstacleData(parts.ObstacleType, parts.UpperMesh, parts.UpperMeshMaterials[i]));
                }
            }

            return list;
        }

        public Mesh UpperMesh => upperMesh;
        public Material UpperMeshMaterial => upperMeshMaterial;
        public Mesh LowerMesh => lowerMesh;
        public Material LowerMeshMaterial => lowerMeshMaterial;
        public Mesh DoorMesh => doorMesh;
        public Material DoorMeshMaterial => doorMeshMaterial;
    }
}