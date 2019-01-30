using System;
using System.Collections.Generic;
using UnityEngine;

namespace de.trustfallgames.underConstruction.core.tilemap {
    public class MapManager : MonoBehaviour {
        [Header("Use \"Generate Classes for Tiles\" to add scripts to all tiles and fill refresh values")]
        [SerializeField]
        private string lastRefresh;

        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Dictionary<TileCoord, Tile> _tiles;

        // Start is called before the first frame update
        void Start() {
            GenerateTileClasses();
            RefreshDictionary();
        }

        void Update() { }

        /// <summary>
        /// Adds Tile Classes to Tiles. Adjust Position.
        /// </summary>
        [ContextMenu("Generate Classes for Tiles")]
        public void GenerateTileClasses() {
            foreach (Transform child in transform) {
                if (child.GetComponent<Tile>() == null) {
                    child.gameObject.AddComponent<Tile>().CheckCollider();
                }

                child.transform.transform.position = new Vector3(
                                                                 (float) Math.Round(
                                                                                    child.transform.position.x,
                                                                                    MidpointRounding.ToEven), 0,
                                                                 (float) Math.Round(
                                                                                    child.transform.position.z,
                                                                                    MidpointRounding.ToEven));
                child.gameObject.GetComponent<Tile>()
                     .SetTilecords((int) child.transform.position.x, (int) child.transform.position.z);
            }

            lastRefresh = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }

        [Header("User \"Generate Field with Dimensions\" to generate a tilemap with the following dimensions")]
        [SerializeField]
        private int xDimension;

        [SerializeField] private int yDimension;

        /// <summary>
        /// Generates a tilemap with a predefined size
        /// </summary>
        [ContextMenu("Generate Tilemap with Dimensions")]
        public void GenerateTilemap() {
            for (int i = 0; i < xDimension; i++) {
                for (int j = 0; j < yDimension; j++) {
                    GameObject tile = GameObject.Instantiate(_tilePrefab);
                    tile.transform.SetParent(transform);
                    tile.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                    tile.transform.position   = new Vector3((0 - (xDimension / 2) + i), 0, (0 - (yDimension / 2) + j));
                    tile.transform.name =
                        "Tile: " + ((0 - (xDimension / 2)) + i) + "|" + ((0 - (yDimension / 2) + j));
                }
            }

            GenerateTileClasses();
        }

        /// <summary>
        /// Deletes the tilemap and everything on it
        /// </summary>
        [ContextMenu("Delete Tilemap")]
        public void DeleteTilemap() {
            List<GameObject> objects = new List<GameObject>();
            foreach (Transform obj in transform) {
                objects.Add(obj.gameObject);
            }

            while (objects.Count != 0) {
                DestroyImmediate(objects[0]);
                objects.RemoveAt(0);
            }
        }

        /// <summary>
        /// Refreshes the Dictionary to get the best, newest and most shiny keys.
        /// </summary>
        public void RefreshDictionary() {
            _tiles = new Dictionary<TileCoord, Tile>();
            foreach (Transform obj in transform) {
                Tile tile = obj.GetComponent<Tile>();
                _tiles[tile.Coords] = tile;
            }

            Debug.Log(_tiles.Count + " tiles refreshed");
        }
    }
}