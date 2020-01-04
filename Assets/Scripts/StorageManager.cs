using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class StorageManager : MonoBehaviour
{
    [Header("Storage Options:")]
    [Tooltip("Amount of Slots this Storage has. Keep in Sync with StorageSlots in UI")]
    [SerializeField]
    byte _amountOfSlots = 12;

    // Values of the Storage 
    [Header("DEBUG_VALUES:")]
    [SerializeField]
    GameObject StorageScreen;
    [SerializeField]
    StorageSlot[] storageSlots;
    
    void Awake()
    {
        // Init Array
        storageSlots = new StorageSlot[_amountOfSlots];
        // Get StorageUI                         Building    Camera      Screen
        StorageScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Init each slot + Link Slots to UI
        for (int i = 0; i < storageSlots.Length; i++)
        {
            storageSlots[i] = new StorageSlot(StorageScreen.transform.GetChild(2).GetChild(i).gameObject);
        }
        // close UI
        StorageScreen.SetActive(false); 
    }
    #region Inserting & Deducting Functions
    /// <summary>
    /// Inserting Product with amount from Slot [0] to [x] 
    /// Returns the Amount which could not inserted into a slot
    /// </summary>
    /// <param name="product"></param>
    /// <param name="amount"></param>
    /// <returns>the amount left</returns>
    public byte InsertProduct(Product product, byte amount)
    {
        byte id = 0;
        while (amount != 0)
        {
            if (!storageSlots[id].Product) // insert into empty
            {
                storageSlots[id].SetProduct(product); // Sets the Product of the Slot
                // TODO: Delete duplicated Code
                for (int i = 0; i != amount; amount -= 1) //adds Product amount times
                {
                    if (!storageSlots[id].AddProduct()) //if adding fails then:
                    {
                        break;
                    }
                };
            }
            else if (storageSlots[id].Product.Equals(product)) // insert into existing
            {
                for (int i = 0; i != amount; amount -= 1) //adds Product amount times
                {
                    if (!storageSlots[id].AddProduct()) //if adding fails then:
                    {
                        break;
                    }
                };
            }
            id += 1; //get next Slot

            if (id == storageSlots.Length) 
            {
                // Debug.Log(amount + " lost");
                break;
                //TODO: try not to lose content
            }
        }
        return amount;
    }

    /// <summary>
    /// Deducting from Slot [x] to [0]
    /// Returns true, if Storage could deduct the product by 1 amount times.
    /// Returns false, if Storage doesn't has enough of the product
    /// </summary>
    public bool DeductProduct(Product product, byte requestedAmount)
    {
        int id = storageSlots.Length-1;
        while (requestedAmount != 0)
        {
            if (id == -1)
            {
                Debug.Log("Cannot Deduct! Missing " + requestedAmount + " " + product.Name);
                return false;
            }
            if (storageSlots[id].Product) // Slot is holding a Product
            {
                if (storageSlots[id].Product.Equals(product)) // Storage has requested Product
                {
                    if (storageSlots[id].Amount >= requestedAmount) // Storage holds enought of this Product
                    {
                        for (int i = 0; i != requestedAmount; requestedAmount--) //deduct Product amount times
                        {
                            if (!storageSlots[id].DecProduct()) //if deducting fails then:
                            {
                                return false;
                            }
                        };
                    }
                }
            }
            id -= 1; //get next Slot
        }
        // Deducting successful!
        return true;
    }
    #endregion

    #region Resetfunction
    public void ResetSlots()
    {
        foreach (StorageSlot slot in storageSlots)
        {
            slot.ResetSlot();
        }
    }
    #endregion
    public void OnClick()
    {
        //Display GUI...
        Debug.Log("You opened the Storage of " + gameObject.transform.parent.name);
        if (!StorageScreen.activeSelf)
            StorageScreen.SetActive(true);
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

    public override string ToString()
    {
        string contents = "";
        for (int i = 0; i < storageSlots.Length; i++)
        {
            contents = contents + "Slot no." + i + ":: " + storageSlots[i].ToString() + " || ";
        }
        return contents;
    }

}
