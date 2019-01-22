using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.trustfallgames.targetsenior.tilemap {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshCollider))]
    public class Tile : MonoBehaviour {
        [SerializeField] private bool blocked;


        private Tile() {
            
        }
        
        public Tile(int x, int z) {
            _coords = new TileCoord(x, z);
        }
        
        private BoxCollider collider;
        private TileCoord _coords;

        public void SetTilecords(int x, int z) {
            _coords = new TileCoord(x,z);
        }
        
        public TileCoord Coords => _coords;

        public bool Blocked => blocked;

        // Start is called before the first frame update
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
        }

        private void OnValidate() {
            GetComponent<BoxCollider>().enabled = blocked;
            GetComponent<MeshCollider>().enabled = !blocked;
        }

        public void CheckCollider() {
            collider = GetComponent<BoxCollider>();
            collider.size = new Vector3(10,5,10);
        }
        
        
    }
}
