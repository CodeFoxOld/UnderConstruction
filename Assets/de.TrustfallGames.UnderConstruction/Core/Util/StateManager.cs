using System;
using System.Collections;
using de.TrustfallGames.UnderConstruction.Core.tilemap;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.Util {
    [Obsolete("Do not use anymore", true)]
    public class StateManager : MonoBehaviour {
        [SerializeField] private GameObject gameManager;
        [SerializeField] private GameObject mapManager;
        [SerializeField] private GameObject character;
        [SerializeField] private GameObject spawnManager;
        private GameObject[] objs;

        [SerializeField] private float initInterval;

        // Start is called before the first frame update
        void Start() {
            objs = new [] {gameManager, mapManager, character, spawnManager};
            //Set the framerate to 30fps for mobile performance
            Application.targetFrameRate = 30;
            for (int i = 0; i < objs.Length; i++) {
                var init = Init(initInterval + (initInterval * i), objs[i]);
                StartCoroutine(init);
            }

            //Destroy(gameObject);
        }

        private IEnumerator Init(float initTime, GameObject gameObject) {
            WaitForSeconds wait = new WaitForSeconds(initTime);
            yield return wait;
            var a = Instantiate(gameObject);
            if (a.GetComponent<MapManager>() != null) {
                a.GetComponent<MapManager>().GenerateTilemap();
            }
            a.transform.SetAsFirstSibling();

            Debug.Log(gameObject.name + " spawned");
        }
    }
}