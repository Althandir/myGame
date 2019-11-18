using UnityEngine;

/// <summary>
/// Links UI_Slot to ProductionSlot. 
/// </summary>

public class ProductionSlot : MonoBehaviour
{
    [Header("Debug_Values::")]
    [SerializeField] ProductionScript productionReference;

    public ProductionScript ProductionReference { get => productionReference; set => productionReference = value; }

    #region Get&Set for NumWorker to be used in Buttons
    public int GetNumWorker()
    {
        return productionReference.NumWorker;
    }
    public void AddWorker()
    {
        productionReference.NumWorker += 1;
    }
    public void DecreaseWorker()
    {
        productionReference.NumWorker -= 1;
    }
    #endregion
}
