using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class Settings : MonoBehaviour {
        [SerializeField] private float spawnDuration = 10;
        [SerializeField] private float moveUpSpeed = 1;
        [SerializeField] private float spawnInterval = 10;

        public float SpawnDuration => spawnDuration;
        public float MoveUpSpeed => moveUpSpeed;
        public float SpawnInterval => spawnInterval;
    }
}