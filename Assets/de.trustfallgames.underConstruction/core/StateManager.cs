using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using UnityEngine;

public class StateManager : MonoBehaviour {
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject mapManager;
    [SerializeField] private GameObject character;    
    [SerializeField] private GameObject spawnManager;

    // Start is called before the first frame update
    void Start(){
        //Set the framerate to 30fps for mobile performance
        Application.targetFrameRate = 30;
        
        Instantiate(gameManager);
        Instantiate(mapManager).GetComponent<MapManager>().GenerateTilemap();
        Instantiate(character);
        if (spawnManager != null)
            Instantiate(spawnManager);
        
        Destroy(gameObject);
    }
}