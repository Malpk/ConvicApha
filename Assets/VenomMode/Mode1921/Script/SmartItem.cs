using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartItem : MonoBehaviour
{
    public bool IsShow { get; private set; } = false;

    public event System.Action HideItemAction;
    public event System.Action ShowItemAction;

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void ShowItem()
    {
#if UNITY_EDITOR
        if (IsShow)
            throw new System.Exception("item is already show");
#endif
        IsShow = true;
        if (ShowItemAction != null)
            ShowItemAction();
    }
    public void HideItem()
    {
#if UNITY_EDITOR
        if (!IsShow)
            throw new System.Exception("item is already hide");
#endif
        IsShow = false;
        if (HideItemAction != null)
            HideItemAction();
    }
}