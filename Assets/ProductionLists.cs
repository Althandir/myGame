using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProductionLists : MonoBehaviour
{
    [Header("List of Products to appear ingame. NO DUPLICATES ALLOWED!")]
    
    static int s_listCounterInitCheck = 0;
    static int s_listCounterDuplicateCheck = 0;
    static List<Product> s_FarmList = new List<Product>();
    static List<Product> s_WoodcutterList = new List<Product>();
    
    [SerializeField]
    List<Product> _FarmList_0 = new List<Product>();
    [SerializeField]
    List<Product> _WoodcutterList_1 = new List<Product>();

    void Awake()
    {
        s_FarmList = _FarmList_0;
        ListCheck(s_FarmList);
        s_WoodcutterList = _WoodcutterList_1;
        ListCheck(s_WoodcutterList);
    }

    static public List<Product> FarmList { get => s_FarmList; }
    static public List<Product> WoodcutterList { get => s_WoodcutterList; }

    static void ListCheck(List<Product> listToCheck)
    {
        DuplicateCheck(listToCheck);
        ProductInitCheck(listToCheck);
    }

    static void DuplicateCheck(List<Product> listToCheck)
    {
        if (listToCheck.Count != listToCheck.Distinct().Count())
        {
            Debug.Log("List No." + s_listCounterDuplicateCheck + " has no duplicates.");
        }
        else
        {
            Debug.LogWarning("List No." + s_listCounterDuplicateCheck + " has duplicates. Please check the List in GameController");
        }
        s_listCounterDuplicateCheck += 1;
    }

    static void ProductInitCheck(List<Product> listToCheck)
    {
        foreach (Product product in listToCheck)
        {
            if (product.GetMinPrice() != 0 && 
                product.GetMaxPrice() != 0 && 
                product.GetName() != string.Empty &&
                product.Icon != null && 
                product.GetNeededTicks() != 0)
            {
                Debug.Log(product.GetName() + " was initialized.");
            }
            else
            {
                Debug.LogWarning("List No. " + 
                    s_listCounterInitCheck + 
                    ".Product No. " + 
                    listToCheck.IndexOf(product) + 
                    " has errors! Please check ScriptableObject in GameController/Products.");
            }
            s_listCounterInitCheck += 1;
        }
    }

    public static List<Product> GetProductList(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Farm:
                {
                    return FarmList;
                }
            case BuildingType.Woodcutter:
                {
                    return WoodcutterList;
                }
            case BuildingType.Storage:
            case BuildingType.Undefined:
            default:
                {
                    return null;
                }
        }     
    }
}
