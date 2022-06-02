using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyController : MonoBehaviour {
    public string productName;
    private GameObject toolTip;
    private BoxCollider2D boxCollider;
    private MenuManager menuManager;

    void Start() {
        toolTip = GameObject.Find("Tooltip");
        boxCollider = GetComponent<BoxCollider2D>();
        menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
    }


    void Update() {
        if(IsMouseInRange() && Input.GetMouseButtonDown(0) && !GameManager.IsInMenu() && toolTip.GetComponent<Tooltip>().GetTooltipText() == gameObject.name) {
            OpenCompanyPanel();
        }
        ChangeToolTipText();
    }

    private void ChangeToolTipText() {
        if(IsMouseInRange()) {
           toolTip.GetComponent<Tooltip>().SetTooltipText(gameObject.name, GetComponent<SpriteRenderer>().sortingOrder); 
        }
    }

    private bool IsMouseInRange() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 boxSize = boxCollider.size;
        if(mousePos[0] > transform.position.x - boxSize[0]/2 && mousePos[0] < transform.position.x + boxSize[0]/2 && mousePos[1] > transform.position.y - boxSize[1]/2 && mousePos[1] < transform.position.y + boxSize[1]/2) {
            return true;
        }
        return false;
    }

    private void OpenCompanyPanel() {
        menuManager.OpenCompanyPanel(gameObject.name, productName);
    }
}
