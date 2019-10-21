﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Nessesary for Array idk why + to be visible in inspector
[System.Serializable]
class StorageSlot
{
    //staticValues
    static readonly byte s_maxAmount = 100;
    static readonly byte s_minAmount = 0;

    [SerializeField]
    GameObject SlotNum_UIReference = null;
    [SerializeField]
    Image UI_SlotIcon = null;
    [SerializeField]
    TextMeshProUGUI UI_SlotText = null;
    [SerializeField]
    UIStorageSlider UI_Slider = null;
    [SerializeField]
    Product product = null;
    [SerializeField]
    byte amount = 0;

    public Product Product { get => product; set => product = value; }
    public byte Amount { get => amount; set => amount = value; }

    // create reference to UI & connect to specific Icon & disable Icon on Init
    public StorageSlot(GameObject SlotReference)
    {
        SlotNum_UIReference = SlotReference;
        UI_SlotIcon = SlotNum_UIReference.transform.GetChild(0).GetComponent<Image>();
        UI_SlotText = SlotNum_UIReference.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        UI_Slider = SlotNum_UIReference.transform.GetChild(2).GetComponent<UIStorageSlider>();

        UI_SlotIcon.enabled = false;
        UI_SlotText.enabled = false;
        //UI_Slider.enabled = false;
    }
    
    public void SetProduct(Product product, byte amount)
    {
        this.product = product;
        this.amount = amount;
        _UpdateSlot();
    }

    public bool AddProduct()
    {
        if (amount >= s_maxAmount )
        {
            amount = s_maxAmount;
//            Debug.Log("Max Amount reached!");
            this._UpdateSlot();
            return false;
        } else if (amount <= s_minAmount)
        {
            amount = s_minAmount;
//            Debug.Log("Min Amount reached!");
            this._UpdateSlot();
            return false;
        } else
        {
            this.amount += 1;
            this._UpdateSlot();
            return true;
        }
    }

    public bool DecProduct()
    {
        if (amount <= s_minAmount)
        {
            amount = s_minAmount;
//            Debug.Log("Min Amount reached!");
            this._UpdateSlot();
            return false;
        }
        else
        {
            this.amount -= 1;
//            Debug.Log("Took one " + product.GetName());
            this._UpdateSlot();
            return true;
        }
    }

    void _UpdateSlot()
    {
        if (amount == 0)
        {
            product = null;
            UI_SlotIcon.sprite = null;
            UI_SlotIcon.enabled = false;

            UI_SlotText.text = null;
            UI_SlotText.enabled = false;

            UI_Slider.UpdateSlider(amount);
        }
        else if (amount > 0) // to many calls!
        {
            UI_SlotIcon.sprite = product.Icon;
            UI_SlotIcon.enabled = true;

            UI_SlotText.text = AmountToUI();
            UI_SlotText.enabled = true;

            UI_Slider.UpdateSlider(amount);
        }
    }

    private string AmountToUI()
    {
        return "" + amount + "|" + s_maxAmount;
    }

    // Debug 
    public override string ToString()
    {
        if (product)
        {
            return (product.GetName() + "| Anzahl: " + this.amount);
        }
        else
        {
            return ("none");
        }
    }
}
