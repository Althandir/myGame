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
    [SerializeField]
    MeshRenderer _meshRenderer;
    [SerializeField]
    Color _initColor;

    void Awake()
    {
        if (this.gameObject.GetComponent<StorageScript>())
        {
            _type = InteractableType.Storage;
        }
        else if (this.gameObject.GetComponent<ProductionManager>())
        {
            _type = InteractableType.Production;
        }
        else
        {
            _type = InteractableType.undefined;
        }

        _meshRenderer = this.GetComponent<MeshRenderer>();
        _initColor = _meshRenderer.material.color;
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
                    GetComponent<ProductionManager>().OnClick();
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

    void OnMouseOver()
    {
        _meshRenderer.material.color = Color.cyan;
    }

    void OnMouseExit()
    {
        _meshRenderer.material.color = _initColor;
    }
}
