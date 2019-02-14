﻿using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using Tile = de.TrustfallGames.UnderConstruction.Core.Tilemap.Tile;

namespace de.TrustfallGames.UnderConstruction.Core.tilemap {
    public class TopIndicator : MonoBehaviour, IInternUpdate {
        [SerializeField] private GameObject[] OtherObjectsToDisable;
        private bool globalState;
        private bool localState = true;
        private Counter counter;
        private Tile tile;

        void Start() {
            RegisterInternUpdate(); 
        }

        public void InternUpdate() {
            if(tile == null || tile.ObstacleData == null) return;
            if (tile.ObstacleData.Stage == GameManager.GetManager().Settings.BuildingHeight) {
                if (counter.Check()) {
                    ToggleLocalState();
                }
            } else if (localState) {
                ToggleLocalState();
            }
        }

        public void RegisterInternUpdate() { GameManager.GetManager().InternTick.RegisterTickObject(this, 80); }

        public void Init() { }

        private void ToggleLocalState() {
            localState = !localState;
            if(GetComponent<MeshRenderer>() == null) return;
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
    }
}