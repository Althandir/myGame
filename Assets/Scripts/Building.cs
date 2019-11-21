using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Debug_Values::")]
    [SerializeField]
    BuildingType _type;
    [SerializeField]
    bool isInitialized = false;
    [SerializeField]
    ProductionScript ownProductionReference = null;
    [SerializeField]
    StorageScript ownStorageReference = null;

    void Awake()
    {
        ownStorageReference = transform.GetChild(4).GetComponent<StorageScript>();
        ownProductionReference = transform.GetChild(5).GetComponent<ProductionScript>();
    }

    // Works as Intended!
    [ContextMenu("Cycle Type")]
    void ChangeType()
    {
        isInitialized = false;
        if (_type == BuildingType.Farm)
        {
            SetType(BuildingType.Woodcutter);
        }
        else if (_type == BuildingType.Woodcutter)
        {
            SetType(BuildingType.Storage);
        }
        else
        {
            SetType(BuildingType.Farm);
        }
    }

    public void SetType(BuildingType newType)
    {
        if (!isInitialized)
        {
            _type = newType;
            ownStorageReference.gameObject.SetActive(true);

            if (_type == BuildingType.Storage)
            {
                ownProductionReference.gameObject.SetActive(false);
            }
            else
            {
                ownProductionReference.gameObject.SetActive(true);
                ownProductionReference.GetComponent<ProductionScript>().UpdateBuildingType(_type);
            }
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning(gameObject.name + " is already initialized");
        }
    }
    [ContextMenu("Reset Type")]
    public void ResetType()
    {
        isInitialized = false;
        _type = BuildingType.Undefined;
        ownStorageReference.gameObject.SetActive(false);
        ownProductionReference.gameObject.SetActive(false);
        ownStorageReference.ResetSlots();
        ownProductionReference.ResetProduction();
    }

    public BuildingType GetBuildingType()
    {
        return _type;
    }
}

public enum BuildingType
{
    Undefined, Storage, Farm, Woodcutter
}
