using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CameraController : MonoBehaviour {
    public float cameraSpeed;
    public TextMeshProUGUI pauseButtonText;

    private void Awake() {
        GameManager.ResetGame();
    }

    void Start() {
        transform.position = new Vector3(0, 0, -10);
        foreach(SpriteRenderer g in GameObject.FindObjectsOfType<SpriteRenderer>()) {
            g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, Random.Range(-1f, 0f));
        }
    }

    void Update() {
        Vector2 mousePos = Input.mousePosition;
        if(!GameManager.IsInMenu()) {
            if(mousePos.x < Screen.width / 6 && transform.position.x > -2) {
                transform.position = new Vector3(transform.position.x - cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            if(mousePos.x > Screen.width - Screen.width / 6 && transform.position.x < 2) {
                transform.position = new Vector3(transform.position.x + cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }

        if(GameManager.IsGamePaused()) {
            pauseButtonText.SetText("Resume");
        } else {
            pauseButtonText.SetText("Pause");
        }
    }

    // Reset everything
    public void ResetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.ResetGame();
    }

    public void CloseGame() {
        Application.Quit();
    }

    public void PauseResumeGame() {
        GameManager.PauseResumeGame();
    }
}
