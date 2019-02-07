using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using UnityEngine;
using Random = System.Random;

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
        Debug.Log("Trying to create new apartment stack for color: " + apartmentColorType);
        foreach (GameObject go in apartments) {
            ApartmentPart part = go.GetComponent<ApartmentPart>();
            if (part.ApartmentColorType == this.apartmentColorType)
                this.apartments.Add(part);
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