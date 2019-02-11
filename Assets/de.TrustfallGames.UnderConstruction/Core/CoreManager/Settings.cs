using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class Settings : MonoBehaviour {
        private GameManager _gameManager;

        [Header("The speed Houses comes out of the ground")]
        [SerializeField]
        private float moveUpSpeed = 1;

        [Header("Player rotation Speed")]
        [Range(0.02f, 1)]
        [SerializeField]
        private float rotationDuration = 0.1f;

        [Header("Player Movement Speed")]
        [UnityEngine.Range(0.02f, 1)]
        [SerializeField]
        private float moveDuration = 0.1f;

        [Header("The Base points for one stacked house")]
        [SerializeField]
        private int basePoint = 100;

        [Header("At how many Points should the player get one destructible")]
        [SerializeField]
        private int destructablesPerPoints = 1000;

        [Header("The max of destructibles a player can receive with one pick up")]
        [SerializeField]
        private int maxDestructablesPerCalc = 2;

        [Header(
            "Building High, after that the Player lose. If a Player should lose,"
            + "when a building is higher than 5, set the value to 6.")]
        [Range(1, 60)]
        [SerializeField]
        private int _buildingHight = 5;

        [Range(0.2f, 2)]
        [Header("Move speed of destructible. Move Speed for one field in seconds")]
        [SerializeField]
        private float _destructibleMoveSpeed = 0.25f;

        [Header("Percent of house obstacles.")]
        [SerializeField]
        [Range(0, 100)]
        private int housePercentage = 40;

        [Header("Percentage of color strike break. Higher value means more breaks.")]
        [SerializeField]
        [Range(0, 100)]
        private int saltGrains = 10;

        [Header("After how many seconds should a object spawn automatic, when the player is on the field?")]
        [SerializeField]
        private float earlySpawnTime = 0.5f;

        public int SaltGrains => saltGrains;
        public int HousePercentage => housePercentage;
        public int DestructablesPerPoints => destructablesPerPoints;
        public float MoveUpSpeed => moveUpSpeed;
        public float RotationDuration => rotationDuration;
        public float MoveDuration => moveDuration;
        public float BuildingHight => _buildingHight;
        public int BasePoint => basePoint;
        public float DestructibleMoveSpeed => _destructibleMoveSpeed;
        public float EarlySpawnTime => earlySpawnTime;
        public int MaxDestructablesPerCalc => maxDestructablesPerCalc;

        [Header("The point at which the game reaches the max difficulty")]
        [SerializeField]
        private float highScoreMax;

        [SerializeField] [Header("Max Amounts of Fields")]
        public int maxField;

        [Header("Start Amount of spawn duration")]
        [SerializeField]
        [Range(0.1f, 10)]
        private float spawnDurationStart;

        [Header("Min Amount of spawn duration")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float spawnDurationMin;

        [Header("Start Amount of spawn interval")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float spawnIntervalStart;

        [Header("Min Amount of spawn interval")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float spawnIntervalMin;

        [Header("Start Amount of grow speed")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float growIntervalStart;

        [Header("Min Amount of grow speed")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float growIntervalMin;

        public float GetSpawnDuration() {
            return GameManager.GetManager().Character.Highscore > highScoreMax ?
                       calc(spawnDurationStart, spawnDurationMin, highScoreMax, highScoreMax) :
                       calc(spawnDurationStart, spawnDurationMin, highScoreMax, GetGameManager().Character.Highscore);
        }

        public float GetSpawnInterval() {
            return GameManager.GetManager().Character.Highscore > highScoreMax ?
                       calc(spawnIntervalStart, spawnIntervalMin, highScoreMax, highScoreMax) :
                       calc(spawnIntervalStart, spawnIntervalMin, highScoreMax, GetGameManager().Character.Highscore);
        }

        public float GetGrowInterval() {
            return GameManager.GetManager().Character.Highscore > highScoreMax ?
                       calc(growIntervalStart, growIntervalMin, highScoreMax, highScoreMax) :
                       calc(growIntervalStart, growIntervalMin, highScoreMax, GetGameManager().Character.Highscore);
        }

        private float calc(float yStart, float min, float xGoal, float point) {
            float a = (float) Math.Pow((0 - xGoal), 2);
            float b = (float) Math.Pow((point - xGoal), 2);
            return ((yStart - min) / (a)) * b + min;
        }

        private GameManager GetGameManager() {
            if (_gameManager == null)
                _gameManager = GameManager.GetManager();
            return _gameManager;
        }
    }
}