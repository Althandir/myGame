using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStorageSlider : MonoBehaviour
{
    [Header("Debug_Values:")]
    [SerializeField]
    Slider _slider = null;
    [SerializeField]
    Image _bar = null;
    /*
    [SerializeField]
    bool isInitialized = false;
    */
    void Awake()
    {
        _slider = this.gameObject.GetComponent<Slider>();
        _slider.enabled = false;
        _bar = this.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    public void Init(byte minValue, byte maxValue)
    {
        _slider.minValue = minValue;
        _slider.maxValue = maxValue;
    }

    public void UpdateSlider(byte amount)
    {
        _slider.value = amount;
        if (amount < 25)
        {
            _bar.color = Color.red;
        }
        else if (amount >= 25 && amount <= 75)
        {
            _bar.color = Color.cyan;
        } 
        else if (amount > 75)
        {
            _bar.color = Color.green;
        }

        if (amount == 0)
        {
            _slider.enabled = false;
        }
        else
        {
            _slider.enabled = true;
        }
    }

}
