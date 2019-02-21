using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace de.TrustfallGames.UnderConstruction.Core.spawnManager {
    public class ApartmentStack {
        private readonly List<ApartmentPart> apartments = new List<ApartmentPart>();

        private ApartmentColorType apartmentColorType;

        private ApartmentStack() { }

        /// <summary>
        /// Creates a internal list of objects with the color.
        /// </summary>
        /// <param name="apartments"></param>
        /// <param name="apartmentColorType"></param>
        public ApartmentStack(GameObject[] apartments, ApartmentColorType apartmentColorType) {
            this.apartmentColorType = apartmentColorType;
            foreach (GameObject go in apartments) {
                ApartmentPart part = go.GetComponent<ApartmentPart>();
                if (part.ApartmentColorType == this.apartmentColorType)
                    this.apartments.Add(part);
                Debug.Log("Created Apartmencolor: " + part.ApartmentColorType);
            }
        }

        /// <summary>
        /// Draws a random object from the list
        /// </summary>
        /// <returns></returns>
        public ApartmentPart draw() {
            Random rand = new Random();
            return apartments[rand.Next(apartments.Count)];
        }
    }
}