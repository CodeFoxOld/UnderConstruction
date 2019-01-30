using System.Collections.Generic;
using de.trustfallgames.underConstruction.core.spawnManager;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject[] apartmentParts;

    private readonly Dictionary<ApartmentColor, ApartmentStack> apartmentStacks = new Dictionary<ApartmentColor, ApartmentStack>();
    
    // Start is called before the first frame update
    private void Start() {
        BuildDictionary();
    }

    private void BuildDictionary() {
        foreach (GameObject part in apartmentParts) {
            if (!apartmentStacks.ContainsKey(part.GetComponent<apartmentPart>().ApartmentColor)) /*create new dictionary entry*/ {
                ApartmentStack stack = new ApartmentStack(apartmentParts, part.GetComponent<apartmentPart>().ApartmentColor);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}