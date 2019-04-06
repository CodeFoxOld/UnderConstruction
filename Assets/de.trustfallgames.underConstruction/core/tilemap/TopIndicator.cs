using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Util;
using UnityEngine;
using Tile = de.TrustfallGames.UnderConstruction.Core.tilemap.Tile;

namespace de.TrustfallGames.UnderConstruction.Core.tilemap {
    /// <summary>
    /// Class to display the top indicator attached to a tile. Enables multiple objects
    /// </summary>
    public class TopIndicator : MonoBehaviour, IInternUpdate {
        [SerializeField] private GameObject[] OtherObjectsToDisable;
        private bool globalState;
        private bool localState = true;
        private Counter counter;
        private Tile tile;

        void Start() { RegisterInternUpdate(); }

        public void InternUpdate() {
            if (tile == null || tile.ObstacleData == null) return;
            if (tile.ObstacleData.Stage == GameManager.GetManager().Settings.BuildingHeight) {
                tile.WarnIndicator(true);
                globalState = true;
                if (counter.Check()) {
                    ToggleLocalState();
                }
            } else if (localState) {
                ToggleLocalState();
            } else {
                if (globalState) {
                    globalState = false;
                    tile.WarnIndicator(false);
                }
            }
        }

        public void RegisterInternUpdate() { GameManager.GetManager().InternTick.RegisterTickObject(this, 80); }

        public void Init() { }

        /// <summary>
        /// toggles the local state
        /// </summary>
        private void ToggleLocalState() {
            localState = !localState;
            try {
                if (gameObject == null || GetComponent<MeshRenderer>() == null) return;
            } catch (MissingReferenceException e) {
                Console.WriteLine(e);
                return;
            }

            if (localState) {
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
            } else {
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
            }
        }

        public void Init(Tile tile) {
            counter = new Counter(tile.TopInidicatorInterval);
            this.tile = tile;
        }

        public void OnDestroy() { GameManager.GetManager().InternTick.UnregisterTickObject(this); }
    }
}