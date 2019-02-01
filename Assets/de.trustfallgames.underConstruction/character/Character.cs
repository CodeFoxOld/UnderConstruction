using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.character {
    [RequireComponent(typeof(Movement))]
    public class Character : MonoBehaviour {
        [Header("Whole Player Object")]
        [SerializeField]
        private Transform _player;

        [Header("Character Model")]
        [SerializeField]
        private Transform _character;

        [Header("Blueprint for new Apartmentparts")]
        [SerializeField]
        private GameObject apartmentBlueprint;

        private TileCoord _currentCoord = new TileCoord(0, 0);
        private MoveDirection _moveDirection = MoveDirection.left;
        private Controller _controller;
        [SerializeField] private Movement _movement;
        private bool moving;

        public bool Moving => moving;

        private int colorCount = 1;
        private ApartmentColor latestColor;

        //UI Stuff
        private int highscore;
        private int highscoreRest;
        private int destRest;
        private int destructibleCount;

        private Character() { }

        private void Start() {
            GameManager.GetManager().RegisterCharacter(this);
            _controller = GameManager.GetManager().Controller;
            _movement = GetComponent<Movement>();
        }

        private void Update() {
            if (moving) {
                Move();
            }
        }

        private void Move() {
            _character.transform.Translate(0, 1 / (GameManager.GetManager().Settings.MoveUpSpeed * 60), 0);
            if (Math.Abs(_character.transform.position.y) < 0.01) {
                moving = false;
                _character.transform.position = new Vector3(CurrentCoord.X, 0, CurrentCoord.Z);
            }
        }

        public void Stack(ApartmentPart apartmentPart) {
            var b = _character.gameObject.transform;
            _character = Instantiate(apartmentBlueprint).transform;                    //Create Blueprint
            _character.position = new Vector3(CurrentCoord.X, -1, CurrentCoord.Z);     //Assign under tile
            _character.GetComponent<MeshFilter>().mesh = apartmentPart.Mesh;           //Assign mesh
            _character.GetComponent<MeshRenderer>().material = apartmentPart.Material; //Assign material
            _character.localRotation = b.transform.localRotation;
            _character.SetParent(_player);
            b.SetParent(_character); //set old parent as Child
            moving = true;           //Start moving
            _movement.CharTransform = _character.transform;
            CalculateHighscore(apartmentPart);
        }

        private void CalculateHighscore(ApartmentPart apartmentPart) {
            if (latestColor == apartmentPart.ApartmentColor) {
                colorCount++;
            } else {
                colorCount = 1;
                latestColor = apartmentPart.ApartmentColor;
            }

            highscore += (GameManager.GetManager().Settings.BasePoint * colorCount);

            int dest = (GameManager.GetManager().Settings.BasePoint * colorCount)
                       / GameManager.GetManager().Settings.DestructablesPerPoints;
            destRest = (GameManager.GetManager().Settings.BasePoint * colorCount)
                       % GameManager.GetManager().Settings.DestructablesPerPoints;
            destructibleCount += dest;

            GameManager.GetManager()
                       .UiManager.OnHighscoreCalc(colorCount, highscore)
                       .OnDeconstructorChange(destructibleCount);
        }

        public float GetCurrentRotation() { return Movement.GetRotationValue(_moveDirection); }

        public Movement Movement {
            get {
                if (_movement == null)
                    _movement = GetComponent<Movement>();
                return _movement;
            }
        }

        public TileCoord CurrentCoord { get { return _currentCoord; } set { _currentCoord = value; } }
        public Transform CharacterTransform { get { return _character; } }
        public MoveDirection CurrentMoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public MoveDirection MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public Transform Player { get { return _player; } }
        public Controller Controller { get { return _controller; } }
    }
}