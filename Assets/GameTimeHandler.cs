using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeHandler : MonoBehaviour {
    [SerializeField] private float turnLightsOn = 0.7f;

    [SerializeField]
    private Gradient dayToDawn;

    [SerializeField] private Color dayColor;
    [SerializeField] private Color dawnColor;
    [SerializeField] private Color nightColor;

    private float currentDayDuration;
    private float currentDawnDuration;
    private DayTime dayTime = DayTime.Day;
    private DayTime lastDayTime;

    [SerializeField] private float dayTimeDuration = 20;
    [SerializeField] private float dawnDuration = 10;
    private Light directionalLight;

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
        
        dayToDawn.SetKeys(colorKey,alphaKey);

        directionalLight = GetComponent<Light>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (dayTime == DayTime.Dawn) {
            if (lastDayTime == DayTime.Night) /*Go To Day*/ {
                currentDawnDuration -= Time.fixedDeltaTime;
                if (currentDawnDuration < 0) {
                    dayTime = DayTime.Day;
                    lastDayTime = DayTime.Dawn;
                    currentDayDuration = dayTimeDuration;
                }
            }

            if (lastDayTime == DayTime.Day) /*Go To Night*/ {
                currentDawnDuration += Time.fixedDeltaTime;
                if (currentDawnDuration > dawnDuration) {
                    dayTime = DayTime.Night;
                    lastDayTime = DayTime.Dawn;
                    currentDayDuration = dayTimeDuration;
                }
            }

            Debug.Log(Mathf.Clamp(currentDawnDuration, 0.001f, dawnDuration) / dawnDuration);
            directionalLight.color = dayToDawn.Evaluate(Mathf.Clamp(currentDawnDuration, 0.001f, dawnDuration) / dawnDuration);
        }

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

    private enum DayTime { Day, Dawn, Night }
}