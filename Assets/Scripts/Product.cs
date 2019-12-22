using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Product : ScriptableObject
{
    [Header("Inital Values :: Do NOT change unnecessary")]
    [SerializeField] byte _InitMinPrice = 0;
    [SerializeField] byte _InitMaxPrice = 0;
    [SerializeField] string _productName = null;
    [SerializeField] Sprite _icon = null;
    [SerializeField] float _InitNeededProductionTime = 0.0f;

    [Header("Actual Values :: Changed during runtime")]
    [SerializeField] byte _MinPrice = 0;
    [SerializeField] byte _MaxPrice = 0;
    [SerializeField] float _NeededProductionTime = 0.0f;
    public Sprite Icon { get => _icon; }
    public float NeededProductionTime { get => _NeededProductionTime; }
    public string Name { get => _productName; }
    public byte InitMinPrice { get => _InitMinPrice;  }
    public byte InitMaxPrice { get => _InitMaxPrice;  }

    public byte MinPrice { get => _MinPrice; }
    public byte MaxPrice { get => _MaxPrice; }

    public void OnEnable()
    {
        if (_InitMaxPrice == 0)
        {
            _InitMaxPrice = (byte)(_InitMinPrice * 3);
        }

        ProductInitCheck(this);

        _MinPrice = _InitMinPrice;
        _MaxPrice = _InitMaxPrice;
        _NeededProductionTime = _InitNeededProductionTime;
    }

    static void ProductInitCheck(Product product)
    {
        if (product.InitMinPrice != 0 &&
            product.InitMaxPrice != 0 &&
            product.Name != string.Empty &&
            product.Icon != null &&
            product.NeededProductionTime != 0.0f)
        {
            Debug.Log("Product was initialized.");
        }
        else
        {
            Debug.LogWarning(product.name + " has errors! Please check ScriptableObject in GameController/Products.");
        }
    }


    //Debugfunction
    public string PrintValues()
    {
        return ("Name: " + Name + " min/maxPrice: " + InitMinPrice + "/" + InitMaxPrice);
    }

}
