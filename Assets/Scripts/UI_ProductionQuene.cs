using UnityEngine;

/// <summary>
/// Links UI_Slot to ProductionSlot. 
/// </summary>

public class UI_ProductionQuene : MonoBehaviour
{
    [Header("Debug_Values::")]
    [SerializeField] ProductionScript productionReference;

    public ProductionScript ProductionReference { get => productionReference; set => productionReference = value; }
    


}
