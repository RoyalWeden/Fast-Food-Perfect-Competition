using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {
    // Market managing
    private Button marketButton;
    public Transform marketPanel;
    private TextMeshProUGUI marketDemandElasticityText;
    private TextMeshProUGUI marketSupplyElasticityText;
    private RectTransform marketPanelSupplyCurve;
    private RectTransform marketPanelDemandCurve;
    private TextMeshProUGUI marketEquilQText;
    private TextMeshProUGUI marketEquilPText;

    // Firm managing
    public Transform companyPanel;
    private TextMeshProUGUI companyPanelTitle;
    private RectTransform companyPanelSupplyCurve;
    private RectTransform companyPanelDemandCurve;
    private TextMeshProUGUI companyPanelEquilQText;
    private TextMeshProUGUI companyPanelEquilQTextMC;
    private TextMeshProUGUI companyPanelEquilPText;
    private TextMeshProUGUI companyPanelElasticityText;

    // Consumer utility
    public Transform personPanel;
    private TextMeshProUGUI personNameText;
    private TextMeshProUGUI personCompanyNameText;
    private Transform marginalUtilValueText;
    private TextMeshProUGUI personPanelElasticityText;
    private int marginalUtilCompanyIndex;
    private TextMeshProUGUI incomeText;
    private TextMeshProUGUI disposableIncomeText;
    private TextMeshProUGUI personBurgerCount;
    private TextMeshProUGUI personCoffeeCount;
    private TextMeshProUGUI personDonutCount;
    private TextMeshProUGUI personIceCreamCount;
    private TextMeshProUGUI personPieCount;
    private TextMeshProUGUI personPizzaCount;
    private TextMeshProUGUI personSalmonCount;
    private TextMeshProUGUI personTacoCount;

    void Start() {
        marketButton = transform.Find("Show Market").GetComponent<Button>();

        marketDemandElasticityText = marketPanel.Find("Elasticity").Find("Demand Elasticity").Find("Value").GetComponent<TextMeshProUGUI>();
        marketSupplyElasticityText = marketPanel.Find("Elasticity").Find("Supply Elasticity").Find("Value").GetComponent<TextMeshProUGUI>();
        marketPanelSupplyCurve = marketPanel.Find("DS Graph").Find("Supply Curve").GetComponent<RectTransform>();
        marketPanelDemandCurve = marketPanel.Find("DS Graph").Find("Demand Curve").GetComponent<RectTransform>();
        marketEquilQText = marketPanel.Find("DS Graph").Find("X Axis").Find("Equilibrium Quantity").GetComponent<TextMeshProUGUI>();
        marketEquilPText = marketPanel.Find("DS Graph").Find("Y Axis").Find("Equilibrium Price").GetComponent<TextMeshProUGUI>();

        companyPanelTitle = companyPanel.Find("Title").GetComponent<TextMeshProUGUI>();
        companyPanelElasticityText = companyPanel.Find("Elasticity").Find("Value").GetComponent<TextMeshProUGUI>();
        companyPanelSupplyCurve = companyPanel.Find("Graphs").Find("DS Graph").Find("Supply Curve").GetComponent<RectTransform>();
        companyPanelDemandCurve = companyPanel.Find("Graphs").Find("DS Graph").Find("Demand Curve").GetComponent<RectTransform>();
        companyPanelEquilQText = companyPanel.Find("Graphs").Find("DS Graph").Find("X Axis").Find("Equilibrium Quantity").GetComponent<TextMeshProUGUI>();
        companyPanelEquilQTextMC = companyPanel.Find("Graphs").Find("MRMC Graph").Find("X Axis").Find("Equilibrium Quantity").GetComponent<TextMeshProUGUI>();
        companyPanelEquilPText = companyPanel.Find("Graphs").Find("DS Graph").Find("Y Axis").Find("Equilibrium Price").GetComponent<TextMeshProUGUI>();

        personNameText = personPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        personCompanyNameText = personPanel.Find("Marginal Utility Panel").Find("QMU Panel").Find("Company Name").GetComponent<TextMeshProUGUI>();
        marginalUtilValueText = personPanel.Find("Marginal Utility Panel").Find("QMU Panel").Find("MU Column").Find("Values");
        personPanelElasticityText = personPanel.Find("Elasticity").Find("Value").GetComponent<TextMeshProUGUI>();
        incomeText = personPanel.Find("Purchases").Find("Income").Find("Value").GetComponent<TextMeshProUGUI>();
        disposableIncomeText = personPanel.Find("Purchases").Find("Disposable Income").Find("Value").GetComponent<TextMeshProUGUI>();
        personBurgerCount = personPanel.Find("Purchases").Find("Burgers").Find("Value").GetComponent<TextMeshProUGUI>();
        personCoffeeCount = personPanel.Find("Purchases").Find("Coffee").Find("Value").GetComponent<TextMeshProUGUI>();
        personDonutCount = personPanel.Find("Purchases").Find("Donuts").Find("Value").GetComponent<TextMeshProUGUI>();
        personIceCreamCount = personPanel.Find("Purchases").Find("Ice Cream").Find("Value").GetComponent<TextMeshProUGUI>();
        personPieCount = personPanel.Find("Purchases").Find("Pies").Find("Value").GetComponent<TextMeshProUGUI>();
        personPizzaCount = personPanel.Find("Purchases").Find("Pizzas").Find("Value").GetComponent<TextMeshProUGUI>();
        personSalmonCount = personPanel.Find("Purchases").Find("Salmon").Find("Value").GetComponent<TextMeshProUGUI>();
        personTacoCount = personPanel.Find("Purchases").Find("Tacos").Find("Value").GetComponent<TextMeshProUGUI>();

        companyPanel.gameObject.SetActive(false);
        marketPanel.gameObject.SetActive(false);
        personPanel.gameObject.SetActive(false);
    }

    void Update() {
        UpdateMarketDS();
    }

    public void EnteredMenu() {
        GameManager.EnteredMenu();
        marketButton.interactable = false;
    }
    public void ExitedMenu() {
        GameManager.ExitedMenu();
        marketButton.interactable = true;
    }

    private void UpdateMarketDS() {
        float avgSupplyElasticity = GameManager.GetAverageSupplyElasticity();
        float avgDemandElasticity = GameManager.GetAverageDemandElasticity();
        marketSupplyElasticityText.SetText(avgSupplyElasticity.ToString("F2"));
        marketDemandElasticityText.SetText(avgDemandElasticity.ToString("F2"));
        float[] equilibriumValues = GameManager.GetMarketEquilibrium();
        marketPanelSupplyCurve.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan(avgSupplyElasticity));
        marketPanelDemandCurve.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(avgDemandElasticity));
        marketEquilQText.SetText(Mathf.RoundToInt(equilibriumValues[0]).ToString() + " Food");
        marketEquilPText.SetText("$" + equilibriumValues[1].ToString("F2"));
    }

    // Paramaters: Company Name, Product Name
    public void OpenCompanyPanel(string s, string p) {
        EnteredMenu();
        companyPanel.gameObject.SetActive(true);
        companyPanelTitle.SetText(s);
        float companyElasticity = GameManager.GetCompanyElasticity(s);
        companyPanelElasticityText.SetText(companyElasticity.ToString("F2"));
        companyPanelSupplyCurve.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan(companyElasticity));
        float demandElasticity = GameManager.GetAverageDemandElasticity();
        companyPanelDemandCurve.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(demandElasticity));
        float[] equilibriumValues = GameManager.GetCompanyEquilibrium(s);
        companyPanelEquilQText.SetText(Mathf.RoundToInt(equilibriumValues[0]).ToString() + " " + p);
        companyPanelEquilQTextMC.SetText(companyPanelEquilQText.text);
        companyPanelEquilPText.SetText("$" + equilibriumValues[1].ToString("F2"));
    }

    public void OpenPersonPanel(string personName) {
        EnteredMenu();
        personPanel.gameObject.SetActive(true);
        marginalUtilCompanyIndex = 0;
        personNameText.SetText(personName);
        personCompanyNameText.SetText(GameManager.GetCompanyName(marginalUtilCompanyIndex));
        personPanelElasticityText.SetText(GameManager.GetPersonElasticity(personName).ToString("F2"));
        incomeText.SetText("$" + GameManager.GetConsumerIncome(personName).ToString("F2"));
        disposableIncomeText.SetText("$" + GameManager.GetConsumerDisposableIncome(personName).ToString("F2"));
        List<float> marginalUtilValues = GameManager.GetConsumerMU(GameManager.GetCompanyName(marginalUtilCompanyIndex), personName);
        for(int i=1; i<=17; i++) {
            marginalUtilValueText.Find("Val" + i).GetComponent<TextMeshProUGUI>().SetText(marginalUtilValues[i-1].ToString("F2"));
        }
        int[] goodsPurchased = GameManager.GetConsumerGoodsPurchased(personName);
        personBurgerCount.SetText(goodsPurchased[0].ToString());
        personCoffeeCount.SetText(goodsPurchased[1].ToString());
        personDonutCount.SetText(goodsPurchased[2].ToString());
        personIceCreamCount.SetText(goodsPurchased[3].ToString());
        personPieCount.SetText(goodsPurchased[4].ToString());
        personPizzaCount.SetText(goodsPurchased[5].ToString());
        personSalmonCount.SetText(goodsPurchased[6].ToString());
        personTacoCount.SetText(goodsPurchased[7].ToString());
    }

    public void IncreasePersonPanelCompany() {
        marginalUtilCompanyIndex++;
        if(marginalUtilCompanyIndex >= GameManager.GetCompanyCount()) {
            marginalUtilCompanyIndex = 0;
        }
        personCompanyNameText.SetText(GameManager.GetCompanyName(marginalUtilCompanyIndex));
        List<float> marginalUtilValues = GameManager.GetConsumerMU(GameManager.GetCompanyName(marginalUtilCompanyIndex), personNameText.text);
        for(int i=1; i<=17; i++) {
            marginalUtilValueText.Find("Val" + i).GetComponent<TextMeshProUGUI>().SetText(marginalUtilValues[i-1].ToString("F2"));
        }
    }
    public void DecreasePersonPanelCompany() {
        marginalUtilCompanyIndex--;
        if(marginalUtilCompanyIndex < 0) {
            marginalUtilCompanyIndex = GameManager.GetCompanyCount()-1;
        }
        personCompanyNameText.SetText(GameManager.GetCompanyName(marginalUtilCompanyIndex));
        List<float> marginalUtilValues = GameManager.GetConsumerMU(GameManager.GetCompanyName(marginalUtilCompanyIndex), personNameText.text);
        for(int i=1; i<=17; i++) {
            marginalUtilValueText.Find("Val" + i).GetComponent<TextMeshProUGUI>().SetText(marginalUtilValues[i-1].ToString("F2"));
        }
    }
}
