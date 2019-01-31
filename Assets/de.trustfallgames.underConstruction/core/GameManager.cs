using System;
using de.trustfallGames.underConstruction.character;
using de.trustfallGames.underConstruction.core.tilemap;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
    [SerializeField] private Character character = null;
    [SerializeField] private Controller controller;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Settings settings;

    private GameManager() { }

    public static GameManager GetManager() {
        return _instance;
    }

    private void Awake() {
        if (_instance == null)
            _instance = this;
        else if (_instance != this) {
            Destroy(gameObject);
        }


        controller = GetComponent<Controller>();
        settings = GetComponent<Settings>();
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void RegisterCharacter(Character character) {
        if (this.character == null) {
            controller.Character = this.character = character;
        } else {
            throw new Exception("Character already set!");
        }
    }

    public void RegisterMapManager(MapManager mapManager) {
        if (this.mapManager == null) {
            this.mapManager = mapManager;
        } else {
            throw new Exception("MapManager already set!");
        }
    }

    public Character Character => character;
    public Controller Controller => controller;
    public MapManager MapManager => mapManager;
    public Settings Settings => settings;

}