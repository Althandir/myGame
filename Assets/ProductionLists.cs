using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProductionLists
{
    static bool isInit = false;
    static Transform ProductsRef = null;
    static List<Product> _FarmList;
    static List<Product> _WoodcutterList;

    public static List<Product> FarmList { get => _FarmList; }
    public static List<Product> WoodcutterList { get => _WoodcutterList; }

    public static void CreateProductionLists()
    {
        if (!isInit)
        {
            ProductsRef = GameObject.FindGameObjectWithTag("GameController").transform.GetChild(0);
            InitFarmList();
            InitWoodcutterList();
        }
        isInit = true;
    }

    public static List<Product> GetProductList(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Farm:
                {
                    return ProductionLists.FarmList;

                }
            case BuildingType.Woodcutter:
                {
                    return ProductionLists.WoodcutterList;
                }
            case BuildingType.Storage:
            case BuildingType.Undefined:
            default:
                {
                    return null;
                }
        }
    }

    static void InitFarmList()
    {
        _FarmList = new List<Product>
        {
            GetProductID(0)
        };
    }

    static void InitWoodcutterList()
    {
        _WoodcutterList = new List<Product>
        {

        };
    }

    static Product GetProductID(int id)
    {
        return ProductsRef.GetChild(id).GetComponent<Product>();
    }
}
