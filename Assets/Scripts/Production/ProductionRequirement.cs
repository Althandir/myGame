using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductionRequirement
{
    [SerializeField] ProductRequirement _ProductRequirement;
    [SerializeField] bool isFullfilled;

    public ProductionRequirement(ProductRequirement productRequirement)
    {
        _ProductRequirement = productRequirement;
    }

    /// <summary>
    /// Returns the Product in <see cref="_ProductRequirement"/>
    /// </summary>
    /// <returns></returns>
    public Product GetProduct()
    {
        return _ProductRequirement.Product;
    }
    /// <summary>
    /// Returns the required Amount in <see cref="_ProductRequirement"/>
    /// </summary>
    /// <returns></returns>
    public int GetRequiredAmount()
    {
        return _ProductRequirement.Amount;
    }

    public bool IsFullfilled { get => isFullfilled; set => isFullfilled = value; }
    
}
