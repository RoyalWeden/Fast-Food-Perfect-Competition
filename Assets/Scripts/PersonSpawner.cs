using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonSpawner : MonoBehaviour {
    public GameObject personPrefab;
    private List<GameObject> peopleObjs;
    public Button pauseButton;

    void Start() {
        peopleObjs = new List<GameObject>();
        for(int i=0; i<GameManager.GetPersonCount(); i++) {
            peopleObjs.Add(Instantiate(personPrefab, new Vector2(-2.307f, -0.079f), Quaternion.Euler(0, 0, 0), transform));
        }
    }

    // Update is called once per frame
    void Update() {
        if(GameManager.IsGamePaused()) {
            PersonController.FreezeMovement();
            foreach(GameObject g in peopleObjs) {
                g.GetComponent<Animator>().speed = 0;
            }
        } else {
            PersonController.UnfreezeMovement();
            foreach(GameObject g in peopleObjs) {
                g.GetComponent<Animator>().speed = 1;
            }
        }
    }
}
