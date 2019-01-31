using UnityEngine;

namespace de.trustfallGames.underConstruction.core.spawnManager {
    public class ApartmentPart : MonoBehaviour {
        [SerializeField] private ApartmentColor _apartmentColor;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        public ApartmentColor ApartmentColor => _apartmentColor;
        public Mesh Mesh => mesh;
        public Material Material => material;
    }

    public enum ApartmentColor { Green, Blue, Red }
}