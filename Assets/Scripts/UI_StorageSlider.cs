using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StorageSlider : MonoBehaviour
{
    [Header("Debug_Values:")]
    [SerializeField]
    Slider _slider = null;
    [SerializeField]
    Image _bar = null;
    [SerializeField]
    byte _quarterOfMax;
    [SerializeField]
    byte _threeQuarterOfMax;

    public void Init(byte minValue, byte maxValue)
    {
        _slider = this.gameObject.GetComponent<Slider>();
        _bar = this.transform.GetChild(1).GetChild(0).GetComponent<Image>();

        _slider.minValue = minValue;
        _slider.maxValue = maxValue;
        _slider.value = 0;

        _quarterOfMax = (byte) (maxValue / 4);
        _threeQuarterOfMax = (byte)((maxValue / 4) * 3);
    }

    public void UpdateSlider(byte amount)
    {
        _slider.value = amount;
        if (_slider != null && _bar != null)
        {
            if (amount != 0)
            {
                if (amount < _quarterOfMax)
                {
                    _bar.color = Color.red;
                }
                else if (amount >= _quarterOfMax && amount <= _threeQuarterOfMax)
                {
                    _bar.color = Color.cyan;
                }
                else if (amount > _threeQuarterOfMax)
                {
                    _bar.color = Color.green;
                }
            }
        }
    }

}
