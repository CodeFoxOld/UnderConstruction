using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    

public class CounterHive : MonoBehaviour {
    private GameManager gameManager;
    private List<Counter> counters = new List<Counter>();


    // Start is called before the first frame update
    void Start() {
        gameManager = GetComponent<GameManager>().RegisterCounterHive(this);
    }

    // Update is called once per frame
    void FixedUpdate() {
            if (!gameManager.UiManager.GamePaused)
                foreach (var counter in counters) {
                    counter.Next();
                }
        }
    public void RegisterCounter(Counter counter) { counters.Add(counter); }

    }
}

