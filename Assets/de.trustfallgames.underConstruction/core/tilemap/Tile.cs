using UnityEngine;
using UnityEngine.Serialization;

namespace de.trustfallgames.underConstruction.core.tilemap {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MeshCollider))]
    public class Tile : MonoBehaviour {
        
        private GameObject house;
        [SerializeField] private bool blocked;

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

        public bool Blocked => blocked;

        public void SpawnObject() {
            //TODO:
        }
        
        
    }
}
