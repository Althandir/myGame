﻿using System.Collections;
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
    ProductionManager SelfProductionReference = null;
    [SerializeField]
    StorageManager SelfStorageReference = null;

    void Awake()
    {
        SelfStorageReference = transform.GetChild(4).GetComponent<StorageManager>();
        SelfProductionReference = transform.GetChild(5).GetComponent<ProductionManager>();
    }
    /// <summary>
    /// TODO: Delete Debugfunction. Just for Builds now.
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        ChangeType();
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
            SelfStorageReference.gameObject.SetActive(true);

            if (_type == BuildingType.Storage)
            {
                SelfProductionReference.gameObject.SetActive(false);
            }
            else
            {
                SelfProductionReference.gameObject.SetActive(true);
                SelfProductionReference.GetComponent<ProductionManager>().UpdateBuildingType(_type);
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
        SelfStorageReference.gameObject.SetActive(false);
        SelfProductionReference.gameObject.SetActive(false);
        SelfStorageReference.ResetSlots();
        SelfProductionReference.ResetProduction();
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
