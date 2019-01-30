using System;
using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.core.spawnManager;
using UnityEngine;
using Random = System.Random;

public class ApartmentStack {
    private readonly List<GameObject> apartments = new List<GameObject>();

    private ApartmentColor apartmentColor;

    private ApartmentStack() { }

    /// <summary>
    /// Creates a internal list of objects with the color.
    /// </summary>
    /// <param name="apartments"></param>
    /// <param name="apartmentColor"></param>
    public ApartmentStack(GameObject[] apartments, ApartmentColor apartmentColor) {
        foreach (GameObject go in apartments) {
            if (go.GetComponent<apartmentPart>().ApartmentColor == apartmentColor)
                this.apartments.Add(go);
        }

        this.apartmentColor = apartmentColor;
    }

    /// <summary>
    /// Draws a random object from the list
    /// </summary>
    /// <returns></returns>
    public GameObject draw() {
        Random rand = new Random();
        return apartments[rand.Next(apartments.Count)];
    }
}