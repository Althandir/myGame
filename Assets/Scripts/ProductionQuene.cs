using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionQuene : MonoBehaviour
{

    [Header("Debug_Values::")]
    [SerializeField] bool isInit = false;
    [SerializeField] HasError hasError ;
    [SerializeField] byte NumAssignedWorker;
    [SerializeField] ProductionScript ProductionRef;
    [SerializeField] UI_ProductionQuene UI_ProductionSlotRef;

    private HasError _hasError { get => hasError; set => hasError = value; }

    #region Definition of Error
    private enum HasError
    {
        no, yes
    }
    #endregion

    public void Init(UI_ProductionQuene UI_ProductionSlotRef, ProductionScript ProductionRef)
    {
        if (!isInit)
        {
            this.UI_ProductionSlotRef = UI_ProductionSlotRef;
            this.ProductionRef = ProductionRef;
            
            isInit = true;
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " is already init!");
        }
    }

    public void StopAllRoutines()
    {
        StopAllCoroutines();
    }

    public void UI_Update()
    {

    }

    #region Increment & Decrement assigned Worker
    public void IncAssignedWorker()
    {
        if (ProductionRef.NumAvailableWorker != 0)
        {
            NumAssignedWorker += 1;
            ProductionRef.DecAvailableWorker();
            UI_Update();
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
            UI_Update();
        }
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
    }
    #endregion
}
