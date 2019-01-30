﻿using System;
using System.Collections.Generic;
using de.trustfallgames.underConstruction.util;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace de.trustfallgames.underConstruction.core.tilemap {
    public class MapManager : MonoBehaviour {
        [Header("Use \"Generate Classes for Tiles\" to add scripts to all tiles and fill refresh values")]
        [SerializeField]
        private string lastRefresh;

        [SerializeField] private GameObject tilePrefab;

        [Header("User \"Generate Field with Dimensions\" to generate a tilemap with the following dimensions")]
        [SerializeField] private int xDimension;
        [SerializeField] private int yDimension;
        [SerializeField] private Dictionary<TileCoord, Tile> tiles;

        // Start is called before the first frame update
        void Start() {
            GameManager.GetManager().RegisterMapManager(this);
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

                Vector3 position = child.transform.position;
                child.transform.transform.position = new Vector3(
                                                                 (float) Math.Round(
                                                                                    position.x,
                                                                                    MidpointRounding.ToEven), 0,
                                                                 (float) Math.Round(
                                                                                    position.z,
                                                                                    MidpointRounding.ToEven));
                child.gameObject.GetComponent<Tile>()
                     .SetTilecords((int) position.x, (int) position.z);
            }

            lastRefresh = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }

        /// <summary>
        /// Generates a tilemap with a predefined size
        /// </summary>
        [ContextMenu("Generate Tilemap with Dimensions")]
        public void GenerateTilemap() {
            for (int i = 0; i < xDimension; i++) {
                for (int j = 0; j < yDimension; j++) {
                    GameObject tile = GameObject.Instantiate(tilePrefab);
                    tile.transform.SetParent(transform);
                    tile.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                    tile.transform.position = new Vector3((0 - (xDimension / 2) + i), 0, (0 - (yDimension / 2) + j));
                    tile.transform.name = "Tile: " + ((0 - (xDimension / 2)) + i) + "|" + ((0 - (yDimension / 2) + j));
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
            tiles = new Dictionary<TileCoord, Tile>(new TileCoordComparer());
            foreach (Transform obj in transform) {
                Tile tile = obj.GetComponent<Tile>();
                tiles[tile.Coords] = tile;
            }

            Debug.Log(tiles.Count + " tiles refreshed");
        }

        public bool FieldBlocked(TileCoord tileCoord) {
            return !tiles.ContainsKey(tileCoord) || tiles[tileCoord].Blocked;
        }

        public bool FieldBlocked(TileCoord tileCoord, MoveDirection moveDirection) {
            return FieldBlocked(tileCoord.NextTileCoord(moveDirection));
        }
    }
}