using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour {
    private float dailyIncome;
    private int gender;
    private Animator animator;
    private float moveToPosX;
    private bool isMoving;
    private bool isMovingTowardsBuildingX;
    private bool isAtBuilding;
    // When walking, the person stays at this Y coordinate
    private float yPosLane;
    // Closest the person can get to buildings
    private float yPosBuildingStop = -0.079f;
    private float moveSpeed;
    private float moveSpeedY;
    private bool approachBuilding;
    private bool leaveBuilding;
    private string companyGoToName;

    private GameObject toolTip;
    private BoxCollider2D boxCollider;
    private string personName;

    private SpriteRenderer spriteRenderer;

    private float chooseBuildingTime;
    private float leaveBuildingTime;

    private bool hasBoughtProduct;

    private MenuManager menuManager;

    private static bool freezeMovement;

    void Start() {
        menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
        chooseBuildingTime = 1f;
        animator = GetComponent<Animator>();
        moveSpeedY = 0.01f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        dailyIncome = Random.Range(60f, 300f);
        gender = Random.Range(0, 2);
        if(gender == 0) {
            animator.SetTrigger("Man" + Random.Range(0, 3).ToString());
            personName = GameManager.GetRandomPersonName(true);
        } else {
            animator.SetTrigger("Woman" + Random.Range(0, 3).ToString());
            personName = GameManager.GetRandomPersonName(false);
        }
        yPosLane = Random.Range(-0.166f, -0.086f);
        moveSpeed = Random.Range(0.1f, 0.3f);
        toolTip = GameObject.Find("Tooltip");
        boxCollider = GetComponent<BoxCollider2D>();
    }


    void Update() {
        spriteRenderer.sortingOrder = (int)Mathf.Abs((transform.position.y - 0.063f) * 1000);
        if(Time.fixedTime == chooseBuildingTime) {
            ChooseBuildingMove();
        }

        // Hide person
        if(isAtBuilding) {
            spriteRenderer.enabled = false;
            if(Time.fixedTime >= leaveBuildingTime) {
                isAtBuilding = false;
                leaveBuilding = true;
            }
        } else {
            spriteRenderer.enabled = true;
            ChangeToolTipText();
        }

        if(IsMouseInRange() && !GameManager.IsInMenu() && Input.GetMouseButtonDown(0) && toolTip.GetComponent<Tooltip>().GetTooltipText() == personName) {
            OpenPersonPanel();
        }

        if(isMoving && !freezeMovement) {
            animator.SetTrigger("Moving");
            animator.ResetTrigger("Idle");
            // Turn person to facing position
            if(isMovingTowardsBuildingX) {
                if(Mathf.Abs(moveToPosX - transform.position.x) < .005f) {
                    transform.position = new Vector2(moveToPosX, transform.position.y);
                }
                if(moveToPosX < transform.position.x) {
                    transform.localScale = new Vector3(-1, 1, 1);
                    if(IsMouseInRange() && !GameManager.IsInMenu()) {
                        transform.position = new Vector2(transform.position.x - (moveSpeed / 2f) * Time.deltaTime, transform.position.y);
                    } else {
                        transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
                    }
                } else if(moveToPosX > transform.position.x) {
                    transform.localScale = new Vector3(1, 1, 1);
                    if(IsMouseInRange() && !GameManager.IsInMenu()) {
                        transform.position = new Vector2(transform.position.x + (moveSpeed / 2f) * Time.deltaTime, transform.position.y);
                    } else {
                        transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
                    }
                } else {
                    isMovingTowardsBuildingX = false;
                    approachBuilding = true;
                }
                if(yPosLane < transform.position.y) {
                    transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeedY * Time.deltaTime);
                } else if(yPosLane > transform.position.y) {
                    transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeedY * Time.deltaTime);
                }
            }
            if(moveToPosX == transform.position.x) {
                if(approachBuilding) {
                    // Move person towards building at slow speed (0.065f)
                    transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeedY * Time.deltaTime);
                    if(Mathf.Abs(transform.position.y - yPosBuildingStop) < 0.005f) {
                        approachBuilding = false;
                        isAtBuilding = true;
                        leaveBuildingTime = Time.fixedTime + 6f;
                    }
                }
                if(leaveBuilding) {
                    if(!hasBoughtProduct) {
                        hasBoughtProduct = true;
                        GameManager.PurchaseGood(personName, companyGoToName);
                    }
                    // Move person away from building at slow speed (0.065f)
                    transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeedY * Time.deltaTime);
                    if(Mathf.Abs(transform.position.y - yPosLane) < 0.005f) {
                        leaveBuilding = false;
                        hasBoughtProduct = false;
                        chooseBuildingTime = Time.fixedTime + 9f;
                        isMoving = false;
                    }
                }
            }
        } else {
            animator.ResetTrigger("Moving");
            animator.SetTrigger("Idle");
        }
    }

    private void ChooseBuildingMove() {
        companyGoToName = GameManager.GetCompanyHighestMUD(personName);
        Vector2 companyTempPos;
        if(companyGoToName != null) {
            companyTempPos = GameManager.GetFirmPos(companyGoToName);
        } else {
            GameManager.GetRandomFirmPos(out companyGoToName, out companyTempPos);
        }
        moveToPosX = companyTempPos[0];
        isMoving = true;
        isMovingTowardsBuildingX = true;
    }

    private void ChangeToolTipText() {
        if(IsMouseInRange()) {
           toolTip.GetComponent<Tooltip>().SetTooltipText(personName, GetComponent<SpriteRenderer>().sortingOrder); 
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

    private void OpenPersonPanel() {
        menuManager.OpenPersonPanel(personName);
    }

    public static void FreezeMovement() {
        freezeMovement = true;
    }
    public static void UnfreezeMovement() {
        freezeMovement = false;
    }
    public static bool GetFreezeMovement() {
        return freezeMovement;
    }
}
