using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.Core.SpawnManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.character {
    [RequireComponent(typeof(Movement))]
    public class Character : MonoBehaviour, IInternUpdate {
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
        private int height;

        public bool Moving => moving;

        private int colorCount = 1;
        private ApartmentColorType latestColorType = ApartmentColorType.None;

        //UI Stuff
        private int highscore;
        private int highscoreRest;
        private int destRest;
        private GameManager gameManager;
        [SerializeField] public int destructibleCount;
        public int Highscore => highscore;

        public int DestructibleCount => destructibleCount;

        private Character() { }

        private void Start() {
            gameManager = GameManager.GetManager().RegisterCharacter(this);
            _controller = gameManager.Controller;
            _movement = GetComponent<Movement>();
            RegisterInternUpdate();
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
            b.position = new Vector3(_character.position.x, 0, _character.position.z);
            moving = true;           //Start moving
            _movement.CharTransform = _character.transform;
            CalculateHighscore(apartmentPart);
            height++;
        }

        private void CalculateHighscore(ApartmentPart apartmentPart) {
            if (latestColorType == apartmentPart.ApartmentColorType) {
                colorCount++;
            } else {
                colorCount = 1;
                latestColorType = apartmentPart.ApartmentColorType;
            }

            int toAdd = GameManager.GetManager().Settings.BasePoint * colorCount;

            highscore += toAdd;

            int dest = (toAdd + destRest) / GameManager.GetManager().Settings.DestructablesPerPoints;
            destRest = (toAdd + destRest) % GameManager.GetManager().Settings.DestructablesPerPoints;
            destructibleCount += dest > gameManager.Settings.MaxDestructablesPerCalc ?
                                     gameManager.Settings.MaxDestructablesPerCalc : dest;

            gameManager.UiManager.OnHighscoreCalc(colorCount, apartmentPart.ApartmentColorType, highscore, height
            ).OnDeconstructorChange(DestructibleCount);
        }

        /// <summary>
        /// Returns true if a destructable is available.
        /// </summary>
        /// <returns></returns>
        public bool TakeDestructible() {
            if (destructibleCount == 0) return false;
            destructibleCount--;
            gameManager.UiManager.OnDeconstructorChange(DestructibleCount);
            return true;
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

        public ApartmentColorType LatestColorType => latestColorType;

        public void InternUpdate() {
            if (moving) {
                Move();
            }
        }

        public void RegisterInternUpdate() { gameManager.InternTick.RegisterTickObject(this, 20); }

        public void Init() { }

        public int Height => height;
    }
}