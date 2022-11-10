using System;
using UnityEngine;

public abstract class SmartItem : MonoBehaviour
{
    public bool IsShow { get; private set; } = false;

    public event Action HideItemAction;
    public event Action ShowItemAction;

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void ShowItem()
    {
        if (!IsShow)
        {
            IsShow = true;
            if (ShowItemAction != null)
                ShowItemAction();
        }
    }
    public void HideItem()
    {
        if (IsShow)
        {
            IsShow = false;
            if (HideItemAction != null)
                HideItemAction();
        }
    }
}