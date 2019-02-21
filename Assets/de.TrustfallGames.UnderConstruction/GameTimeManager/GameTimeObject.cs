using System.Collections;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.GameTimeManager {
    public class GameTimeObject : MonoBehaviour {
        private GameTimeHandler gameTimeHandler;
        private MeshRenderer renderer;

        [SerializeField] private bool instantUpdate;
        [SerializeField] private bool useOwnColor;
        [SerializeField] private Color dayColor;
        [SerializeField] private Color nightColor;

        private Gradient blackToWhite;
        private Color setColor;

        private void Awake() {
            renderer = GetComponent<MeshRenderer>();

            gameTimeHandler = GameTimeHandler.GetInstance();
            gameTimeHandler.RegisterTimeObject(this);
        }

        private void Start() {
            blackToWhite = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].time = 0;
            colorKeys[0].color = useOwnColor ? nightColor : Color.white;
            colorKeys[1].time = 1;
            colorKeys[1].color = useOwnColor ? dayColor : Color.black;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].time = 0;
            alphaKeys[0].alpha = 1;
            alphaKeys[1].time = 1;
            alphaKeys[1].alpha = 1;

            blackToWhite.SetKeys(colorKeys, alphaKeys);


            if (useOwnColor) {
                TurnLightOnInstant(GetColor());
            } else {
                TurnLightOnInstant(gameTimeHandler.GetEmissionColor());
            }
        }

        private void FixedUpdate() {
            if (instantUpdate && useOwnColor) {
                renderer.material.SetColor("_EmissionColor", GetColor());
            }
        }

        public void ToggleLight(Color color) {
            if (instantUpdate) return;
            setColor = color;
            StartCoroutine(TurnLightOn(color));
        }

        private void OnDestroy() {
            if (gameTimeHandler == null) return;
            gameTimeHandler.RemoveTimeObject(this);
        }

        private IEnumerator TurnLightOn(Color color) {
            yield return new WaitForSeconds(Random.Range(0f, gameTimeHandler.LightOnScatter));
            renderer.material.SetColor("_EmissionColor", GetColor());
        }

        private void TurnLightOnInstant(Color color) { renderer.material.SetColor("_EmissionColor", color); }

        private Color GetColor() {
            if (!useOwnColor) {
                return setColor;
            }

            if (instantUpdate) {
                return blackToWhite.Evaluate(gameTimeHandler.DimValue());
            }
            
            return gameTimeHandler.LightState == LightState.On ? nightColor : dayColor;
        }
    }
}