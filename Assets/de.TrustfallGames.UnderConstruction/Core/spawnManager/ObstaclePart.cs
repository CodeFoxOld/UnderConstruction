using de.TrustfallGames.UnderConstruction.Core.tilemap;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ObstaclePart : MonoBehaviour {
        [Header("Obstacle Type of the prefab. If the tpye is \"NotHouse\", only the upper part must set. Multiple Materials are possible!")]
        [SerializeField]
        private ObstacleType obstacleType;

        [Header("Mesh of the upper part")]
        [SerializeField]
        private Mesh upperMesh;

        [Header("Assign materials for the upper part. Each materials generates a unique obstacle")]
        [SerializeField]
        private Material[] upperMeshMaterials;

        [Header("Mesh of the lower Part")]
        [SerializeField]
        private Mesh lowerMesh;

        [Header(
            "Assign materials for the lower part. IMPORTANT: Materials must be assigned in the same order like the upper parts. ")]
        [SerializeField]
        private Material[] lowerMeshMaterials;
    
        [Header("Mesh of the door Object")]
        [SerializeField]
        private Mesh doorMesh;

        [FormerlySerializedAs("doorMat")]
        [Header("Material of the door object. IMPORTANT: Materials must be assigned in the same order like the upper parts. ")]
        [SerializeField]
        private Material[] doorMeshMaterial;

        public ObstacleType ObstacleType => obstacleType;
        public Mesh UpperMesh => upperMesh;
        public Material[] UpperMeshMaterials => upperMeshMaterials;
        public Mesh LowerMesh => lowerMesh;
        public Material[] LowerMeshMaterials => lowerMeshMaterials;
        public Mesh DoorMesh => doorMesh;
        public Material[] DoorMeshMaterial => doorMeshMaterial;
    }
}