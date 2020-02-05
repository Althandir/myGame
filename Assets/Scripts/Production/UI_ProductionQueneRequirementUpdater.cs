using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_ProductionQueneRequirementUpdater : MonoBehaviour
{
    static int requirementMaxAmount = 4;
    
    [SerializeField] List<ProductionRequirement> _ProductionRequirements;
    [SerializeField] List<UI_ProductionQueneRequirement> UI_Requirements;
    [SerializeField] UI_PQ_RequirementIconContainer iconContainer;

    public void LinkToRequirementList(List<ProductionRequirement> productionRequirements)
    {
        _ProductionRequirements = productionRequirements;
        foreach (Transform UI_Requirement in transform)
        {
            UI_Requirements.Add(UI_Requirement.GetComponent<UI_ProductionQueneRequirement>());
        }

        // Initial Update
        FullRequirementsUIUpdate();
    }

    public void FullRequirementsUIUpdate()
    {
        int index = 0;
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            if (requirement.IsFullfilled)
            {
                UI_Requirements[index].StatusIcon.sprite = iconContainer.fullfilledSprite;
            }
            else
            {
                UI_Requirements[index].StatusIcon.sprite = iconContainer.unfullfilledSprite; 
            }
            UI_Requirements[index].ProductIcon.sprite = requirement.GetProduct().Icon;
            UI_Requirements[index].ReqAmountText.text = requirement.GetRequiredAmount().ToString();
            index += 1;
        }

        for (int i = _ProductionRequirements.Count; i < requirementMaxAmount; i++)
        {
            UI_Requirements[i].gameObject.SetActive(false);
        }

    }

    public void UpdateIcons()
    {
        int index = 0;
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            UI_Requirements[index].ProductIcon.sprite = requirement.GetProduct().Icon;
            index += 1;
        }
    }

    public void UpdateAmountText()
    {
        int index = 0;
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            UI_Requirements[index].ReqAmountText.text = requirement.GetRequiredAmount().ToString();
        }
    }

    public void UpdateStatus()
    {
        int index = 0;
        foreach (ProductionRequirement requirement in _ProductionRequirements)
        {
            if (requirement.IsFullfilled)
            {
                UI_Requirements[index].StatusIcon.sprite = iconContainer.fullfilledSprite;
            }
            else
            {
                UI_Requirements[index].StatusIcon.sprite = iconContainer.unfullfilledSprite;
            }
            index += 1;
        }
    }
}
