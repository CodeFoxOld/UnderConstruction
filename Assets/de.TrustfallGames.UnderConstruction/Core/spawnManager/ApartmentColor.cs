using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ApartmentColor : MonoBehaviour {
        [SerializeField] public Color green;
        [SerializeField] public Color blue;
        [SerializeField] public Color red;
        [SerializeField] public Color yellow;

        public Color GetColor(ApartmentColorType color) {
            switch (color) {
                case ApartmentColorType.Green:
                    return green;
                case ApartmentColorType.Blue:
                    return blue;
                case ApartmentColorType.Red:
                    return red;
                case ApartmentColorType.Yellow:
                    return yellow;
            }

            return new Color();
        }
    }

    public enum ApartmentColorType { Green, Blue, Red, Yellow }
}