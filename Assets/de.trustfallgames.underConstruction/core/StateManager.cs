using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.character;
using de.trustfallgames.underConstruction.core.tilemap;
using UnityEngine;
using UnityEngine.Serialization;

public class StateManager : MonoBehaviour {
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject mapManager;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject spawnManager;

    // Start is called before the first frame update
    void Start() {
        Instantiate(gameManager);
        Instantiate(mapManager).GetComponent<MapManager>().GenerateTilemap();
        Instantiate(character);
        Instantiate(ui);
        if (spawnManager != null)
            Instantiate(spawnManager);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() { }
}