using System.Collections;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.GameTimeManager {
    public class GameTimeObject : MonoBehaviour {
        private GameTimeHandler timeHandler;
        private MeshRenderer renderer;

        [SerializeField] private bool instantUpdate;
        [SerializeField] private bool useOwnColor;
        [SerializeField] private Color dayColor;
        [SerializeField] private Color nightColor;

        private Gradient blackToWhite;
        private Color setColor;

        private void Awake() {
            renderer = GetComponent<MeshRenderer>();

            //gameTimeHandler = GameTimeHandler.GetInstance();
            //gameTimeHandler.RegisterTimeObject(this);
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
                TurnLightOnInstant(GameTimeHandler.GetInstance().GetEmissionColor());
            }
        }

        private void FixedUpdate() {
            if (instantUpdate && useOwnColor) {
                renderer.material.SetColor("_EmissionColor", GetColor());
            }

            /*if (!GameTimeHandler.GetInstance().IsRegistered(this)) {
                gameTimeHandler = GameTimeHandler.GetInstance();
                gameTimeHandler.RegisterTimeObject(this);
            }*/
        }

        /// <summary>
        /// Starts a coroutine to turn on the lights
        /// </summary>
        /// <param name="color"></param>
        public void ToggleLight(Color color) {
            if (instantUpdate) return;
            setColor = color;
            StartCoroutine(TurnLightOn(color));
        }

        /// <summary>
        /// Removed time object from hive
        /// </summary>
        private void OnDestroy() {
            if (timeHandler == null) return;
            timeHandler.RemoveTimeObject(this);
        }

        /// <summary>
        /// Turns the light on after a random time depending on the light scatter in game time handler
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private IEnumerator TurnLightOn(Color color) {
            yield return new WaitForSeconds(Random.Range(0f, timeHandler.LightOnScatter));
            renderer.material.SetColor("_EmissionColor", GetColor());
        }

        /// <summary>
        /// turns the lights instant on
        /// </summary>
        /// <param name="color"></param>
        private void TurnLightOnInstant(Color color) { renderer.material.SetColor("_EmissionColor", color); }

        /// <summary>
        /// returns the color which is used for the emission map
        /// </summary>
        /// <returns></returns>
        private Color GetColor() {
            if (!useOwnColor) {
                return setColor;
            }

            if (instantUpdate) {
                return blackToWhite.Evaluate(GameTimeHandler.GetInstance().DimValue());
            }

            return GameTimeHandler.GetInstance().LightState == LightState.On ? nightColor : dayColor;
        }

        public GameTimeHandler TimeHandler { get => timeHandler; set => timeHandler = value; }
    }
}