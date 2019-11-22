using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionQuene : MonoBehaviour
{
    [Header("Debug_Values::")]
    [SerializeField] byte NumAssignedWorker;
    [SerializeField] ProductionScript ProductionRef;
    [SerializeField] GameObject UI_ProductionSlotRef;
    private void Awake()
    {
        ProductionRef = this.transform.parent.GetComponent<ProductionScript>();
    }

    public void IncAssignedWorker()
    {
        if (ProductionRef.NumAvailableWorker != 0)
        {
            NumAssignedWorker += 1;
        }
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
    }

    public void DecAssignedWorker()
    {
        if (NumAssignedWorker != 0)
        {
            NumAssignedWorker -= 1;
            ProductionRef.IncAvailableWorker();
        }
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
    }

}
