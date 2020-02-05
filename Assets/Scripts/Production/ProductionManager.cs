using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Interactable))]

public class ProductionManager : MonoBehaviour
{
    const byte initWorkerCost = 100;
    const byte maxWorkerNum = 100;
    const byte minWorkerNum = 0;
    
    [Header("Reference to the Prefabs of the ProductionQuene")]
    [SerializeField] GameObject UI_ProductQuenePrefab = null;
    [SerializeField] GameObject _ProductQuenePrefab = null;
    [Header("Debug_Values :: OnAwake")]
    [SerializeField] Transform _ProductionScreen;
    [SerializeField] StorageManager _BuildingStorage;
    [SerializeField] Transform UI_ProductionContent = null;
    [SerializeField] Transform UI_WorkerPanel = null;
    [SerializeField] List<GameObject> UI_ProductionQueneList;
    [SerializeField] List<GameObject> _ProductionQueneList;
    [SerializeField] Button UI_IncreaseOverallWorkerButton;
    [SerializeField] Button UI_DecreaseOverallWorkerButton;
    [SerializeField] ScrollviewContentHeightUpdater UI_ContentHeightUpdater;
    [Header("Debug_Values :: Runtime")]
    [SerializeField] BuildingType buildingType;
    [SerializeField] List<Product> productList;
    [SerializeField] byte numOverallWorker;
    [SerializeField] byte numAvailableWorker;
    [SerializeField] uint newWorkerCost;
    [SerializeField] float salaryTimer;



    void Awake()
    {
        // TODO: Maybe giving References directly in finished Prefab?
        // Link to UI                                Building    Camera      Screen
        _ProductionScreen = this.gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(1);
        UI_ProductionContent = _ProductionScreen.GetChild(2).GetChild(0).GetChild(0);
        UI_WorkerPanel = _ProductionScreen.GetChild(3);

        UI_IncreaseOverallWorkerButton = UI_WorkerPanel.GetChild(0).GetChild(0).GetComponent<Button>();
        UI_DecreaseOverallWorkerButton = UI_WorkerPanel.GetChild(0).GetChild(1).GetComponent<Button>();
        UI_IncreaseOverallWorkerButton.onClick.AddListener(IncreaseOverallWorker);
        UI_DecreaseOverallWorkerButton.onClick.AddListener(DecreaseOverallWorker);
        
        UI_ContentHeightUpdater = UI_ProductionContent.GetComponent<ScrollviewContentHeightUpdater>();


        // Link to Storage
        _BuildingStorage = this.transform.parent.GetChild(4).GetComponent<StorageManager>();
        // Init List
        UI_ProductionQueneList = new List<GameObject>();
        _ProductionQueneList = new List<GameObject>();
    }

    private void Start()
    {
        ResetProduction();
    }

    void Update()
    {
        salaryTimer += Time.deltaTime;
    }

    #region ResetFunction
    public void ResetProduction()
    {
        numOverallWorker = 0;
        numAvailableWorker = 0;
        salaryTimer = 0.0f;
        productList = null;
        newWorkerCost = initWorkerCost;
        ResetProductionQuene();
        UI_ProductionQueneList.Clear();
        UI_ResetProductionQuene();
        UI_UpdateWorkerNumber();
        UI_UpdateWorkerPrice();

        // Disable UI
        _ProductionScreen.gameObject.SetActive(false);
    }
    #endregion

    #region Calculation for WorkerCosts

    void CalcNewWorkerCost()
    {
        // log(aX^2) + bx + c  
        // log Limits the maximum Number while x^2 causes a high rise in costs
        newWorkerCost = (uint)(Mathf.Log(newWorkerCost * numOverallWorker ^ 2) + initWorkerCost * numOverallWorker + newWorkerCost);
        UI_UpdateWorkerPrice();
    }

    #endregion

    #region Functions for Worker
    
