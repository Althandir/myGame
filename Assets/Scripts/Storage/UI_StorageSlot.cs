using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StorageSlot : MonoBehaviour
{
    [SerializeField] StorageSlot _StorageSlotRef;
    [SerializeField] Image _ProductIcon;
    [SerializeField] TMP_Text _AmountText;
    [SerializeField] ScrollviewContentHeightUpdater ContentHeightUpdater;
    public void Init(StorageSlot storageSlot)
    {
        _StorageSlotRef = storageSlot;

        // TODO: give References directly in Prefab

        _ProductIcon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        _AmountText = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        ContentHeightUpdater = transform.parent.GetComponent<ScrollviewContentHeightUpdater>();

        this.gameObject.name = "UI_StorageSlot (" + _StorageSlotRef.Product.Name + ")";
        
        _ProductIcon.sprite = _StorageSlotRef.Product.Icon;
        _AmountText.text = _StorageSlotRef.Amount.ToString();
    }

    public void UpdateStorageSlotUI()
    {
        _AmountText.text = _StorageSlotRef.Amount.ToString();

        if (_StorageSlotRef.Amount == 0)
        {
            DisableUI();
            ContentHeightUpdater.UpdateHeight();
        }
        else
        {
            EnableUI();
            ContentHeightUpdater.UpdateHeight();
        }
    }

    private void DisableUI()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void EnableUI()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
    }
}
