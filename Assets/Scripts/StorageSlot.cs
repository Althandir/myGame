using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
class StorageSlot
{
    //staticValues
    static readonly byte s_maxAmount = 100;
    static readonly byte s_minAmount = 0;
    [Header("Debug_Values :: Runtime")]
    [SerializeField] GameObject SlotNum_UIReference = null;
    [SerializeField] Image UI_SlotIcon = null;
    [SerializeField] TMP_Text UI_SlotText = null;
    [SerializeField] UI_StorageSlider UI_Slider = null;
    [SerializeField] Product product = null;
    [SerializeField] byte amount = 0;

    public Product Product { get => product; set => product = value; }
    public byte Amount { get => amount; set => amount = value; }

    // create reference to UI & connect to specific Icon & disable Icon on Init
    public StorageSlot(GameObject SlotReference)
    {
        // TODO: Change into Prefab and give References directly in Prefab?
        SlotNum_UIReference = SlotReference;
        UI_SlotIcon = SlotNum_UIReference.transform.GetChild(0).GetComponent<Image>();
        UI_SlotText = SlotNum_UIReference.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        UI_Slider = SlotNum_UIReference.transform.GetChild(2).GetComponent<UI_StorageSlider>();

        UI_SlotIcon.enabled = false;
        UI_SlotIcon.color = Color.white;
        UI_SlotText.enabled = false;
        UI_Slider.Init(s_minAmount, s_maxAmount);
    }
    
    /// <summary>
    /// Sets the Product of this Slot and inserts the given amount
    /// </summary>
    /// <param name="product">Scriptable Object of type Product</param>
    /// <param name="amount">Amount to be inserted</param>
    public void SetProduct(Product product, byte amount)
    {
        this.product = product;
        this.amount = amount;
        _UpdateSlot();
    }

    /// <summary>
    /// Adding one Item of the defined Product into this Slot
    /// </summary>
    /// <returns>false if Max/Min Amount reached, true if successful</returns>
    public bool AddProduct()
    {
        if (amount >= s_maxAmount )
        {
            amount = s_maxAmount;
            // Debug.Log("Max Amount reached!");
            this._UpdateSlot();
            return false;
        } else if (amount <= s_minAmount)
        {
            amount = s_minAmount;
            // Debug.Log("Min Amount reached!");
            this._UpdateSlot();
            return false;
        } else
        {
            this.amount += 1;
            this._UpdateSlot();
            return true;
        }
    }

    /// <summary>
    /// Decrements one Item of the defined Product from this Slot
    /// </summary>
    /// <returns>false if Min Amount reached, true if successful</returns>
    public bool DecProduct()
    {
        if (amount <= s_minAmount)
        {
            amount = s_minAmount;
            // Debug.Log("Min Amount reached!");
            this._UpdateSlot();
            return false;
        }
        else 
        {
            this.amount -= 1;
            // Debug.Log("Took one " + product.GetName());
            this._UpdateSlot();
            return true;
        }

    }

    void _UpdateSlot()
    {
        if (amount == 0)
        {
            ResetSlot();
        }
        else if (amount > 0) 
        {
            UI_SlotIcon.sprite = product.Icon;
            UI_SlotIcon.enabled = true;

            UI_SlotText.text = AmountToUI();
            UI_SlotText.enabled = true;
            UI_Slider.UpdateSlider(amount);
        }
    }

    public void ResetSlot()
    {
        product = null;
        UI_SlotIcon.sprite = null;
        UI_SlotIcon.enabled = false;

        UI_SlotText.text = null;
        UI_SlotText.enabled = false;
        UI_Slider.UpdateSlider(0);
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
            return (product.Name + "| Anzahl: " + this.amount);
        }
        else
        {
            return ("none");
        }
    }
}
