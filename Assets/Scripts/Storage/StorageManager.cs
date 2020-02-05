using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class StorageManager : MonoBehaviour
{
    [Header("Reference to the Prefabs for the StorageSlot")]
    [SerializeField] GameObject UI_StorageSlotPrefab;

    // Values of the Storage 
    [Header("DEBUG_VALUES:")]
    [SerializeField] GameObject UI_StorageScreen;
    [SerializeField] Transform UI_StorageContent;
    [SerializeField] UI_StorageValues UI_StorageValues;
    [SerializeField] StorageSettings _StorageSettings;
    [SerializeField] List<StorageSlot> _StorageSlots;

    void Awake()
    {
        LinkToUI();
        _StorageSettings = new StorageSettings(_StorageSlots);
        UpdateSettingValues();
    }

    void UpdateSettingValues()
    {
        _StorageSettings.UpdateActualAmount();
        // TODO: Thniking about an init Update of UI_StorageValues
        UpdateUI_ActAmount();
        UpdateUI_MaxAmount();
        UpdateUI_Level();
    }

    void UpdateUI_ActAmount()
    {
        Debug.Log("UpdateActValue Called in Manager");
        UI_StorageValues.UpdateActAmountText(_StorageSettings.ActualAmount);
    }

    void UpdateUI_MaxAmount()
    {
        UI_StorageValues.UpdateMaxAmountText(_StorageSettings.CurrentMaxAmount);
    }

    void UpdateUI_Level()
    {
        UI_StorageValues.UpdateLevelText(_StorageSettings.Level);
    }

    void LinkToUI()
    {
        // TODO: Give Reference directly in Prefab
        // Get StorageUI                         Building    Camera      Screen
        UI_StorageScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(0).gameObject;
        UI_StorageContent = UI_StorageScreen.transform.GetChild(2).GetChild(0).GetChild(0);
        UI_StorageValues = UI_StorageScreen.transform.GetChild(3).GetComponent<UI_StorageValues>();
        // close UI
        UI_StorageScreen.SetActive(false);
    }

    #region Inserting & Deducting Functions
    /// <summary>
    /// Inserting Product into a StorageSlot
    /// if the Storage doesn't has the Product it creates a new Slot for this Product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="amount"></param>
    public void InsertProduct(Product product, int amount)
    {
        if (_StorageSlots.Count > 0)
        {
            if (!TryInsertIntoExistingSlot(product, amount))
            {
                _StorageSlots.Add(CreateNewSlot(product, amount));
                // TODO: Combine Update of Settings and UI in one line;
                _StorageSettings.UpdateActualAmount();
                UpdateUI_ActAmount();
            }
        }
        else
        {
            _StorageSlots.Add(CreateNewSlot(product, amount));
            // TODO: Combine Update of Settings and UI in one line;
            _StorageSettings.UpdateActualAmount();
            UpdateUI_ActAmount();
        }
    }

    bool TryInsertIntoExistingSlot(Product product, int amount)
    {
        foreach (StorageSlot storageSlot in _StorageSlots)
        {
            if (storageSlot.Product.Equals(product))
            {
                Debug.Log("Insert into existing!");
                storageSlot.InsertAmount(amount);
                // TODO: Combine Update of Settings and UI in one line;
                _StorageSettings.UpdateActualAmount();
                UpdateUI_ActAmount();
                return true;
            }
        }
        return false;
    }

    StorageSlot CreateNewSlot(Product product, int amount )
    {
        Debug.Log("Adding new Product into Storage");
        return new StorageSlot(product, amount, CreateNewUI_Slot());
    }

    UI_StorageSlot CreateNewUI_Slot()
    {
        GameObject UI_StorageSlot = Instantiate(UI_StorageSlotPrefab, UI_StorageContent);
        return UI_StorageSlot.GetComponent<UI_StorageSlot>();
    }

    /// <summary>
    /// Deducting from Storage.
    /// Returns true, if Storage could deduct the product.
    /// Returns false, if Storage doesn't has enough of the product.
    /// </summary>
    public bool DeductProduct(Product product, int requestedAmount)
    {
        foreach (StorageSlot slot in _StorageSlots)
        {
            // Checks if the Storage has the requested Product
            if (slot.Product.Equals(product))
            {
                // Checks if the Product is available amount times
                if (slot.Amount >= requestedAmount)
                {
                    // Deduct Product
                    slot.DeductAmount(requestedAmount);
                    // TODO: Combine Update of Settings and UI in one line;
                    _StorageSettings.UpdateActualAmount();
                    UpdateUI_ActAmount();
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region Resetfunction
    public void ResetSlots()
    {
        _StorageSlots.Clear();
    }
    #endregion
    public void OnClick()
    {
        //Display GUI...
        Debug.Log("You opened the Storage of " + gameObject.transform.parent.name);
        if (!UI_StorageScreen.activeSelf)
            UI_StorageScreen.SetActive(true);
        // Test for Apple
        // Working as intended!
        /*
        Product product = ProductionLists.FarmList[0];
        Product product2 = ProductionLists.FarmList[3];
        InsertProduct(product, 20);
        
        DeductProduct(product, 10);
        InsertProduct(product2, 23);
        */
        //Debug.Log(this);

    }
}
