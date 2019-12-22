using UnityEngine;

/// <summary>
/// Links UI_Slot to ProductionSlot. 
/// </summary>

public class UI_ProductionQuene : MonoBehaviour
{
    [Header("Debug_Values::")]
    [SerializeField] Product _Product;
    [SerializeField] ProductionManager _ProductionManagerReference;
    [SerializeField] ProductionQuene _ProductionQueneReference;

    public void Init(ProductionQuene productionQueneRef, Product product, ProductionManager productionManagerRef)
    {
        _Product = product;
        _ProductionManagerReference = productionManagerRef;
        _ProductionQueneReference = productionQueneRef;
    }

}
