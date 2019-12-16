using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Product : ScriptableObject
{
    [Header("Do NOT change unnecessary")]
    [SerializeField] byte _minPrice = 0;
    [SerializeField] byte _maxPrice = 0;
    [SerializeField] string _productName = null;
    [SerializeField] Sprite _icon = null;
    [SerializeField] float _neededProductionTime = 0.0f;

    public Sprite Icon { get => _icon; set => _icon = value; }
    public float NeededProductionTime { get => _neededProductionTime; set => _neededProductionTime = value; }
    public string Name { get => _productName; }
    public byte InitMinPrice { get => _minPrice; set => _minPrice = value; }
    public byte InitMaxPrice { get => _maxPrice; set => _maxPrice = value; }

    //Debugfunction
    public string PrintValues()
    {
        return ("Name: " + Name + " min/maxPrice: " + InitMinPrice + "/" + InitMaxPrice);
    }

}
