using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundUpdater : MonoBehaviour {
    private List<Light> lampLights;
    private bool areLightsOn = true;

    void Start() {
        lampLights = new List<Light>();
        foreach(Transform tr in transform) {
            if(tr.name.Contains("lamp")) {
                foreach(Transform light in tr) {
                    lampLights.Add(light.GetComponent<Light>());
                }
            }
        }
    }

    void Update() {
        foreach(Transform g in transform) {
            g.GetComponent<SpriteRenderer>().sortingOrder = (int)Mathf.Abs((g.transform.position.y - 0.159f) * 1000);
        }
    }

    public void TurnLightsOff() {
        foreach(Light light in lampLights) {
            light.enabled = false;
            areLightsOn = false;
        }
    }

    public void TurnLightsOn() {
        foreach(Light light in lampLights) {
            light.enabled = true;
            areLightsOn = true;
        }
    }

    public bool AreLightsOn() {
        return areLightsOn;
    }
}
