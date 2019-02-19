using System.Collections;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.GameTimeManager {
    public class GameTimeObject : MonoBehaviour {
        private GameTimeHandler gameTimeHandler;
        private MeshRenderer renderer;

        private void Start() {
            gameTimeHandler = GameTimeHandler.GetInstance();
            gameTimeHandler.RegisterTimeObject(this);
            TurnLightOn(gameTimeHandler.GetEmissionColor());
            renderer = GetComponent<MeshRenderer>();
        }

        public void ToggleLight(Color color) { StartCoroutine(TurnLightOn(color)); }

        private void OnDestroy() {
            if(gameTimeHandler == null) return;
            gameTimeHandler.RemoveTimeObject(this);
        }

        private IEnumerator TurnLightOn(Color color) {
            yield return new WaitForSeconds(Random.Range(0f,gameTimeHandler.LightOnScatter));
            renderer.material.SetColor("_EmissionColor", color);
        }
    }
}