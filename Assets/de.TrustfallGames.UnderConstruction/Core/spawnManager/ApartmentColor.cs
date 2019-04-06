using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ApartmentColor : MonoBehaviour {
        [SerializeField] public Color green;
        [SerializeField] public Color blue;
        [SerializeField] public Color red;
        [SerializeField] public Color yellow;

        /// <summary>
        /// Returns the color of the apartment
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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
                case ApartmentColorType.None:
                    return new Color(0,0,0,0);
            }

            return new Color();
        }
    }

    public enum ApartmentColorType {
        Green = 0, Blue = 1, Red = 2, Yellow = 3,
        None = 4
    }
}