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

    [SerializeField] List<ProductionRequirement> _ProductionRequirements;

    private IEnumerator _Coroutine_Produce;
    [SerializeField] bool _ProductionActive = false;
    private IEnumerator _Coroutine_Timer;
    [SerializeField] bool _ProductionTimerActive = false;
    //private ProductionError hasError { get => _hasError; set => _hasError = value; }

    public byte NumAssignedWorker { get => _NumAssignedWorker; }
    public List<ProductionRequirement> ProductionRequirements { get => _ProductionRequirements; }

    #region Definition of Error
    private enum ProductionError
    {
        no, yes_NoRessources
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

            CreateProductionRequirements();

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

    /// <summary>
    /// Creates new Objects to store Requirement with a bool.
    /// </summary>
    void CreateProductionRequirements()
    {
        foreach (ProductRequirement requirement in _Product.ProductRequirements)
        {
            _ProductionRequirements.Add(new ProductionRequirement(requirement));
        }
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
            ResetFullfilledRequirements();
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
            // try to get required Ressources
            // if this product has any requirements
            if (HasAnyRequirements())
            {
                // while there are unfullfilled requirements
                while (!CheckAllRequirements())
                {
                    // try to fullfill them
                    TryFullfillRequirements();
                    // prevents Spam of Storage
                    yield return new WaitForEndOfFrame();
                }
                // if fullfilled start the timer
                ProductionTimerStart();
            }
            else 
            {
                // just start the timer
                ProductionTimerStart();
            }

            switch (_hasError)
            {
                case ProductionError.no:
                    {
                        if (_ProductionTimer > _Product.NeededProductionTime / _NumAssignedWorker)
                        {
                            CreateProduct();
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }
                case ProductionError.yes_NoRessources:
                    {
                        Debug.LogWarning(this.gameObject.name + " has no Resources!");
                        break;
                    }
                default:
                    {
                        Debug.LogError("Something went wrong in Produce() in " + this.gameObject.name);
                        break;
                    }
            }
            yield return null; // Prevents full freeze of Engine
        }
    }

    void CreateProduct()
    {
        _StorageManagerRef.InsertProduct(_Product, 1);
        ResetProductionTimer();
        ProductionTimerStop();
        ResetFullfilledRequirements();
        Debug.Log(_Product.Name + " produced in " + _Product.NeededProductionTime + " Seconds.");
    }

    /// <summary>
    /// Checks if this Product has any Requirements to be fullfilled
    /// </summary>
    /// <returns></returns>
    bool HasAnyRequirements()
    {
        if (_ProductionRequirements.Count > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if all Requirements for this Product are fullfilled.
    /// Sets <see cref="_hasError"/> to no or yes_NoRessources.
    /// </summary>
    /// <returns></returns>
    bool CheckAllRequirements()
    {
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            if (!requirement.IsFullfilled)
            {
                _hasError = ProductionError.yes_NoRessources;
                return false;
            }
        }
        _hasError = ProductionError.no;
        return true;
    }

    /// <summary>
    /// Sets all booleans in <see cref="_ProductionRequirements"/> to false.
    /// </summary>
    void ResetFullfilledRequirements()
    {
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            requirement.IsFullfilled = false;
        }
        _UI_ProductionQueneRef.UpdateRequirementStatus();
    }

    /// <summary>
    /// Accesses Storage and tries to deduct needed Ressources.
    /// If successfull: IsFullfilled is set to true.
    /// Else: IsFullfilled is set to false.
    /// /// </summary>
    void TryFullfillRequirements()
    {
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            if (!requirement.IsFullfilled)
            {
                requirement.IsFullfilled = _StorageManagerRef.DeductProduct(requirement.GetProduct(), requirement.GetRequiredAmount());
            }
        }
        _UI_ProductionQueneRef.UpdateRequirementStatus();
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
