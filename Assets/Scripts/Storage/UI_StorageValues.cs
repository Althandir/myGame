using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StorageValues : MonoBehaviour
{
    [SerializeField] TMP_Text _actAmountText;
    [SerializeField] TMP_Text _maxAmountText;
    [SerializeField] TMP_Text _storageLevel;
    public void UpdateActAmountText(int value)
    {
        Debug.Log("Update ActAmount called in Values with " + value.ToString() + "!");
        _actAmountText.text = value.ToString();
    }

    public void UpdateMaxAmountText(int value)
    {
        _maxAmountText.text = value.ToString();
    }

    public void UpdateLevelText(int value)
    {
        _storageLevel.text = value.ToString();
    }
}
