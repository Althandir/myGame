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
    GameObject ownProductionReference = null;
    [SerializeField]
    GameObject ownStorageReference = null;

    void Awake()
    {
        ownStorageReference = transform.GetChild(4).gameObject;
        ownProductionReference = transform.GetChild(5).gameObject;
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
            ownStorageReference.SetActive(true);

            if (_type == BuildingType.Storage)
            {
                ownProductionReference.SetActive(false);
            }
            else
            {
                ownProductionReference.SetActive(true);
                ownProductionReference.GetComponent<ProductionScript>().UpdateBuildingType(_type);
            }
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning(gameObject.name + " is already initialized");
        }
    }

    public void ResetType()
    {
        isInitialized = false;
        _type = BuildingType.Undefined;
        ownStorageReference.SetActive(false);
        ownProductionReference.SetActive(false);
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
