using UnityEngine;

namespace de.trustfallgames.underConstruction.core.spawnManager {
    public class apartmentPart : MonoBehaviour {
        private ApartmentColor _apartmentColor;
        public ApartmentColor ApartmentColor => _apartmentColor;
    }

    public enum ApartmentColor {
        Green,
        Blue,
        Red
    }
    
    
}