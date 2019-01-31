using System.Collections.Generic;
using de.trustfallGames.underConstruction.core.spawnManager;
using UnityEngine;
using Random = System.Random;

public class ApartmentStack {
    private readonly List<ApartmentPart> apartments = new List<ApartmentPart>();

    private ApartmentColor apartmentColor;

    private ApartmentStack() { }

    /// <summary>
    /// Creates a internal list of objects with the color.
    /// </summary>
    /// <param name="apartments"></param>
    /// <param name="apartmentColor"></param>
    public ApartmentStack(GameObject[] apartments, ApartmentColor apartmentColor) {
        this.apartmentColor = apartmentColor;
        Debug.Log("Trying to create new apartment stack for color: " + apartmentColor);
        foreach (GameObject go in apartments) {
            ApartmentPart part = go.GetComponent<ApartmentPart>();
            if (part.ApartmentColor == this.apartmentColor)
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