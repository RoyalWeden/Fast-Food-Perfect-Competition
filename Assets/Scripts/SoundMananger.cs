using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour {
    public AudioClip dayAmbientAudio;
    public AudioClip nightAmbientAudio;
    public AudioClip dayMusic;
    public AudioClip nightMusic;
    public AudioClip[] randomDaySounds;
    public AudioClip[] randomNightSounds;
    public AudioSource ambientAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource randomAudioSource;
    private bool playedRandom;
    private float randomTime;

    void Start() {
        ambientAudioSource.Play();
        musicAudioSource.Play();
        PickRandomTime();
    }

    void Update() {
        if(Mathf.Abs(randomTime - Time.fixedTime) < .05f && !playedRandom) {
            playedRandom = true;
            PickRandomTime();
            if(musicAudioSource.clip == dayMusic) {
                if(randomDaySounds.Length > 0) {
                    randomAudioSource.PlayOneShot(randomDaySounds[Random.Range(0, randomDaySounds.Length)]);
                }
            } else {
                if(randomNightSounds.Length > 0) {
                    randomAudioSource.PlayOneShot(randomNightSounds[Random.Range(0, randomNightSounds.Length)]);
                }
            }
        }
    }

    public void SetDayTime() {
        ambientAudioSource.clip = dayAmbientAudio;
        musicAudioSource.clip = dayMusic;
        ambientAudioSource.Play();
        musicAudioSource.Play();
        ambientAudioSource.volume = 0.8f;
        musicAudioSource.volume = 0.45f;
    }

    public void SetNightTime() {
        ambientAudioSource.clip = nightAmbientAudio;
        musicAudioSource.clip = nightMusic;
        ambientAudioSource.Play();
        musicAudioSource.Play();
        ambientAudioSource.volume = 0.6f;
        musicAudioSource.volume = 0.6f;
    }

    public void PauseAudio() {
        ambientAudioSource.Pause();
        musicAudioSource.Pause();
    }
    public void ResumeAudio() {
        ambientAudioSource.Play();
        musicAudioSource.Play();
    }
    private void PickRandomTime() {
        randomTime = Random.Range(Time.fixedTime, Time.fixedTime + 10f);
        playedRandom = false;
    }
}
