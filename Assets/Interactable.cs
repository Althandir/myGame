using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    enum InteractableType
    {
        undefined, Storage, Production
    }

    [Header("DEBUG_VALUES:")]
    [SerializeField]
    InteractableType _type;

    void Awake()
    {
        if (this.gameObject.GetComponent<StorageScript>())
        {
            _type = InteractableType.Storage;
        }
        else if (this.gameObject.GetComponent<ProductionScript>())
        {
            _type = InteractableType.Production;
        }
        else
        {
            _type = InteractableType.undefined;
        }
    }

    public void OnClick()
    {
        switch (_type)
        {
            case InteractableType.Storage:
                {
                    GetComponent<StorageScript>().OnClick();
                    break;
                }
            case InteractableType.Production:
                {
                    GetComponent<ProductionScript>().OnClick();
                    break;
                }
            case InteractableType.undefined:
            default:
                {
                    Debug.LogError("Could not define Interaction type of " + ToString() + " in " + transform.parent.name + "!");
                    break;
                }
        }
    }
}
