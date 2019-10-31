using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class ProductionScript : MonoBehaviour
{
    [Header("DEBUG_VALUES:")]
    [SerializeField]
    Transform ProductionScreen;
    [SerializeField]
    BuildingType buildingType;
    [SerializeField]
    List<Product> productList;
    [SerializeField]
    GameObject UI_ProductSlotPrefab = null;
    [SerializeField]
    Transform UI_ProductionContent = null;

    void Awake()
    {
        // Link to UI                                Building    Camera      Screen
        ProductionScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(1);
        UI_ProductionContent = ProductionScreen.GetChild(3).GetChild(0);
        // Resets ScrollView Content
        UI_ProductionContent.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 20);
        // Disable UI
        ProductionScreen.gameObject.SetActive(false);
        // GetProductionType
        buildingType = transform.parent.GetComponent<Building>().GetBuildingType();
    }

    public void UpdateBuildingType(BuildingType buildingType)
    {
        this.buildingType = buildingType;
    }

    public void UpdateProductionList()
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

        buildingType = transform.parent.GetComponent<Building>().GetBuildingType();
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
                foreach (Product product in productList)
                {
                    string name = "ProductSlot (";
                    int i = 0;

                    // Increase size of Background
                    RectTransform ProdContent_rt = UI_ProductionContent.GetComponent<RectTransform>();
                    ProdContent_rt.sizeDelta = new Vector2(ProdContent_rt.sizeDelta.x, ProdContent_rt.sizeDelta.y + 100);

                    GameObject UI_ProductSlot = Instantiate(UI_ProductSlotPrefab, UI_ProductionContent);
                    UI_ProductSlot.name = name + i + ")";
                    // Position Icon on left side
                    RectTransform ProdSlot_rt = UI_ProductSlot.GetComponent<RectTransform>();
                    ProdSlot_rt.anchoredPosition = new Vector2(60, (-100) * (i) - 60);
                }
            }
        }
    }

    IEnumerator GetNextTick()
    {
        // Checks for next Tick every 0.5 sec

        yield return new WaitForSeconds(0.5f);
    }
    

    public void OnClick()
    {
        // Display Production.GUI
        if (!ProductionScreen.gameObject.activeSelf)
            ProductionScreen.gameObject.SetActive(true);

        // Testfunctions
        UpdateProductionList();
    }

}
