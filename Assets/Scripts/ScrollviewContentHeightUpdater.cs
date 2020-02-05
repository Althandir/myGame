using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(VerticalLayoutGroup))]
public class ScrollviewContentHeightUpdater : MonoBehaviour
{
    [SerializeField] RectTransform Content_rt;
    [SerializeField] VerticalLayoutGroup verticalLayoutGroup;

    void CheckVariables()
	{
        if (!Content_rt)
        {
            Content_rt = GetComponent<RectTransform>();
        }
        if (!verticalLayoutGroup)
        {
            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }
	}

    public void UpdateHeight()
    {
        CheckVariables();
        ResetHeight();
        foreach (Transform tf in transform)
        {
            if (tf.GetComponent<RectTransform>() && tf.gameObject.activeSelf)
            {
                Content_rt.sizeDelta += IncreaseHeight(tf.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
    }

    private void ResetHeight()
    {
        // TODO: Reset seems not to work. Value in Updateheight is still the same.
        Content_rt.sizeDelta = Vector2.zero;
    }

    Vector2 IncreaseHeight(float prefabHeight)
    {
        return new Vector2(0, (verticalLayoutGroup.spacing * 1.5f) + prefabHeight);
    }

}
