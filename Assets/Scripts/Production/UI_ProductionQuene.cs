using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
/// <summary>
/// Links UI_Slot to ProductionSlot. 
/// </summary>

public class UI_ProductionQuene : MonoBehaviour
{
    [Header("Debug_Values :: OnInit")]
    [SerializeField] Product _Product;
    [SerializeField] ProductionManager _ProductionManagerReference;
    [SerializeField] ProductionQuene _ProductionQueneReference;
    [Header("Linked UI Components")]
    [SerializeField] Image _Icon;
    [SerializeField] TMP_Text _TimeText;
    [SerializeField] Slider _ProductionProgressBar;
    [SerializeField] Button _AddAssignedWorkerButton;
    [SerializeField] Button _DecAssignedWorkerButton;
    [SerializeField] TMP_Text _WorkerNumber;
    [SerializeField] Transform _RequirementArea;
    [SerializeField] UI_ProductionQueneRequirementUpdater _RequirementUpdater;
    public ProductionQuene ProductionQueneReference { get => _ProductionQueneReference; }

    #region Init function
    public void Init(ProductionQuene productionQueneRef, Product product, ProductionManager productionManagerRef)
    {
        //TODO: Find better Solution than this: Awake is called only is GameObject is enabled :/
        _Icon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _TimeText = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        _ProductionProgressBar = transform.GetChild(0).GetChild(2).GetComponent<Slider>();
        _AddAssignedWorkerButton = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        _DecAssignedWorkerButton = transform.GetChild(1).GetChild(1).GetComponent<Button>();
        _WorkerNumber = transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        _RequirementArea = transform.GetChild(2);
        _RequirementUpdater = _RequirementArea.GetComponent<UI_ProductionQueneRequirementUpdater>();
        // Adding functionality of Adding / Decreasing Workers to Buttons
        LinkButtonsToProductionQuene();

        _Product = product;
        _ProductionManagerReference = productionManagerRef;
        _ProductionQueneReference = productionQueneRef;

        // Sets Icon of the UI_ProductQuene
        _Icon.sprite = _Product.Icon;

        Update_AssignedWorkerNumber();
        Update_ProgessBar(0);

        // Linking UI_RequirementsUpdater with Quene
        _RequirementUpdater.LinkToRequirementList(_ProductionQueneReference.ProductionRequirements);
    }
    #endregion


    #region LinkButtons
    void LinkButtonsToProductionQuene()
    {
        if (_AddAssignedWorkerButton != null && _DecAssignedWorkerButton != null)
        {
            _AddAssignedWorkerButton.onClick.AddListener(IncrementAssignedWorker);
            _DecAssignedWorkerButton.onClick.AddListener(DecrementAssignedWorker);
        }
        else
        {
            Debug.LogError("Something went wrong during Button linking!");
        }
    }
    #endregion

    #region Update of ProductionProgressBar
    /// <summary>
    /// Sets the slider and the text of the ProductionProgessBar to the given number
    /// </summary>
    /// <param name="number"></param>
    public void Update_ProgessBar(float number)
    {
        float value = (float) System.Math.Round((decimal)number, 2);
        _ProductionProgressBar.value = value;
        
        // TODO: make this better.
        if (NoAssignedWorker())
        {
            _TimeText.text = value.ToString() + " | " + (_ProductionProgressBar.maxValue);
        }
        else
        {
            _TimeText.text = value.ToString() + " | " + (_ProductionProgressBar.maxValue);
        }
        
    }
    #endregion
    #region Update of AssignedWorkerNumber

    bool NoAssignedWorker()
    {
        if (_ProductionQueneReference.NumAssignedWorker == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Changes the WorkerNumberUI Text by looking up the Value in the referenced Quene.
    /// Also updates the MaximunValue of the Slider
    /// </summary>
    public void Update_AssignedWorkerNumber()
    {
        _WorkerNumber.text = _ProductionQueneReference.NumAssignedWorker.ToString();

        // TODO: make this better.
        if (NoAssignedWorker())
        {
            _ProductionProgressBar.maxValue = (_Product.NeededProductionTime);
        }
        else
        {
            _ProductionProgressBar.maxValue = (float) System.Math.Round((decimal)(_Product.NeededProductionTime / _ProductionQueneReference.NumAssignedWorker), 2);
        }
        
    }

    /// <summary>
    /// Called by OnClickEvent from Button to Increment 
    /// Number of AssignedWorker in the referenced Quene by 1
    /// </summary>
    void IncrementAssignedWorker()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 0; i < 5; i++)
            {
                _ProductionQueneReference.IncAssignedWorker();
            }
        }
        else
        {
            _ProductionQueneReference.IncAssignedWorker();
        }
        Update_AssignedWorkerNumber();      
    }

    /// <summary>
    /// Called by OnClickEvent from Button to Decrement 
    /// Number of AssignedWorker in the referenced Quene by 1
    /// </summary>
    void DecrementAssignedWorker()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 0; i < 5; i++)
            {
                _ProductionQueneReference.DecAssignedWorker();
            }
        }
        else
        {
            _ProductionQueneReference.DecAssignedWorker();
        }
        Update_AssignedWorkerNumber();
    }
    #endregion
    #region Update of Requirements
    public void UpdateRequirementsFullUI()
    {
        _RequirementUpdater.FullRequirementsUIUpdate();
    }
    public void UpdateRequirementStatus()
    {
        _RequirementUpdater.UpdateStatus();
    }
    #endregion





}
