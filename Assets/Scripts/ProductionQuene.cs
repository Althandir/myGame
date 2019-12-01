using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionQuene : MonoBehaviour
{

    [Header("Debug_Values::")]
    [SerializeField] bool isInit = false;
    [SerializeField] HasError hasError;
    [SerializeField] byte NumAssignedWorker;
    [SerializeField] ProductionScript ProductionRef;
    [SerializeField] GameObject UI_ProductionSlotRef;
    
    #region Definition of Error
    private enum HasError
    {
        yes, no
    }
    #endregion

    private void Awake()
    {
        ProductionRef = this.transform.parent.GetComponent<ProductionScript>();
    }



    public void Init(GameObject UI_ProductionSlotRef)
    {
        if (!isInit)
        {
            this.UI_ProductionSlotRef = UI_ProductionSlotRef;


            isInit = true;
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " is already init!");
        }
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
    #endregion
}
