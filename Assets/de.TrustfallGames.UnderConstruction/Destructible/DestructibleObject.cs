using System;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Tilemap;
using de.TrustfallGames.UnderConstruction.UI;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Destructible {
    public class DestructibleObject : MonoBehaviour {
        private GameManager _gameManager;
        private MapManager _mapManager;
        private Character _character;
        private DestructibleDirection _direction;
        private Vector3 _directionVector3;
        private bool start;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() {
            if (!start) return;
            gameObject.transform.Translate(_directionVector3);
            
            
        }

        public DestructibleObject SetGameManager(GameManager gameManager) {
            _gameManager = gameManager;
            return this;
        }

        public DestructibleObject SetMapManager(MapManager mapManager) {
            _mapManager = mapManager;
            return this;
        }

        public DestructibleObject SetCharacter(Character character) {
            _character = character;
            return this;
        }

        public DestructibleObject SetDirection(DestructibleDirection direction) {
            _direction = direction;
            SetDirectionVector3();
            return this;
        }

        public void Init() {
            start = true;
            //TODO: Transform to start point
        }

        private void SetDirectionVector3() {
            Vector3 dirVector = new Vector3();
            switch (_direction) {
                case DestructibleDirection.horizontal:
                    dirVector = new Vector3(1, 0, 0);
                    break;
                case DestructibleDirection.vertical:
                    dirVector = new Vector3(0, 0, 1);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            _directionVector3 = (dirVector * _gameManager.Settings.DestructibleMoveSpeed) * Time.fixedDeltaTime;
        }
    }
}