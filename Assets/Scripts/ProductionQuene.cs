using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionQuene : MonoBehaviour
{

    [Header("Debug_Values::")]
    [SerializeField] bool isInit = false;
    [SerializeField] ProductionError _hasError ;
    [SerializeField] byte NumAssignedWorker;
    [SerializeField] Product _Product;
    [SerializeField] ProductionManager _ProductionManagerRef;
    [SerializeField] UI_ProductionQuene _UI_ProductionSlotRef;

    private ProductionError hasError { get => _hasError; set => _hasError = value; }

    #region Definition of Error
    private enum ProductionError
    {
        no, yes
    }
    #endregion

    public void Init(UI_ProductionQuene UI_ProductionSlotRef, Product product, ProductionManager ProductionManagerRef)
    {
        if (!isInit)
        {
            _UI_ProductionSlotRef = UI_ProductionSlotRef;
            _ProductionManagerRef = ProductionManagerRef;
            _Product = product;

            StartCoroutine(Produce());


            isInit = true;
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " is already init!");
        }
    }

    IEnumerator Produce()
    {
        while (_hasError == 0)
        {
            yield return new WaitForSeconds(_Product.NeededProductionTime);
            Debug.Log(_Product.Name + " produced in " + _Product.NeededProductionTime + " Seconds.");
        }
        Debug.LogError("ProductionQuene Routine ended! " + gameObject.name);
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
        if (_ProductionManagerRef.NumAvailableWorker != 0)
        {
            NumAssignedWorker += 1;
            _ProductionManagerRef.DecAvailableWorker();
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
            _ProductionManagerRef.IncAvailableWorker();
            UI_Update();
        }
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
    }
    #endregion
}
