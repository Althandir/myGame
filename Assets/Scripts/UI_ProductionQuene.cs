using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Links UI_Slot to ProductionSlot. 
/// </summary>

public class UI_ProductionQuene : MonoBehaviour
{
    [Header("Debug_Values :: OnInit")]
    [SerializeField] Product _Product;
    [SerializeField] ProductionManager _ProductionManagerReference;
    [SerializeField] ProductionQuene _ProductionQueneReference;
    [Header("Debug_Values :: OnAwake")]
    [SerializeField] Image _Icon;
    [SerializeField] TMP_Text _TimeText;
    [SerializeField] Slider _ProductionProgressBar;
    [SerializeField] Button _AddAssignedWorkerButton;
    [SerializeField] Button _DecAssignedWorkerButton;
    [SerializeField] TMP_Text _WorkerNumber;

    #region Awake function
    private void Awake()
    {
        // TODO: give References directly in Prefab?
        _Icon = transform.GetChild(0).GetComponent<Image>();
        _TimeText = transform.GetChild(1).GetComponent<TMP_Text>();
        _ProductionProgressBar = transform.GetChild(2).GetComponent<Slider>();
        _AddAssignedWorkerButton = transform.GetChild(3).GetComponent<Button>();
        _DecAssignedWorkerButton = transform.GetChild(4).GetComponent<Button>();
        _WorkerNumber = transform.GetChild(5).GetComponent<TMP_Text>();

        // Adding functionality of Adding / Decreasing Workers to Buttons
        LinkButtonsToProductionQuene();
    }
    #endregion

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

    public void Update_FullUI()
    {
        // TODO: finish Update_FullUI
        
    }

    #region Update of ProductionProgressBar
    /// <summary>
    /// Sets the slider and the text of the ProductionProgessBar to the given number
    /// </summary>
    /// <param name="number"></param>
    public void Update_ProgessBar(float number)
    {
        _ProductionProgressBar.value = number;
        _TimeText.text = number.ToString();
    }
    #endregion

    #region Update of AssignedWorkerNumber
    /// <summary>
    /// Changes the WorkerNumberUI Text by looking up the Value in the referenced Quene
    /// </summary>
    public void Update_AssignedWorkerNumber()
    {
        _WorkerNumber.text = _ProductionQueneReference.NumAssignedWorker.ToString();
    }

    /// <summary>
    /// Called by OnClickEvent from Button to Increment 
    /// Number of AssignedWorker in the referenced Quene by 1
    /// </summary>
    void IncrementAssignedWorker()
    {
        _ProductionQueneReference.IncAssignedWorker();
        Update_AssignedWorkerNumber();      
    }

    /// <summary>
    /// Called by OnClickEvent from Button to Decrement 
    /// Number of AssignedWorker in the referenced Quene by 1
    /// </summary>
    void DecrementAssignedWorker()
    {
        _ProductionQueneReference.DecAssignedWorker();
        Update_AssignedWorkerNumber();
    }
    #endregion

    #region Init function
    public void Init(ProductionQuene productionQueneRef, Product product, ProductionManager productionManagerRef)
    {
        _Product = product;
        _ProductionManagerReference = productionManagerRef;
        _ProductionQueneReference = productionQueneRef;

        // Inital Update of the UI
        Update_FullUI();
    }
    #endregion
}