    #region Overall
    public void IncreaseOverallWorker()
    {
        // TODO: Delete duplicated Code
        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 0; i < 5; i++)
            {
                // max 99
                if (numOverallWorker < maxWorkerNum)
                {
                    numOverallWorker += 1;
                    numAvailableWorker += 1;
                    UI_UpdateWorkerNumber();
                    CalcNewWorkerCost();
                }
                else if (numOverallWorker == maxWorkerNum)
                {
                    Debug.LogWarning("Max amount of Worker reached!");
                }
            }
        }
        else
        {
            // max 99
            if (numOverallWorker < maxWorkerNum)
            {
                numOverallWorker += 1;
                numAvailableWorker += 1;
                UI_UpdateWorkerNumber();
                CalcNewWorkerCost();
            }
            else if (numOverallWorker == maxWorkerNum)
            {
                Debug.LogWarning("Max amount of Worker reached!");
            }
        }
        
    }

    public void DecreaseOverallWorker()
    {
        // TODO: Delete duplicated Code
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // min 0
            if (numOverallWorker >= minWorkerNum && numAvailableWorker != 0)
            {
                numOverallWorker -= 1;
                numAvailableWorker -= 1;
                UI_UpdateWorkerNumber();
            }
            else if (numOverallWorker == minWorkerNum)
            {
                Debug.LogWarning("Min amount of Worker reached!");
            }
            else if (numAvailableWorker == 0)
            {
                Debug.LogWarning("No unassigned Worker to fire!");
            }
        }
        else
        {
            if (numOverallWorker >= minWorkerNum && numAvailableWorker != 0)
            {
                numOverallWorker -= 1;
                numAvailableWorker -= 1;
                UI_UpdateWorkerNumber();

            }
            else if (numOverallWorker == minWorkerNum)
            {
                Debug.LogWarning("Min amount of Worker reached!");
            }
            else if (numAvailableWorker == 0)
            {
                Debug.LogWarning("No unassigned Worker to fire!");
            }
        }
        
    }
    // Get Property
    public byte NumOverallWorker { get => numOverallWorker; }
    
    #endregion

    #region Available Worker Functions

    public void IncAvailableWorker()
    {
        numAvailableWorker += 1;
        UI_UpdateWorkerNumber();
    }

    public void DecAvailableWorker()
    {
        numAvailableWorker -= 1;
        UI_UpdateWorkerNumber();
    }
    // Get Property
    public byte NumAvailableWorker { get => numAvailableWorker; }
    #endregion

    #endregion

    #region UpdateOf BuildingType & ProductionLists
    /// <summary>
    /// Whenever BuildingType changes the nessesary productionList is getting loaded.
    /// </summary>
    /// <param name="buildingType"></param>
    public void UpdateBuildingType(BuildingType buildingType)
    {
        this.buildingType = buildingType;
        UpdateProductionList();
    }

    public void UpdateProductionList()
    {
        ResetProductionQuene();
        UI_ResetProductionQuene();
        productList = ProductionLists.GetProductList(buildingType);
        if (productList == null && buildingType == BuildingType.Storage)
        {
            gameObject.SetActive(false);
        }
        else if (productList == null && buildingType != BuildingType.Storage)
        {
            Debug.LogWarning(transform.parent.name + " Production has Problems in ProductionScript! Please Check Type!" );
        }
        else if (productList != null)
        {
            // Spawn ProductionQuenes & Linking to UI
            CreateAllProductionQuenes();
        }
    }
    #endregion

    #region UI_Functions
    private void UI_ResetProductionQuene()
    {
        // Checks for Children and destorys them! - Anakin Skywalker Style
        if (UI_ProductionContent.childCount > 0)
        {
            foreach (Transform UI_QueneChild in UI_ProductionContent)
            {
                Destroy(UI_QueneChild.gameObject);
            }
            UI_ProductionContent.DetachChildren();
        }
    }

    private void UI_UpdateWorkerNumber()
    {
        UI_WorkerPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = numOverallWorker.ToString() + "\n" + numAvailableWorker.ToString();
    }

    private void UI_UpdateWorkerPrice()
    {
        UI_WorkerPanel.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = newWorkerCost.ToString();
    }

    #endregion
    
    private void CreateAllProductionQuenes()
    {
        if (!UI_ProductQuenePrefab)
        {
            Debug.LogError("Could not assign UI_Elements for " + transform.parent.name);
        }
        else
        {
            // Creates one clickable Icon & Quene for each Product in the List
            foreach (Product product in productList)
            {
                // Create new ProductionQuene 
                ProductionQuene _ProductionQuene = _Instantiate_ProductionQuene(product);
                // Create ProductionQuene UI
                UI_ProductionQuene _UI_ProductionQuene = _Instantiate_UI_ProductionQuene(product);

                // Init UI_ProductionQuene & ProductionQuene by linking each other
                _ProductionQuene.Init(_UI_ProductionQuene, product, _BuildingStorage, this);
                _UI_ProductionQuene.Init(_ProductionQuene, product, this);
                
            }

            UI_ContentHeightUpdater.UpdateHeight();
        }
    }
    private ProductionQuene _Instantiate_ProductionQuene(Product product)
    {
        const string ProductionQueneName = "ProductionQuene (";
        GameObject ProductionQuene = Instantiate(_ProductQuenePrefab, this.transform);
        ProductionQuene.name = ProductionQueneName + productList.IndexOf(product) + ")";
        ProductionQuene _ProductionQueneScriptRef = ProductionQuene.GetComponent<ProductionQuene>();

        // Adds ProductSlot into List<ProductionQuene> to be referenced
        _ProductionQueneList.Add(ProductionQuene);

        return _ProductionQueneScriptRef;
    }
    private UI_ProductionQuene _Instantiate_UI_ProductionQuene(Product product)
    {
        const string UI_ProductionQueneName = "ProductQueneUI (";
        GameObject UI_ProductQuene = Instantiate(UI_ProductQuenePrefab, UI_ProductionContent);
        UI_ProductQuene.name = UI_ProductionQueneName + productList.IndexOf(product) + ")";

        // Sets Slider 
        Slider _slider = UI_ProductQuene.transform.GetChild(0).GetChild(2).GetComponent<Slider>();
        _slider.value = 0;
        _slider.maxValue = product.NeededProductionTime;
        _slider.minValue = 0;

        // Adds ProductSlot into List<ProductionSlot> to be referenced
        UI_ProductionQueneList.Add(UI_ProductQuene);

        return UI_ProductQuene.GetComponent<UI_ProductionQuene>();
    }

    // Checks for any Quene as Child of this transform and destroys them
    private void ResetProductionQuene()
    {
        if (this.transform.childCount > 0)
        {
            foreach (Transform Quene_Child in this.transform)
            {
                Destroy(Quene_Child.gameObject);
            }
            transform.DetachChildren();
        }
    }

    public void OnClick()
    {
        // Display Production.GUI
        if (!_ProductionScreen.gameObject.activeSelf)
            _ProductionScreen.gameObject.SetActive(true);
    }

}
