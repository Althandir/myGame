using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class StorageScript : MonoBehaviour
{
    // Values of the Storage 
    [Header("DEBUG_VALUES:")]
    [SerializeField]
    GameObject StorageScreen;
    [SerializeField]
    StorageSlot[] storageSlots;
    
    void Awake()
    {
        // Init Array
        storageSlots = new StorageSlot[6];
        // Get StorageUI                         Building    Camera      Screen
        StorageScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Init each slot + Link Slots to UI
        for (int i = 0; i < storageSlots.Length; i++)
        {
            storageSlots[i] = new StorageSlot(StorageScreen.transform.GetChild(3).GetChild(i).gameObject);
        }
        // close UI
        StorageScreen.SetActive(false); 
    }
    #region Inserting & Deducting Functions
    // Inserting from Slot no.0 to x
    public void InsertProduct(Product product, byte amount)
    {
        byte id = 0;
        while (amount != 0)
        {
            if (!storageSlots[id].Product) // insert into empty
            {
                storageSlots[id].SetProduct(product, amount);
                break;
            }
            else if (storageSlots[id].Product.Equals(product)) // insert into existing
            {
                for (int i = 0; i != amount; amount--) //adds Product amount times
                {
                    if (!storageSlots[id].AddProduct()) //if adding fails then:
                    {
                        break;
                    }
                };
            }
            id += 1; //get next Slot

            if (id == storageSlots.Length) // is is f.e. = 4 which is equals the lenght of array
            {
                Debug.Log(amount + " lost");
                break;
                //TODO::try not to lose content
            }
        }
    }

    //Deducting from Slot no.x to 0
    public void DeductProduct(Product product, byte amount)
    {
        int id = storageSlots.Length-1;
        while (amount != 0)
        {
            if (id == -1)
            {
                Debug.Log("Cannot Deduct! Missing " + amount + " " + product.Name);
                break;
                //TODO::Abort Deduction :: InformPlayer or Ignore if put on cart
            }
            if (storageSlots[id].Product)
            {
                if (storageSlots[id].Product.Equals(product)) // deduct
                {
                    for (int i = 0; i != amount; amount--) //deduct Product amount times
                    {
                        if (!storageSlots[id].DecProduct()) //if deducting fails then:
                        {
                            break;
                        }
                    };
                }
            }
            id -= 1; //get next Slot
        }
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
        InsertProduct(product, 20);
        */
        //DeductProduct(product.GetComponent<Product>(), 23);
        
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
