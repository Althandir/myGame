using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nessesary for Array idk why + to be visible in inspector
[CreateAssetMenu]
public class Product : ScriptableObject
{
    [Header("Do NOT change unnecessary")]
    [SerializeField] byte initMinPrice = 0;
    [SerializeField] byte initMaxPrice = 0;
    [SerializeField] string productName = null;
    [SerializeField] Sprite icon = null;
    [SerializeField] byte neededTicks = 0;

    // learn how 2 do this 
    public Sprite Icon { get => icon; set => icon = value; }

    public string GetName()
    {
        return productName;
    }

    public byte GetMinPrice()
    {
        return initMinPrice;
    }

    public byte GetMaxPrice()
    {
        return initMaxPrice;
    }

    public byte GetNeededTicks ()
    {
        return neededTicks;
    }

    //Debugfunction
    public string PrintValues()
    {
        return ("Name: " + productName + " min/maxPrice: " + initMinPrice + "/" + initMaxPrice);
    }

}
