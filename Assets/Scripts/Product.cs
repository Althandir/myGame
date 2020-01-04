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

    [Header("Requirement Settings for this Product.")]
    [SerializeField] Product[] _RequiredProducts;
    [SerializeField] byte[] _RequiredAmount;

    public Sprite Icon { get => _icon; }
    public float NeededProductionTime { get => _NeededProductionTime; }
    public string Name { get => _productName; }
    public byte InitMinPrice { get => _InitMinPrice;  }
    public byte InitMaxPrice { get => _InitMaxPrice;  }

    public byte MinPrice { get => _MinPrice; }
    public byte MaxPrice { get => _MaxPrice; }
    public Product[] RequiredProducts { get => _RequiredProducts; }
    public byte[] RequiredAmount { get => _RequiredAmount; }

    public void OnEnable()
    {
        if (_InitMaxPrice == 0)
        {
            _InitMaxPrice = (byte)(_InitMinPrice * 3);
        }

        ProductCheck(this);

        _MinPrice = _InitMinPrice;
        _MaxPrice = _InitMaxPrice;
        _NeededProductionTime = _InitNeededProductionTime;
    }

    static void ProductCheck(Product product)
    {
        string genError = " has errors! Please check ScriptableObject in GameController/Products.";
        if (!ProductInitCheck(product))
        {
            Debug.LogWarning(product.name + genError);
        }
        if (!ProductRequirementCheck(product))
        {
            Debug.LogWarning(product.name + genError);
        }
    }


    /// <summary>
    /// Checks the general values of a Product. Returns true if ok.
    /// </summary>
    /// <param name="product"></param>
    static bool ProductInitCheck(Product product)
    {
        if (product.InitMinPrice != 0 &&
            product.InitMaxPrice != 0 &&
            product.Name != string.Empty &&
            product.Icon != null &&
            product.NeededProductionTime != 0.0f)
        {
            Debug.Log("Product: Initial check ok!");
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Checks the RequirementLists to be equally long & amounts being > 0. 
    /// Returns true if ok.
    /// </summary>
    /// <param name="product"></param>
    static bool ProductRequirementCheck(Product product)
    {
        if (RequiredProductLenghtCheck(product) && 
            RequiredProductCheck(product) && 
            RequiredAmountCheck(product))
        {
            Debug.Log("Product: Requirement Check ok!");
            return true;
        }
        else
        {
            return false;
        }
    }

    static bool RequiredProductLenghtCheck(Product product)
    {
        if (!(product.RequiredAmount.Length == product.RequiredProducts.Length))
        {
            Debug.LogError(product.name + " has different Requirementslist lenght!");
            return false;
        }
        return true;
    }

    static bool RequiredProductCheck(Product product)
    {
        foreach (Product requirement in product.RequiredProducts)
        {
            if (requirement == null)
            {
                Debug.LogError(product.name + " has missing requirement!");
                return false;
            }
        }
        return true;
    }

    static bool RequiredAmountCheck(Product product)
    {
        foreach (var number in product.RequiredAmount)
        {
            if (number <= 0)
            {
                Debug.LogError(product.name + " has wrong required amount!");
                return false;
            }
        }
        return true;
    }

    //Debugfunction
    public string PrintValues()
    {
        return ("Name: " + Name + " min/maxPrice: " + InitMinPrice + "/" + InitMaxPrice);
    }

}
