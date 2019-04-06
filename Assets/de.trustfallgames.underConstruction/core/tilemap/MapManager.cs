using System;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.Destructible;
using de.TrustfallGames.UnderConstruction.UI.Core;
using de.TrustfallGames.UnderConstruction.UI.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.Core.tilemap {
    /// <summary>
    /// Class to generate an manage the map
    /// </summary>
    [RequireComponent(typeof(ApartmentColor))]
    public class MapManager : MonoBehaviour {
        [Header("Use \"Generate Classes for Tiles\" to add scripts to all tiles and fill refresh values")]
        [SerializeField]
        private string lastRefresh;

        [SerializeField] private GameObject tilePrefab;

        [FormerlySerializedAs("xDimension")]
        [Header("User \"Generate Field with Dimensions\" to generate a tilemap with the following dimensions")]
        [SerializeField]
        private int zDimension;

        [SerializeField] private int yDimension;
        [SerializeField] private Dictionary<TileCoord, Tile> tiles;
        [SerializeField] private GameObject DestructiblePrefab;
        
        private GameManager gameManager;
        private Character.Character character;

        private ApartmentColor apartmentColor;

        // Start is called before the first frame update
        void Start() {
            GameManager.GetManager().RegisterMapManager(this);
            GenerateTileClasses();
            RefreshDictionary();
            gameManager = GameManager.GetManager();
            apartmentColor = GetComponent<ApartmentColor>();
        }

        public ApartmentColor ApartmentColor => apartmentColor;

        public void RegisterCharacter(Character.Character character) { this.character = character; }

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
                child.gameObject.GetComponent<Tile>().Coords = new TileCoord((int) position.x, (int) position.z);
            }

            lastRefresh = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }

        /// <summary>
        /// Generates a tilemap with a predefined size
        /// </summary>
        [ContextMenu("Generate Tilemap with Dimensions")]
        public void GenerateTilemap() {
            for (int i = 0; i < zDimension; i++) {
                for (int j = 0; j < yDimension; j++) {
                    GameObject tile = GameObject.Instantiate(tilePrefab);
                    tile.transform.SetParent(transform);
                    tile.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                    tile.transform.position = new Vector3((0 - (zDimension / 2) + i), 0, (0 - (yDimension / 2) + j));
                    tile.transform.name = "Tile: " + ((0 - (zDimension / 2)) + i) + "|" + ((0 - (yDimension / 2) + j));
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
        }

        /// <summary>
        /// Returns if a field is blocked
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool FieldBlocked(TileCoord position) {
            return !tiles.ContainsKey(position) || tiles[position].Blocked;
        }

        /// <summary>
        /// Returns if the field is blocked in a specified direction depending on a position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="moveDirection"></param>
        /// <returns></returns>
        public bool FieldBlocked(TileCoord position, MoveDirection moveDirection) {
            return FieldBlocked(position.NextTileCoord(moveDirection));
        }

        /// <summary>
        /// Converts easy coord in coords for the map
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TileCoord GetCoordForEasyCoord(int x, int y) {
            return new TileCoord(0 - (zDimension / 2) + x, (0 - (yDimension / 2) + y));
        }

        /// <summary>
        /// Returns the tile depending on the tile coord
        /// </summary>
        /// <param name="tileCoord"></param>
        /// <returns></returns>
        public Tile GetTile(TileCoord tileCoord) {
            if (tiles.ContainsKey(tileCoord)) {
                return tiles[tileCoord];
            }

            return null;
        }

        public int ZDimension => zDimension;
        public int YDimension => yDimension;

        public Dictionary<TileCoord, Tile> Tiles => tiles;

        /// <summary>
        /// Spawns a destructible
        /// </summary>
        /// <param name="direction"></param>
        public void SpawnDesctructible(DestructibleDirection direction) {
            if (character.TakeDestructible()) {
                Instantiate(DestructiblePrefab)
                    .GetComponent<DestructibleObject>()
                    .Setup(gameManager, this, character, direction, character.CurrentCoord)
                    .Init();
            }
        }
    }
}