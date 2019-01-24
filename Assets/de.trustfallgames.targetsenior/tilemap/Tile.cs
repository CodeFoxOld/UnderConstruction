using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.trustfallgames.targetsenior.tilemap {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshCollider))]
    public class Tile : MonoBehaviour {
        private Tile() {
            
        }
        
        public Tile(int x, int z) {
            Coords = new TileCoord(x, z);
        }
        
        private BoxCollider collider;

        public void SetTilecords(int x, int z) {
            Coords = new TileCoord(x,z);
        }
        
        public TileCoord Coords { get; private set; }

        [SerializeField] public bool Blocked;

        // Start is called before the first frame update
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
        }

        private void OnValidate() {
            GetComponent<BoxCollider>().enabled = Blocked;
            GetComponent<MeshCollider>().enabled = !Blocked;
        }

        public void CheckCollider() {
            collider = GetComponent<BoxCollider>();
            collider.size = new Vector3(10,5,10);
        }
        
        
    }
}
