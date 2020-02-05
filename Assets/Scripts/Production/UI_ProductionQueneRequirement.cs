using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ProductionQueneRequirement : MonoBehaviour
{
    [Header("Debug_Values:")]
    [SerializeField] Image _ProductIcon;
    [SerializeField] Image _StatusIcon;
    [SerializeField] TMP_Text _ReqAmountText;

    public Image ProductIcon { get => _ProductIcon; set => _ProductIcon = value; }
    public Image StatusIcon { get => _StatusIcon; set => _StatusIcon = value; }
    public TMP_Text ReqAmountText { get => _ReqAmountText; set => _ReqAmountText = value; }
}
