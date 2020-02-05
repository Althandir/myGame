using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductRequirement
{
    [SerializeField] Product _Product;
    [SerializeField] int _Amount;

    public Product Product 
    { 
        get => _Product;
        set 
        {
            _Product = value;
        } 
    }
    public int Amount { get => _Amount; }
}
 