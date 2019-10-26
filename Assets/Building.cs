using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    BuildingType _type;
    [SerializeField]
    bool isInitialized;

    public void SetType(BuildingType newType)
    {
        if (!isInitialized)
        {
            _type = newType;
        }
        else
        {
            Debug.LogWarning(gameObject.name + "is already initialized");
        }
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
