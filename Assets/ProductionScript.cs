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

    void Awake()
    {
        // Link to UI                                           Building    Camera      Screen
        ProductionScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(1).gameObject;
        // Diable UI
        ProductionScreen.SetActive(false);
        // GetProductionType
        buildingType = transform.parent.GetComponent<Building>().GetBuildingType();
        InitProduction();
    }

    public void InitProduction()
    {
        // Checks if Building is a Storage: if so disable ProductionSystem
        if (buildingType == BuildingType.Storage)
        {
            gameObject.SetActive(false);
        }
        else if (buildingType != BuildingType.Undefined)
        {
            // TODO : Produce Goods
            Produce();
        }
        else // Undefined!
        {
            Debug.LogWarning("Invalid BuildingType for " + transform.parent.name);
        }
    }

    void Produce()
    {
        switch (buildingType)
        {
            case BuildingType.Undefined:
                {
                    break;
                }
            case BuildingType.Storage:
                {
                    break;
                }
            case BuildingType.Farm:
                {
                    break;
                }
            case BuildingType.Woodcutter:
                {
                    break;
                }
            default:
                {
                    Debug.LogWarning("Something went wrong During Production " + transform.parent.name);
                    break;
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
        if (!ProductionScreen.activeSelf)
            ProductionScreen.SetActive(true);

        // Testfunctions
    }

}
