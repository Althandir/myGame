using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionQuene : MonoBehaviour
{

    [Header("Debug_Values::")]
    [SerializeField] bool _isInit = false;
    [SerializeField] ProductionError _hasError ;
    [SerializeField] byte _NumAssignedWorker;
    [SerializeField] Product _Product;
    [SerializeField] ProductionManager _ProductionManagerRef;
    [SerializeField] StorageManager _StorageManagerRef;
    [SerializeField] UI_ProductionQuene _UI_ProductionQueneRef;
    [SerializeField] float _ProductionTimer;

    [SerializeField] byte overflowAmount = 0;
    [SerializeField] bool _hasResources;
    [SerializeField] bool[] _RequirementFullfilled;

    // TODO: keeping fullfilled, amount & product in one single class 

    private IEnumerator _Coroutine_Produce;
    [SerializeField] bool _ProductionActive = false;
    private IEnumerator _Coroutine_Timer;
    [SerializeField] bool _ProductionTimerActive = false;
    //private ProductionError hasError { get => _hasError; set => _hasError = value; }

    public byte NumAssignedWorker { get => _NumAssignedWorker; }

    #region Definition of Error
    private enum ProductionError
    {
        no, yes_NoRessources, yes_StorageFull
    }
    #endregion

    public void Init(UI_ProductionQuene UI_ProductionQueneRef, Product product, StorageManager storageManagerRef, ProductionManager productionManagerRef)
    {
        if (!_isInit)
        {
            _UI_ProductionQueneRef = UI_ProductionQueneRef;
            _Product = product;
            _StorageManagerRef = storageManagerRef;
            _ProductionManagerRef = productionManagerRef;

            _RequirementFullfilled = new bool[_Product.RequiredProducts.Length];
            
            _Coroutine_Produce = Produce();
            _Coroutine_Timer = ProductionTimeSystem();

            _isInit = true;
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " is already init!");
        }
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }

    void AssignedWorkerCheck()
    {
        if (_NumAssignedWorker == 0)
        {
            ProductionRoutineStop();
            ProductionTimerStop();
        }
        else if (_NumAssignedWorker > 0)
        {
            ProductionRoutineStart();
        }
    }

    #region ProductionTimer Start&Stopp
    void ProductionTimerStart()
    {
        if (!_ProductionTimerActive)
        {
            _ProductionTimerActive = true;
            StartCoroutine(_Coroutine_Timer);
        }/*
        else
        {
            Debug.LogWarning("Coroutine ProductionTimer in " + gameObject.name + " already started!");
        }*/
    }
    void ProductionTimerStop()
    {
        if (_ProductionTimerActive)
        {
            StopCoroutine(_Coroutine_Timer);
            _ProductionTimerActive = false;
            Debug.Log("Coroutine ProductionTimer in " + gameObject.name + " stopped!");
            ResetProductionTimer();
        }/*
        else
        {
            Debug.LogWarning("Tried to stop Coroutine ProductionTimer in " + gameObject.name + "!");
        }*/
    }
    #endregion

    #region ProductionTimer
    /// <summary>
    /// Local timing for Quene 
    /// </summary>
    /// <returns></returns>
    IEnumerator ProductionTimeSystem()
    {
        while (true)
        {
            _ProductionTimer += Time.deltaTime;
            _UI_ProductionQueneRef.Update_ProgessBar(_ProductionTimer);
            yield return null; // Prevents full freeze of Engine
        }
    }

    /// <summary>
    /// Sets the ProductionTimer back to 0 and resets the ProgressBar
    /// </summary>
    void ResetProductionTimer()
    {
        _ProductionTimer = 0;
        _UI_ProductionQueneRef.Update_ProgessBar(_ProductionTimer);
    }
    #endregion

    #region ProduceRoutine Start&Stopp
    /// <summary>
    /// Stops the Coroutine "Produce()" when active and sets its bool to false;
    /// </summary>
    void ProductionRoutineStop()
    {
        if (_ProductionActive)
        {
            StopCoroutine(_Coroutine_Produce);
            _ProductionActive = false;
            Debug.Log("Coroutine Produce in " + gameObject.name + " stopped!");
        }
        /*
        else
        {
            Debug.LogWarning("Tried to stop Coroutine Produce in " + gameObject.name + "!");
        }
        */
    }
    /// <summary>
    /// Starts the Coroutine "Produce()" when inactive and sets its bool to true;
    /// </summary>
    void ProductionRoutineStart()
    {
        if (!_ProductionActive)
        {
            _ProductionActive = true;
            StartCoroutine(_Coroutine_Produce);
        }
        /*
        else
        {
            Debug.LogWarning("Couroutine Produce in " + gameObject.name + " already started!");
        }
        */
    }
    #endregion

    #region Produce Routine
    IEnumerator Produce()
    {
        while (true)
        {
            // TODO: Design this better!!!! Horrible Layout but working :(
            if (!CheckAllRequirements())
            {
                _hasError = ProductionError.yes_NoRessources;
            }
            else
            {
                _hasResources = true;
            }

            // Check if there is an Error
            switch (_hasError)
            {
                case ProductionError.no: 
                    {
                        ProductionTimerStart();
                        // Check if Timer is completed
                        if (_ProductionTimer >= _Product.NeededProductionTime && _hasResources)
                        {
                            // TODO: Amount shouldn't be _NumAssignedWorker
                            Debug.Log(_Product.Name + " produced in " + _Product.NeededProductionTime + " Seconds.");
                            
                            overflowAmount = _StorageManagerRef.InsertProduct(_Product, _NumAssignedWorker);
                            ResetRequirements();

                            if (overflowAmount > 0 )
                            {
                                // Inserting failed because Storage is full!
                                _hasError = ProductionError.yes_StorageFull;
                            } 
                            ResetProductionTimer();
                        }
                        break;
                    }
                case ProductionError.yes_NoRessources:
                    {
                        // Timer to prevent Spam of the Storage
                        yield return new WaitForSecondsRealtime(1);

                        TryFullfillRequirements();
                        if (CheckAllRequirements())
                        {
                            _hasError = ProductionError.no;
                            _hasResources = true;
                            ProductionTimerStart();
                        }
                        else
                        {
                            Debug.LogWarning("Could not start Production, due to lack of needed Ressources");
                        }

                        // TODO: Display Error UI

                        ProductionTimerStop();
                        break;
                    }
                case ProductionError.yes_StorageFull:
                    {
                        // Timer to prevent Spam of the Storage
                        yield return new WaitForSecondsRealtime(1);
                        overflowAmount = TryInsertOverflowAmount(overflowAmount);
                        if (overflowAmount == 0)
                        {
                            _hasError = ProductionError.no;
                            Debug.Log("Overflow inserted!");
                        }
                        else
                        {
                            Debug.LogWarning("Storage Full! " + _StorageManagerRef.transform.parent.name);
                        }
                        // TODO: Display Error UI

                        ProductionTimerStop();
                        break;
                    }
                default:
                    {
                        Debug.LogError("Something went wrong in Produce! " + gameObject.name); ;
                        break;
                    }
            }
            yield return null; // Prevents full freeze of Engine
        }
    }

    bool CheckAllRequirements()
    {
        if (_RequirementFullfilled.Length > 0)
        {
            foreach (bool value in _RequirementFullfilled)
            {
                if (!value)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void TryFullfillRequirements()
    {
        if (_RequirementFullfilled.Length > 0)
        {
            for (int i = 0; i < _RequirementFullfilled.Length; i++)
            {
                if (_StorageManagerRef.DeductProduct(_Product.RequiredProducts[i], _Product.RequiredAmount[i]))
                {
                    _RequirementFullfilled[i] = true;
                }
            }
        }
    }

    void ResetRequirements()
    {
        for (int i = 0; i < _RequirementFullfilled.Length; i++)
        {
            _RequirementFullfilled[i] = false;
        }
        _hasResources = false;
    }

    byte TryInsertOverflowAmount(byte overflowAmount)
    {
        return _StorageManagerRef.InsertProduct(_Product, overflowAmount);
    }
    #endregion

    #region Increment & Decrement assigned Worker
    /// <summary>
    /// Checks if the Quene can take Workers from the ProductionManager:
    /// If so increments assigned Worker by 1 and decrements available Worker in Manager by 1
    /// Then checks if Couroutine "Produce" has to be enabled
    /// </summary>
    public void IncAssignedWorker()
    {
        if (_ProductionManagerRef.NumAvailableWorker != 0)
        {
            _NumAssignedWorker += 1;
            _ProductionManagerRef.DecAvailableWorker();
            AssignedWorkerCheck();
        }
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
    }
    /// <summary>
    /// Checks if the Quene can give back Workers to the ProductionManager:
    /// If so decrements assigned Worker by 1 and increments available Worker in Manager by 1
    /// Then checks if Couroutine "Produce" has to be disabled
    /// </summary>
    public void DecAssignedWorker()
    {
        if (_NumAssignedWorker != 0)
        {
            _NumAssignedWorker -= 1;
            _ProductionManagerRef.IncAvailableWorker();
            AssignedWorkerCheck();
        }/*
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
        */
    }
    #endregion
}
