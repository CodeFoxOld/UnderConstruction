using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class CounterHive : MonoBehaviour, IInternUpdate {
        private GameManager gameManager;
        private List<Counter> counters = new List<Counter>();

        // Start is called before the first frame update
        void Start() {
            gameManager = GetComponent<GameManager>().RegisterCounterHive(this);
            RegisterInternUpdate();
        }

        public void RegisterCounter(Counter counter) { counters.Add(counter); }

        public void InternUpdate() {
            if (gameManager.UiManager == null) return;
            if (!gameManager.UiManager.GamePaused)
                foreach (var counter in counters) {
                    counter.Next();
                }
        }

        public void RegisterInternUpdate() { gameManager.InternTick.RegisterTickObject(this, 100); }

        public void Init() { }
        
        public void OnDestroy() {
            gameManager.InternTick.RemoveTickObject(this);
        }

    }
}