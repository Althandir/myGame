using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Interactable))]

public class ProductionScript : MonoBehaviour
{
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
    Transform UI_ProductionContent = null;
    [SerializeField]
    int numWorker;
    [SerializeField]
    float paymentTimer;

    public int NumWorker { get => numWorker; set => numWorker = value; }

    void Awake()
    {
        // Link to UI                                Building    Camera      Screen
        ProductionScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(1);
        UI_ProductionContent = ProductionScreen.GetChild(3).GetChild(0);
        // Resets ScrollView Content
        ResetScrollViewUI();
        // Disable UI
        ProductionScreen.gameObject.SetActive(false);
        // init numWorker & paymentTimer as 0
        numWorker = 0;
        paymentTimer = 0.0f;
    }

    #region UI_Creation&Linking
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
        ResetScrollViewUI();

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
            // Link to UI
            if (!UI_ProductSlotPrefab)
            {
                Debug.LogError("Could not assign UI_Elements for " + transform.parent.name);
            }
            else
            {
                // Creates one clickable Icon for each Product in the List
                foreach (Product product in productList)
                {
                    string name = "ProductSlot (";

                    // Increase size of Background
                    RectTransform ProdContent_rt = UI_ProductionContent.GetComponent<RectTransform>();
                    ProdContent_rt.sizeDelta = new Vector2(ProdContent_rt.sizeDelta.x, ProdContent_rt.sizeDelta.y + 100);

                    // Create Production Slot
                    GameObject UI_ProductSlot = Instantiate(UI_ProductSlotPrefab, UI_ProductionContent);
                    UI_ProductSlot.name = name + productList.IndexOf(product) + ")";
                    UI_ProductSlot.GetComponent<ProductionSlot>().ProductionReference = this;

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
                }
            }
        }
    }
    #endregion

    private void ResetScrollViewUI()
    {
        // Checks for Children and destorys them! - Anakin Skywalker Style
        if (UI_ProductionContent.childCount > 0)
        {
            foreach (Transform child in UI_ProductionContent)
            {
                Debug.Log("Destroying " + child.name);
                Destroy(child.gameObject);
            }
        }
        // Resets Scrollview 
        UI_ProductionContent.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 20);
    }

    public void OnClick()
    {
        // Display Production.GUI
        if (!ProductionScreen.gameObject.activeSelf)
            ProductionScreen.gameObject.SetActive(true);
    }

}
