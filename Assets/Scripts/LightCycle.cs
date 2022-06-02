using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCycle : MonoBehaviour {
    private Light spotLight;
    private float dayTimeLength; // seconds
    public ForegroundUpdater foregroundUpdater;
    private SoundMananger soundManager;
    private float timeOffsetPaused;
    private float pausedTime;
    private float timeOffsetPlay;

    void Start() {
        spotLight = GetComponent<Light>();
        dayTimeLength = 3f * 60;
        foregroundUpdater.TurnLightsOff();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundMananger>();
        soundManager.SetDayTime();
    }

    void Update() {
        if(!GameManager.IsGamePaused()) {
            timeOffsetPlay += timeOffsetPaused;
            timeOffsetPaused = 0;
            pausedTime = 0;
        } else {
            if(pausedTime == 0) {
                pausedTime = Time.fixedTime;
            }
            timeOffsetPaused = Time.fixedTime - pausedTime;
        }
        spotLight.intensity = 3.5f * Mathf.Cos(((2 * Mathf.PI) / (dayTimeLength * 2)) * (Time.fixedTime - timeOffsetPlay - timeOffsetPaused));
        Color backColor = Color.HSVToRGB(217f / 360f, 56f / 100f, spotLight.intensity / 3.5f);
        Camera.main.backgroundColor = backColor;
        if(!foregroundUpdater.AreLightsOn() && spotLight.intensity < 0.2f) {
            foregroundUpdater.TurnLightsOn();
            soundManager.SetNightTime();
        }
        if(foregroundUpdater.AreLightsOn() && spotLight.intensity > 0.2f) {
            foregroundUpdater.TurnLightsOff();
            soundManager.SetDayTime();
        }
    }
}
