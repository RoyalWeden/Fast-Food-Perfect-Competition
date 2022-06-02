using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private TextMeshProUGUI tooltipText;
    private float hideTooltipTime;
    private Vector2 tooltipSize;
    private Transform tooltipPanel;
    private int objectOrderLayer;

    void Start() {
        tooltipPanel = transform.GetChild(0);
        tooltipText = tooltipPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        Vector2 mousePos = Input.mousePosition;
        if(mousePos[0] > 250) {
            transform.position = new Vector2(mousePos[0], mousePos[1]);
        } else {
            transform.position = new Vector2(mousePos[0] + Screen.width / 4.25f, mousePos[1]);
        }
        if(hideTooltipTime < Time.fixedTime || GameManager.IsInMenu()) {
            tooltipPanel.GetComponent<Image>().enabled = false;
            tooltipText.enabled = false;
            objectOrderLayer = -1;
        } else {
            tooltipPanel.GetComponent<Image>().enabled = true;
            tooltipText.enabled = true;
        }
    }

    public void SetTooltipText(string s, int objSortOrder) {
        List<string> allPersonNames = new List<string>();
        foreach(string name in GameManager.GetAllPersonNames()) {
            allPersonNames.Add(name);
        }
        if((tooltipText.enabled && !allPersonNames.Contains(tooltipText.text)) || !tooltipText.enabled) {
            if(objSortOrder > objectOrderLayer) {
                hideTooltipTime = Time.fixedTime + 0.35f;
                tooltipText.SetText(s);
                objectOrderLayer = objSortOrder;
            }
        }
    }

    public string GetTooltipText() {
        return tooltipText.text;
    }
}
