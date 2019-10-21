using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nessesary for Array idk why + to be visible in inspector
[System.Serializable]
public class Product : MonoBehaviour
{
    [Header("Do NOT change unnecessary")]
    [SerializeField] byte initMinPrice = 0;
    [SerializeField] byte initMaxPrice = 0;
    [SerializeField] string prodName = null;
    [SerializeField] Sprite icon = null;

    void Awake()
    {
        if (initMinPrice != 0 && initMaxPrice != 0 && prodName != null & icon != null)
        {
            Debug.Log(prodName + " was initialized.");
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " has errors! Please check GameObject in Global/Products.");
        }
    }
    // learn how 2 do this 
    public Sprite Icon { get => icon; set => icon = value; }

    public string GetName()
    {
        return prodName;
    }

    public byte GetMinPrice()
    {
        return initMinPrice;
    }

    public byte GetMaxPrice()
    {
        return initMaxPrice;
    }

    //Debugfunction
    public string PrintValues()
    {
        return ("Name: " + prodName + " min/maxPrice: " + initMinPrice + "/" + initMaxPrice);
    }

}
