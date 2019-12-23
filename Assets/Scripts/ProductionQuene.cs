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
    [SerializeField] UI_ProductionQuene _UI_ProductionQueneRef;
    [SerializeField] float _ProductionTimer;

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

    public void Init(UI_ProductionQuene UI_ProductionQueneRef, Product product, ProductionManager ProductionManagerRef)
    {
        if (!_isInit)
        {
            _UI_ProductionQueneRef = UI_ProductionQueneRef;
            _ProductionManagerRef = ProductionManagerRef;
            _Product = product;

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
            ProductionTimerStart();
        }
    }

    #region ProductionTimer Start&Stopp
    void ProductionTimerStart()
    {
        if (!_ProductionTimerActive)
        {
            _ProductionTimerActive = true;
            StartCoroutine(_Coroutine_Timer);
            Debug.Log("Coroutine ProductionTimer in " + gameObject.name + " started!");
        }
        else
        {
            Debug.LogWarning("Couroutine ProductionTimer in " + gameObject.name + " already started!");
        }
    }
    void ProductionTimerStop()
    {
        if (_ProductionTimerActive)
        {
            StopCoroutine(_Coroutine_Timer);
            _ProductionTimerActive = false;
            Debug.Log("Coroutine ProductionTimer in " + gameObject.name + " stopped!");
            ResetProductionTimer();
        }
        else
        {
            Debug.LogWarning("Tried to stop Coroutine ProductionTimer in " + gameObject.name + "!");
        }
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
        else
        {
            Debug.LogWarning("Tried to stop Coroutine Produce in " + gameObject.name + "!");
        }
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
            Debug.Log("Couroutine Produce in " + gameObject.name + " started!");
        }
        else
        {
            Debug.LogWarning("Couroutine Produce in " + gameObject.name + " already started!");
        }

    }
    #endregion

    #region Produce Routine
    IEnumerator Produce()
    {
        while (true)
        {
            // Check if there is an Error
            switch (_hasError)
            {
                case ProductionError.no: 
                    {
                        // Check if Timer is completed
                        if (_ProductionTimer >= _Product.NeededProductionTime)
                        {
                            // TODO: Take needed Ressources from Storage & Set Error if no Ressources available

                            // TODO: Generate Product
                            Debug.Log(_Product.Name + " produced in " + _Product.NeededProductionTime + " Seconds.");
                            ResetProductionTimer();
                        }
                        break;
                    }
                case ProductionError.yes_NoRessources:
                    {
                        // TODO: Display Error UI
                        break;
                    }
                case ProductionError.yes_StorageFull:
                    {
                        // TODO: Display Error UI
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
        }
        else
        {
            Debug.LogWarning("Parent does not have enough available Worker!");
        }
    }
    #endregion
}
