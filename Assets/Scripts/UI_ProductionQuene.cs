using UnityEngine;

/// <summary>
/// Links UI_Slot to ProductionSlot. 
/// </summary>

public class UI_ProductionQuene : MonoBehaviour
{
    [Header("Debug_Values::")]
    [SerializeField] ProductionScript ProductionManagerReference;
    [SerializeField] ProductionQuene ProductionQueneReference;

    public void Init(ProductionQuene productionQueneRef, ProductionScript productionManagerRef)
    {
        ProductionManagerReference = productionManagerRef;
        ProductionQueneReference = productionQueneRef;
    }

}
