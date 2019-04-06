using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    /// <summary>
    /// Class to save the settings
    /// </summary>
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

        [FormerlySerializedAs("_buildingHight")]
        [Header(
            "Building High, after that the Player lose. If a Player should lose,"
            + "when a building is higher than 5, set the value to 6.")]
        [Range(1, 60)]
        [SerializeField]
        private int _buildingHeight = 5;

        [Range(0.2f, 2)]
        [Header("Move speed of destructible. Move Speed for one field in seconds")]
        [SerializeField]
        private float _destructibleMoveSpeed = 0.25f;

        [Header("Minimum/Start Percent of house obstacles.")]
        [SerializeField]
        [Range(0, 100)]
        private int minHousePercentage = 30;
        
        [Header("Maximum/End Percent of house obstacles.")]
        [SerializeField]
        [Range(0, 100)]
        private int maxHousePercentage = 80;

        [Header("Percentage of color strike break. Higher value means more breaks.")]
        [SerializeField]
        [Range(0, 100)]
        private int saltGrains = 10;

        [Header("After how many seconds should a object spawn automatic, when the player is on the field?")]
        [SerializeField]
        private float earlySpawnTime = 0.5f;

        [Header("The highscore at which the game reaches the max difficulty")]
        [SerializeField]
        private int highScoreMax = 10000;

        [Header("The gametime at which the game reaches the max difficulty")]
        [SerializeField]
        private int timeMax = 180;

        [SerializeField]
        [Header("Max Amount of Fields")]
        [Range(2, 6)]
        private int maxField = 2;

        [SerializeField]
        [Header("Min Amount of Fields")]
        [Range(2, 6)]
        private int minField = 2;

        [Header("Start Amount of spawn duration")]
        [SerializeField]
        [Range(0.1f, 10)]
        private float spawnDurationStart = 20;

        [Header("Min Amount of spawn duration")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float spawnDurationMin = 2;

        [Header("Start Amount of spawn interval")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float spawnIntervalStart = 20;

        [Header("Min Amount of spawn interval")]
        [SerializeField]
        [Range(0.1f, 10f)]
        private float spawnIntervalMin = 2;

        [Header("Start Amount of grow speed")]
        [SerializeField]
        [Range(0.1f, 30f)]
        private float growIntervalStart = 30;

        [Header("Min Amount of grow speed")]
        [SerializeField]
        [Range(0.1f, 30f)]
        private float growIntervalMin = 5;

        public int   SaltGrains              => saltGrains;

        public int   DestructablesPerPoints  => destructablesPerPoints;
        public float MoveUpSpeed             => moveUpSpeed;
        public float RotationDuration        => rotationDuration;
        public float MoveDuration            => moveDuration;
        public int   BuildingHeight          => _buildingHeight;
        public int   BasePoint               => basePoint;
        public float DestructibleMoveSpeed   => _destructibleMoveSpeed;
        public float EarlySpawnTime          => earlySpawnTime;
        public int   MaxDestructablesPerCalc => maxDestructablesPerCalc;
        public int   MaxField                => maxField;
        public int   MinField                => minField;
        public int   HighScoreMax            => highScoreMax;

        /// <summary>
        /// Returns the house percentage depending on current played time and current highscore 
        /// </summary>
        /// <returns></returns>
        public int GetHousePercentage() {
            return Mathf.RoundToInt(((GameManager.GetManager().Character.Highscore > highScoreMax ?
                    Calc(minHousePercentage, maxHousePercentage, highScoreMax, highScoreMax) :
                    Calc(minHousePercentage, maxHousePercentage, highScoreMax, GetGameManager().Character.Highscore))
                + (Time.time > timeMax ? Calc(minHousePercentage, maxHousePercentage, timeMax, timeMax) :
                    Calc(minHousePercentage, maxHousePercentage, timeMax, Time.time))) / 2);

        }

        
        /// <summary>
        /// Returns the spawn Duration depending on current played time and current highscore
        /// </summary>
        /// <returns></returns>
        public float GetSpawnDuration() {
            return ((GameManager.GetManager().Character.Highscore > highScoreMax ?
                         Calc(spawnDurationStart, spawnDurationMin, highScoreMax, highScoreMax) :
                         Calc(spawnDurationStart, spawnDurationMin, highScoreMax, GetGameManager().Character.Highscore))
                    + (Time.time > timeMax ? Calc(spawnDurationStart, spawnDurationMin, timeMax, timeMax) :
                           Calc(spawnDurationStart, spawnDurationMin, timeMax, Time.time))) / 2;
        }

        /// <summary>
        /// Returns the Spawn interval depending on current played time and current highscore
        /// </summary>
        /// <returns></returns>
        public float GetSpawnInterval() {
            return ((GameManager.GetManager().Character.Highscore > highScoreMax ?
                         Calc(spawnIntervalStart, spawnIntervalMin, highScoreMax, highScoreMax) :
                         Calc(spawnIntervalStart, spawnIntervalMin, highScoreMax, GetGameManager().Character.Highscore))
                    + (Time.time > timeMax ? Calc(spawnIntervalStart, spawnIntervalMin, timeMax, timeMax) :
                           Calc(spawnIntervalStart, spawnIntervalMin, timeMax, Time.time))) / 2;
        }

        /// <summary>
        /// Returns the Grow interval depending on current played time and current highscore
        /// </summary>
        /// <returns></returns>
        public float GetGrowInterval() {
            return ((GameManager.GetManager().Character.Highscore > highScoreMax ?
                         Calc(growIntervalStart, growIntervalMin, highScoreMax, highScoreMax) :
                         Calc(growIntervalStart, growIntervalMin, highScoreMax, GetGameManager().Character.Highscore))
                    + (Time.time > timeMax ? Calc(growIntervalStart, growIntervalMin, timeMax, timeMax) :
                           Calc(growIntervalStart, growIntervalMin, timeMax, Time.time))) / 2;
        }

        /// <summary>
        /// Method to get a point on a parable with the specified parameter
        /// </summary>
        /// <param name="yAtX0">Value of f(0)</param>
        /// <param name="xAtPoint">Value of x at f(x)=yAtPointX</param>
        /// <param name="yAtPointX">Value of f(xAtPoint)</param>
        /// <param name="x">X at f(x) on the generated parable</param>
        /// <returns></returns>
        private float Calc(float yAtX0, float yAtPointX, float xAtPoint, float x) {
            float a = (float) Math.Pow((0 - xAtPoint), 2);
            float b = (float) Math.Pow((x - xAtPoint), 2);
            return ((yAtX0 - yAtPointX) / (a)) * b + yAtPointX;
        }

        private GameManager GetGameManager() {
            if (_gameManager == null)
                _gameManager = GameManager.GetManager();
            return _gameManager;
        }
    }
}