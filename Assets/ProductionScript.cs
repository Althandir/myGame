using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class ProductionScript : MonoBehaviour
{
    [Header("DEBUG_VALUES:")]
    [SerializeField]
    GameObject ProductionScreen;
    [SerializeField]
    BuildingType buildingType;
    [SerializeField]
    List<Product> productList;
    void Awake()
    {
        // Link to UI                                           Building    Camera      Screen
        ProductionScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(1).gameObject;
        // Diable UI
        ProductionScreen.SetActive(false);
        // GetProductionType
        buildingType = transform.parent.GetComponent<Building>().GetBuildingType();
        // TODO Init when Type is set
        // InitProduction();
    }

    public void InitProduction()
    {
        productList = ProductionLists.GetProductList(buildingType);
        if (productList == null && buildingType == BuildingType.Storage)
        {
            gameObject.SetActive(false);
        }
        else if (productList == null && buildingType != BuildingType.Storage)
        {
            Debug.LogWarning(transform.parent.name + " Production has Problems in ProductionScript! Please Check Type!" );
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
        if (!ProductionScreen.activeSelf)
            ProductionScreen.SetActive(true);

        // Testfunctions
        InitProduction();
    }

}
