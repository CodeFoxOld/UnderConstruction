using de.trustfallGames.underConstruction.core.tilemap;
using UnityEngine;

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