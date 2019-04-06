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

        /// <summary>
        /// Constructor to create a new obstacle data. used for stackable obstacles
        /// </summary>
        /// <param name="obstacleType"></param>
        /// <param name="upperMesh"></param>
        /// <param name="upperMeshMaterial"></param>
        /// <param name="lowerMesh"></param>
        /// <param name="lowerMeshMaterial"></param>
        /// <param name="doorMesh"></param>
        /// <param name="doorMeshMaterial"></param>
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

        /// <summary>
        /// Constructor to create a new obstacle data.  used for non stackable objects
        /// </summary>
        /// <param name="obstacleType"></param>
        /// <param name="upperMesh"></param>
        /// <param name="upperMeshMaterial"></param>
        public ObstacleData(ObstacleType obstacleType, Mesh upperMesh, Material upperMeshMaterial) {
            this.obstacleType = obstacleType;
            this.upperMesh = upperMesh;
            this.upperMeshMaterial = upperMeshMaterial;
        }

        public ObstacleType ObstacleType => obstacleType;

        /// <summary>
        /// Returns a string with object meta
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return ("Obstacle Type: " + obstacleType + "with Upper Mesh " + upperMesh.name + " with Material "
                    + upperMeshMaterial.name + " ObstacleType: " + obstacleType);
        }

        /// <summary>
        /// Builds a obstacle data list out of obstacle part
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
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