using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager {
    // Producer data
    private static string[] companyNames = {"Burger Hut", "Morning Coffee", "Tasty Donuts", "Melting Ice Cream", "Pi", "Pi-zza", "Mmm.. Salmon", "Taco Takedown"};
    private static Dictionary<string, float> companySupplyElasticity;
    private static Dictionary<string, Vector2> companyLocations;
    private static Dictionary<string, float> companySupplyPrice0;

    // Consumer data
    private static string[] possiblePersonNamesMale = {"Jim", "Tom", "Zach", "Sam", "Ethan", "Robert", "Richard", "Kyle", "Steve", "Jack", "Chris"};
    private static string[] possiblePersonNamesFemale = {"Jenna", "Sarah", "Caroline", "Beth", "Jessica", "Rebecca", "Kylie", "Carol", "Helen", "Samantha"};
    private static string[] possiblePersonLastNames = {"Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "White", "Harris", "Clark", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Scott", "Hill", "Adams"};
    private static int personCount = 15;
    private static List<string> personNames;
    private static float[] personDemandElasticity;
    private static float[] personDemandPrice0;
    // Each marginal utility is a parabula curve (-ax^2+2.5x+c) => float[] = {-a, 2.5, c}
    private static Dictionary<string, List<float[]>> consumerMU;
    private static float[] personIncome;
    private static float[] personDisposableIncome;
    // Goods purchased in each company per person for int[]
    private static Dictionary<string, int[]> personGoodsPurchased;
    
    private static bool mouseOnPerson;

    private static bool isInMenu;

    private static bool isGamePaused;

    private static SoundMananger soundMananger;

    public static void ResetGame() {
        soundMananger = GameObject.Find("SoundManager").GetComponent<SoundMananger>();
        personNames = new List<string>();

        // Set company supply elasticity
        companySupplyElasticity = new Dictionary<string, float>();
        companyLocations = new Dictionary<string, Vector2>();
        companySupplyPrice0 = new Dictionary<string, float>();
        foreach(string s in companyNames) {
            companySupplyElasticity.Add(s, Random.Range(0f, 2.5f));
            companyLocations.Add(s, GameObject.Find("Buildings").transform.Find(s).position);
            companySupplyPrice0.Add(s, 0);
        }

        // Set person demand elasticity
        personDemandElasticity = new float[personCount];
        personDemandPrice0 = new float[personCount];
        for(int i=0; i < personCount; i++) {
            personDemandElasticity[i] = Random.Range(0f, 2.5f);
            personDemandPrice0[i] = Random.Range(75f, 100f);
        }

        // Create person marginal utility for each company
        consumerMU = new Dictionary<string, List<float[]>>();
        personGoodsPurchased = new Dictionary<string, int[]>();
        foreach(string s in companyNames) {
            List<float[]> listMU = new List<float[]>();
            int[] tempGoodsPurchased = new int[personCount];
            for(int i=0; i<personCount; i++) {
                float tempElasticity = personDemandElasticity[i];
                listMU.Add(new float[]{-Mathf.Lerp(.15f, 1.25f, (2.5f - tempElasticity) / 2.5f), Random.Range(4.5f, 5.5f), Random.Range(0f, 10f)});
                tempGoodsPurchased[i] = 0;
            }
            consumerMU.Add(s, listMU);
            personGoodsPurchased.Add(s, tempGoodsPurchased);
        }

        // Create person income
        personIncome = new float[personCount];
        personDisposableIncome = new float[personCount];
        for(int i=0; i<personCount; i++) {
            personIncome[i] = personDisposableIncome[i] = Random.Range(250f, 1000f);
        }
    }

    public static string GetCompanyName(int idx) {
        return companyNames[idx];
    }
    public static int GetCompanyIndex(string companyName) {
        int idx;
        for(idx=0; idx<companyNames.Length; idx++) {
            if(companyNames[idx] == companyName) {
                return idx;
            }
        }
        return -1;
    }
    public static int GetCompanyCount() {
        return companyNames.Length;
    }

    // Returns the average/market supply elasticity
    public static float GetAverageSupplyElasticity() {
        float elasticitySum = 0;
        foreach(string s in companyNames) {
            elasticitySum += companySupplyElasticity[s];
        }
        return elasticitySum / companyNames.Length;
    }

    // Returns the average/market demand elasticity
    public static float GetAverageDemandElasticity() {
        float elasticitySum = 0;
        for(int i=0; i<personCount; i++) {
            elasticitySum += personDemandElasticity[i];
        }
        return elasticitySum / personCount;
    }

    // Returns the average/market demand price at 0 quantity
    public static float GetAverageDemandPrice0() {
        float priceSum = 0;
        for(int i=0; i<personCount; i++) {
            priceSum += personDemandPrice0[i];
        }
        return priceSum / personCount;
    }

    // Returns the average/market supply price at 0 quantity
    public static float GetAverageSupplyPrice0() {
        float priceSum = 0;
        foreach(string s in companyNames) {
            priceSum += companySupplyPrice0[s];
        }
        return priceSum / companyNames.Length;
    }

    // Returns singular supply elasticity
    public static float GetCompanyElasticity(string s) {
        return companySupplyElasticity[s];
    }

    // Returns singular consumer elasticity
    public static float GetPersonElasticity(string s) {
        return personDemandElasticity[personNames.IndexOf(s)];
    }

    // Returns singular firm price at quantity 0
    public static float GetCompanyPrice0(string s) {
        return companySupplyPrice0[s];
    }

    // Returns singular firm's equilibrium quantity and price
    public static float[] GetCompanyEquilibrium(string s) {
        float companyElasticity = GetCompanyElasticity(s);
        float initialCompanyPrice = GetCompanyPrice0(s);
        float demandElasticity = GetAverageDemandElasticity();
        float initialDemandPrice = GetAverageDemandPrice0();
        float equilibriumQuantity = Mathf.RoundToInt((initialCompanyPrice - initialDemandPrice) / (-demandElasticity - companyElasticity));
        float equilibriumPrice = equilibriumQuantity * companyElasticity;
        return new float[]{equilibriumQuantity, equilibriumPrice};
    }

    public static float[] GetMarketEquilibrium() {
        float supplyElasticity = GetAverageSupplyElasticity();
        float demandElasticity = GetAverageDemandElasticity();
        float initialDemandPrice = GetAverageDemandPrice0();
        float initialSupplyPrice = GetAverageSupplyPrice0();
        float equilibriumQuantity = Mathf.RoundToInt((initialSupplyPrice - initialDemandPrice) / (-demandElasticity - supplyElasticity));
        float equilibriumPrice = equilibriumQuantity * supplyElasticity;
        return new float[]{equilibriumQuantity, equilibriumPrice};
    }

    // Returns random firm world space position and name
    public static void GetRandomFirmPos(out string returnCompanyName, out Vector2 returnCompanyPos) {
        returnCompanyName = companyNames[Random.Range(0, companyNames.Length)];
        returnCompanyPos = companyLocations[returnCompanyName];
    }
    public static Vector2 GetFirmPos(string companyName) {
        return companyLocations[companyName];
    }

    public static int GetPersonCount() {
        return personCount;
    }

    public static void EnteredMenu() {
        isInMenu = true;
    }
    public static void ExitedMenu() {
        isInMenu = false;
    }
    public static bool IsInMenu() {
        return isInMenu;
    }

    public static string GetRandomPersonName(bool isMale) {
        string fullName = "";
        if(isMale) {
            fullName += possiblePersonNamesMale[Random.Range(0, possiblePersonNamesMale.Length)];
        } else {
            fullName += possiblePersonNamesFemale[Random.Range(0, possiblePersonNamesFemale.Length)];
        }
        fullName += " " + possiblePersonLastNames[Random.Range(0, possiblePersonLastNames.Length)];
        personNames.Add(fullName);
        return fullName;
    }
    public static string[] GetAllPersonNames() {
        string[] allPossiblePersonNames = new string[possiblePersonNamesMale.Length + possiblePersonNamesFemale.Length];
        possiblePersonNamesMale.CopyTo(allPossiblePersonNames, 0);
        possiblePersonNamesFemale.CopyTo(allPossiblePersonNames, possiblePersonNamesMale.Length);
        return allPossiblePersonNames;
    }

    // Get marginal utility list of a person for a specific company
    public static List<float> GetConsumerMU(string companyName, string personName) {
        int personIndex = personNames.IndexOf(personName);
        float[] varsMU = consumerMU[companyName][personIndex];
        List<float> valuesMU = new List<float>();
        for(int i=1; i<=17; i++) {
            valuesMU.Add(varsMU[0] * Mathf.Pow(i, 2) + varsMU[1] * i + varsMU[2]);
        }
        return valuesMU;
    }

    public static float GetConsumerDisposableIncome(string personName) {
        int personIndex = personNames.IndexOf(personName);
        return personDisposableIncome[personIndex];
    }
    public static float GetConsumerIncome(string personName) {
        int personIndex = personNames.IndexOf(personName);
        return personIncome[personIndex];
    }

    public static int[] GetConsumerGoodsPurchased(string personName) {
        int personIndex = personNames.IndexOf(personName);
        int[] goodsPurchased = new int[personCount];
        int i = 0;
        foreach(string tempCompany in companyNames) {
            goodsPurchased[i] = personGoodsPurchased[tempCompany][personIndex];
            i++;
        }
        return goodsPurchased;
    }

    // Returns the company name which has the highest marginal utility per dollar spent for the consumer
    public static string GetCompanyHighestMUD(string personName) {
        float income = GetConsumerDisposableIncome(personName);
        int personIndex = personNames.IndexOf(personName);
        int[] goodsPurchased = GetConsumerGoodsPurchased(personName);
        string bestCompany = null;
        float bestMUD = 0f;
        foreach(string tempCompany in companyNames) {
            int companyIndex = GetCompanyIndex(tempCompany);
            float productPrice = GetCompanyEquilibrium(tempCompany)[1];
            float marginalUtilDol = GetConsumerMU(tempCompany, personName)[goodsPurchased[companyIndex]] / productPrice;
            if(marginalUtilDol > bestMUD && income > productPrice) {
                bestMUD = marginalUtilDol;
                bestCompany = tempCompany;
            }
        }
        return bestCompany;
    }

    public static void PurchaseGood(string personName, string companyName) {
        int personIndex = personNames.IndexOf(personName);
        float productPrice = GetCompanyEquilibrium(companyName)[1];
        personDisposableIncome[personIndex] -= productPrice;
        personGoodsPurchased[companyName][personIndex]++;
    }

    public static void SetMouseOnPerson(bool setVal) {
        mouseOnPerson = setVal;
    }
    public static bool GetMouseOnPerson() {
        return mouseOnPerson;
    }

    public static void PauseResumeGame() {
        if(isGamePaused) {
            isGamePaused = false;
            soundMananger.ResumeAudio();
        } else {
            isGamePaused = true;
            soundMananger.PauseAudio();
        }
    }
    public static bool IsGamePaused() {
        return isGamePaused;
    }
}
