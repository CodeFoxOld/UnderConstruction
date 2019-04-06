using System;
using System.Collections.Generic;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.GameTimeManager {
    public class GameTimeHandler : MonoBehaviour {
        [SerializeField]
        private Gradient dayToDawn;

        [SerializeField] private Color dayColor;
        [SerializeField] private Color dawnColor;
        [SerializeField] private Color nightColor;

        [SerializeField] private Color dayEmissionColor;
        [SerializeField] private Color nightEmissionColor;

        [SerializeField] private Image dayBg;
        [SerializeField] private Image dawnBg;

        private float currentDayDuration;
        private float currentDawnDuration;
        private DayTime dayTime = DayTime.Day;
        private DayTime lastDayTime;

        [SerializeField] private float dayTimeDuration = 20;
        [SerializeField] private float dawnDuration = 10;

        [Range(0, 1)]
        [SerializeField]
        private float turnLightsOn;

        [SerializeField] private float lightOnScatter;

        private Light directionalLight;
        private static GameTimeHandler _instance;

        private List<GameTimeObject> timeObjects = new List<GameTimeObject>();

        private LightState lightState = LightState.Off;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }

        public static GameTimeHandler GetInstance() { return _instance; }

        /// <summary>
        /// Register at time hive
        /// </summary>
        /// <param name="timeObject"></param>
        public void RegisterTimeObject(GameTimeObject timeObject) { timeObjects.Add(timeObject); }

        public void RemoveTimeObject(GameTimeObject timeObject) {
            timeObjects.RemoveAt(timeObjects.FindIndex(timeObject.Equals));
        }

        /// <summary>
        /// Returns if it is night and returns the global emissive color
        /// </summary>
        /// <param name="emissiveColor"></param>
        /// <returns></returns>
        public bool isNight(out Color emissiveColor) {
            if (dawnDuration > dawnDuration * turnLightsOn) {
                emissiveColor = nightColor;
                return true;
            }

            emissiveColor = dayColor;
            return false;
        }

        // Start is called before the first frame update
        void Start() {
            dayToDawn = new Gradient();
            currentDayDuration = dayTimeDuration;
            var colorKey = new GradientColorKey[3];
            colorKey[0].color = dayColor;
            colorKey[0].time = 0.0f;
            colorKey[1].color = dawnColor;
            colorKey[1].time = 0.5f;
            colorKey[2].color = nightColor;
            colorKey[2].time = 1.0f;

            var alphaKey = new GradientAlphaKey[3];
            alphaKey[0].alpha = 1f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1f;
            alphaKey[1].time = 0.5f;
            alphaKey[2].alpha = 1f;
            alphaKey[2].time = 1.0f;

            dayToDawn.SetKeys(colorKey, alphaKey);

            directionalLight = GetComponent<Light>();
        }

        // Update is called once per frame
        private void FixedUpdate() {
            if (currentDawnDuration > dawnDuration * turnLightsOn && lightState == LightState.Off) {
                ToggleLights(LightState.On);
            } else if (currentDawnDuration < dawnDuration * turnLightsOn && lightState == LightState.On) {
                ToggleLights(LightState.Off);
            }

            //Dimm Lights
            if (dayTime == DayTime.Dawn) {
                currentDawnDuration = lastDayTime == DayTime.Night ? currentDawnDuration - Time.fixedDeltaTime :
                                          currentDawnDuration + Time.fixedDeltaTime;

                if (lastDayTime == DayTime.Night) /*Go To Day*/ {
                    if (currentDawnDuration < 0) {
                        SetDayTime(DayTime.Day);
                    }
                }

                if (lastDayTime == DayTime.Day) /*Go To Night*/ {
                    if (currentDawnDuration > dawnDuration) {
                        SetDayTime(DayTime.Night);
                    }
                }

                void SetDayTime(DayTime time) {
                    dayTime = time;
                    lastDayTime = DayTime.Dawn;
                    currentDayDuration = dayTimeDuration;
                }

                var a = dawnDuration / 2;

                //Fade night
                if (currentDawnDuration > a) {
                    var color = dayBg.color;
                    dayBg.color = new Color(color.r, color.g, color.b, 0);

                    color = dawnBg.color;
                    color = new Color(color.r, color.g, color.b, 1 - ((GetClampedDawnDuration() - a) / a));
                    dawnBg.color = color;
                }

                // Fade day
                else {
                    var color = dawnBg.color;
                    dawnBg.color = new Color(color.r, color.g, color.b, 1);

                    color = dayBg.color;
                    dayBg.color = new Color(color.r, color.g, color.b, 1 - (GetClampedDawnDuration() / a));
                }

                float GetClampedDawnDuration() { return Mathf.Clamp(currentDawnDuration, 0.001f, dawnDuration); }

                directionalLight.color =
                    dayToDawn.Evaluate(Mathf.Clamp(currentDawnDuration, 0.001f, dawnDuration) / dawnDuration);
            }

            //Static Light
            if (dayTime == DayTime.Day || dayTime == DayTime.Night) {
                currentDayDuration -= Time.fixedDeltaTime;
                if (currentDayDuration < 0) {
                    lastDayTime = dayTime;
                    dayTime = DayTime.Dawn;
                    if (lastDayTime == DayTime.Day)
                        currentDawnDuration = 0;
                    if (lastDayTime == DayTime.Night)
                        currentDawnDuration = dawnDuration;
                }
            }
        }

        /// <summary>
        /// Toggle the lights to the desired state
        /// </summary>
        /// <param name="state"></param>
        private void ToggleLights(LightState state) {
            lightState = state;
            for (int i = 0; i < timeObjects.Count; i++) {
                if (timeObjects[i] != null) {
                    timeObjects[i].ToggleLight(state == LightState.On ? nightEmissionColor : dayEmissionColor);
                } else {
                    timeObjects.RemoveAt(i);
                    i--;
                }
            }
        }

        public float LightOnScatter => lightOnScatter;

        public Color GetEmissionColor() { return lightState == LightState.On ? nightEmissionColor : dayEmissionColor; }

        public LightState LightState => lightState;

        private enum DayTime { Day, Dawn, Night }

        /// <summary>
        /// Fades the color depending on the current time
        /// </summary>
        /// <returns></returns>
        public float DimValue() {
            if (GetClampedDawnDuration() > dawnDuration * turnLightsOn) {
                float b = dawnDuration - dawnDuration * turnLightsOn;
                float c = GetClampedDawnDuration() - dawnDuration * turnLightsOn;

                return 1-c/b;
            }

            return 1;
        }

        float GetClampedDawnDuration() { return Mathf.Clamp(currentDawnDuration, 0.001f, dawnDuration); }

        private void OnDestroy() {
            _instance = null;
        }
    }
    
    

    public enum LightState { Off, On }
}