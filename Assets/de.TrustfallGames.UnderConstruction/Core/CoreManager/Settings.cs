using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class Settings : MonoBehaviour {
        [SerializeField] private float spawnDuration = 10;
        [SerializeField] private float moveUpSpeed = 1;
        [SerializeField] private float spawnInterval = 10;

        [Range(0.01f, 1)]
        [SerializeField]
        private float rotationDuration = 0.1f;

        [Range(0.01f, 1)]
        [SerializeField]
        private float moveDuration = 0.1f;

        [SerializeField] private int basePoint = 100;

        public float SpawnDuration => spawnDuration;
        public float MoveUpSpeed => moveUpSpeed;
        public float SpawnInterval => spawnInterval;
        public float RotationDuration => rotationDuration;
        public float MoveDuration => moveDuration;
        public int BasePoint => basePoint;
    }
}