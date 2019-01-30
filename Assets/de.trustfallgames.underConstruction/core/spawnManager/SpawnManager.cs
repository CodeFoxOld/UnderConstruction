using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject[] apartmentParts;
    
    Dictionary<Color, ApartmentStack> apartmentStacks = new Dictionary<Color, ApartmentStack>();
    
    // Start is called before the first frame update
    void Start() {
        BuildDictionary();
    }

    private void BuildDictionary() {
        foreach (GameObject part in apartmentParts) {
            
        }
    }

    // Update is called once per frame
    void Update() { }
}