using System;
using System.Collections;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.character;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
   [SerializeField]private Character character = null;
   [SerializeField] private Controller controller;

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

    public Character Character => character;
    public Controller Controller => controller;
}