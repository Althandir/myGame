using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Interactable))]

public class ProductionScript : MonoBehaviour
{
    const byte initWorkerCost = 100;
    const byte maxWorkerNum = 99;
    const byte minWorkerNum = 0;

    [Header("Reference to the Prefab of the ProductionSlot")]
    [SerializeField]
    GameObject UI_ProductSlotPrefab = null;
    
    [Header("DEBUG_VALUES:")]
    [SerializeField]
    Transform ProductionScreen;
    [SerializeField]
    BuildingType buildingType;
    [SerializeField]
    List<Product> productList;
    [SerializeField]
    byte numOverallWorker;
    [SerializeField]
    byte numAvailableWorker;
    [SerializeField]
    uint newWorkerCost;
    [SerializeField]
    float salaryTimer;
    [SerializeField]
    Transform UI_ProductionContent = null;
    [SerializeField]
    Transform UI_WorkerPanel = null;
    [SerializeField]
    List<GameObject> ProductionSlots;

    

    void Awake()
    {
        // Link to UI                                Building    Camera      Screen
        ProductionScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(1);
        UI_ProductionContent = ProductionScreen.GetChild(3).GetChild(0);
        UI_WorkerPanel = ProductionScreen.GetChild(4);

        ProductionSlots = new List<GameObject>();

        ResetProduction();
        
    }

    void Update()
    {
        salaryTimer += Time.deltaTime;
    }
    #region ResetFunction
    public void ResetProduction()
    {
        numOverallWorker = 0;
        numAvailableWorker = 0;
        salaryTimer = 0.0f;
        productList = null;
        newWorkerCost = initWorkerCost;
        ProductionSlots.Clear();
        UI_ResetScrollView();
        UI_UpdateOverallWorkerNumber();
        UI_UpdateWorkerPrice();

        // Disable UI
        ProductionScreen.gameObject.SetActive(false);
    }
    #endregion

    #region Calculation for WorkerCosts

    void CalcNewWorkerCost()
    {
        // log(aX^2) + bx + c  
        // log Limits the maximum Number while x^2 causes a high rise in costs
        newWorkerCost = (uint)(Mathf.Log(newWorkerCost * numOverallWorker ^ 2) + initWorkerCost * numOverallWorker + newWorkerCost);
        UI_UpdateWorkerPrice();
    }

    #endregion

    #region Functions for Worker
    
    #region Overall
    public void IncOverallWorker()
    {
        // max 99
        if (numOverallWorker <= maxWorkerNum)
        {
            numOverallWorker += 1;
            numAvailableWorker += 1;
            UI_UpdateOverallWorkerNumber();
            CalcNewWorkerCost();
        }
        else if (numOverallWorker == maxWorkerNum)
        {
            Debug.LogWarning("Max amount of Worker reached!");
        }
    }

    public void DecOverallWorker()
    {
        // min 0
        if (numOverallWorker >= minWorkerNum && numAvailableWorker != 0)
        {
            numOverallWorker -= 1;
            numAvailableWorker -= 1;
            UI_UpdateOverallWorkerNumber();
            
        } else if (numOverallWorker == minWorkerNum)
        {
            Debug.LogWarning("Min amount of Worker reached!");
        }
        else if (numAvailableWorker == 0)
        {
            Debug.LogWarning("No unassigned Worker to fire!");
        }
    }
    // Get Property
    public byte NumOverallWorker { get => numOverallWorker; }
    
    #endregion

    #region Available
    public void IncAvailableWorker()
    {
        numAvailableWorker += 1;
        UI_UpdateOverallWorkerNumber();
    }

    public void DecAvailableWorker()
    {
        numAvailableWorker -= 1;
        UI_UpdateOverallWorkerNumber();
    }
    // Get Property
    public byte NumAvailableWorker { get => numAvailableWorker; }
    #endregion

    #endregion

    #region UpdateOf BuildingType & ProductionLists
    /// <summary>
    /// Whenever BuildingType changes the nessesary productionList is getting loaded.
    /// </summary>
    /// <param name="buildingType"></param>
    public void UpdateBuildingType(BuildingType buildingType)
    {
        this.buildingType = buildingType;
        UpdateProductionList();
    }

    public void UpdateProductionList()
    {
        UI_ResetScrollView();

        productList = ProductionLists.GetProductList(buildingType);
        if (productList == null && buildingType == BuildingType.Storage)
        {
            gameObject.SetActive(false);
        }
        else if (productList == null && buildingType != BuildingType.Storage)
        {
            Debug.LogWarning(transform.parent.name + " Production has Problems in ProductionScript! Please Check Type!" );
        }
        else if (productList != null)
        {
            // Spawn ProductionSlot & Linking to UI
            UI_CreateAllProductionSlots();
        }
    }
    #endregion

    #region UI_Functions
    private void UI_ResetScrollView()
    {
        // Checks for Children and destorys them! - Anakin Skywalker Style
        if (UI_ProductionContent.childCount > 0)
        {
            foreach (Transform child in UI_ProductionContent)
            {
                Destroy(child.gameObject);
            }
        }
        // Resets Scrollview 
        UI_ProductionContent.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 20);
    }

    private void UI_UpdateOverallWorkerNumber()
    {
        UI_WorkerPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = numOverallWorker.ToString() + "\n" + numAvailableWorker.ToString();
    }

    private void UI_UpdateWorkerPrice()
    {
        UI_WorkerPanel.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = newWorkerCost.ToString();
    }

    private void UI_CreateAllProductionSlots()
    {
        if (!UI_ProductSlotPrefab)
        {
            Debug.LogError("Could not assign UI_Elements for " + transform.parent.name);
        }
        else
        {
            // Creates one clickable Icon for each Product in the List
            foreach (Product product in productList)
            {
                // string name = "ProductSlot (";
                string name = "ProductSlot";
                // Increase size of Background
                RectTransform ProdContent_rt = UI_ProductionContent.GetComponent<RectTransform>();
                ProdContent_rt.sizeDelta = new Vector2(ProdContent_rt.sizeDelta.x, ProdContent_rt.sizeDelta.y + 100);

                // Create Production Slot
                GameObject UI_ProductSlot = Instantiate(UI_ProductSlotPrefab, UI_ProductionContent);
                //UI_ProductSlot.name = name + productList.IndexOf(product) + ")";
                UI_ProductSlot.name = name;
                UI_ProductSlot.GetComponent<UI_ProductionSlot>().ProductionReference = this;

                // Position Icon on left side
                RectTransform ProdSlot_rt = UI_ProductSlot.GetComponent<RectTransform>();
                ProdSlot_rt.anchoredPosition = new Vector2(60, (-100) * (productList.IndexOf(product)) - 60);

                // Sets Icon of the ProductSlot
                UI_ProductSlot.transform.GetChild(0).GetComponent<Image>().sprite = product.Icon;
                // Sets maxNeededTicks
                UI_ProductSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0|" + product.NeededProductionTime;
                // Sets Slider 
                Slider slider = UI_ProductSlot.transform.GetChild(2).GetComponent<Slider>();
                slider.value = 0;
                slider.maxValue = product.NeededProductionTime;
                slider.minValue = 0;

                // Adds ProductSlot into List<ProductionSlot> to be referenced
                ProductionSlots.Add(UI_ProductSlot);
            }
        }
    }
    #endregion



    public void OnClick()
    {
        // Display Production.GUI
        if (!ProductionScreen.gameObject.activeSelf)
            ProductionScreen.gameObject.SetActive(true);
    }

}
