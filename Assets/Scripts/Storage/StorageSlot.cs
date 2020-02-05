using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageSlot 
{
    [SerializeField] Product product;
    [SerializeField] int amount;
    [SerializeField] UI_StorageSlot UI_StorageSlotRef;
    /// <summary>
    /// Gets the Product inside this Slot
    /// </summary>
    public Product Product { get => product; }
    
    /// <summary>
    /// Gets the Amount. 
    /// For changing Amount: see <see cref="InsertAmount(int)"/> and <see cref="DeductAmount(int)"/>
    /// </summary>
    public int Amount { get => amount; }

    public void InsertAmount(int amount)
    {
        this.amount += amount;
        UI_StorageSlotRef.UpdateStorageSlotUI();
    }

    public void DeductAmount(int amount)
    {
        this.amount -= amount;
        UI_StorageSlotRef.UpdateStorageSlotUI();
    }

    public StorageSlot(Product product, int amount, UI_StorageSlot ui_StorageSlot)
    {
        this.product = product;
        this.amount = amount;
        this.UI_StorageSlotRef = ui_StorageSlot;

        UI_StorageSlotRef.Init(this);
    }

}
