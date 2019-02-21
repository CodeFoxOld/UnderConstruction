using UnityEngine;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ApartmentPart : MonoBehaviour {
        [FormerlySerializedAs("apartmentColorEnum")] [FormerlySerializedAs("_apartmentColor")] [SerializeField] private ApartmentColorType apartmentColorType;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        public ApartmentColorType ApartmentColorType => apartmentColorType;
        public Mesh Mesh => mesh;
        public Material Material => material;
    }

}