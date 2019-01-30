using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.character;
using de.trustfallgames.underConstruction.core.tilemap;
using UnityEngine;

public class StateManager : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private MapManager  _mapManager;
    [SerializeField] private Character   _character;

    // Start is called before the first frame update
    void Start() {
        Instantiate(_gameManager);
        Instantiate(_character);
        Instantiate(_mapManager).GenerateTilemap();
    }

    // Update is called once per frame
    void Update() { }
}