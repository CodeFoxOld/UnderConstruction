﻿using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class Settings : MonoBehaviour {
        [Header("The speed Houses comes out of the ground")]
        [SerializeField]
        private float moveUpSpeed = 1;

        [Header("The time, after starting the spawn event, until the field spawns.")]
        [SerializeField]
        private float spawnDuration = 10;

        [Header("How often should a new spawn event fire")]
        [SerializeField]
        private float spawnInterval = 10;

        [Header("Player rotation Speed")]
        [Range(0.01f, 1)]
        [SerializeField]
        private float rotationDuration = 0.1f;

        [Header("Player Movement Speed")]
        [Range(0.01f, 1)]
        [SerializeField]
        private float moveDuration = 0.1f;

        [Header("The Base points for one stacked house")]
        [SerializeField]
        private int basePoint = 100;

        [Header("At how many Points should the player get one destructible")]
        [SerializeField]
        private int destructablesPerPoints;

        [Header("The Speed a apartment should grow in Seconds")]
        [SerializeField]
        private float GrowSpeedMin;

        [SerializeField]
        private float GrowSpeedMax;

        [Header(
            "Building High, after that the Player lose. If a Player should lose,"
            + "when a building is higher than 5, set the value to 6.")]
        [Range(1,60)]
        [SerializeField]
        private int _buildingHight = 4;

        [Range(1,60)]
        [Header("Move speed of destructible. Move Speed for one field in seconds")]
        [SerializeField]
        private float _destructibleMoveSpeed = 0.25f;
        
        
        public int DestructablesPerPoints => destructablesPerPoints;
        public float SpawnDuration => spawnDuration;
        public float MoveUpSpeed => moveUpSpeed;
        public float SpawnInterval => spawnInterval;
        public float RotationDuration => rotationDuration;
        public float MoveDuration => moveDuration;
        public float BuildingHight => _buildingHight;
        public int BasePoint => basePoint;
        public float DestructibleMoveSpeed => _destructibleMoveSpeed;
        

        public GrowSpeed GetGrowSpeed() { return new GrowSpeed(GrowSpeedMin, GrowSpeedMax); }
        
        
    }

    public struct GrowSpeed {
        public float Min;
        public float Max;

        public GrowSpeed(float min, float max) {
            Min = min;
            Max = max;
        }
    }
}