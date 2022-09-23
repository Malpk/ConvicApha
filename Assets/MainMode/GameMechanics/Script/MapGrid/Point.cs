using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

public class Point
{
    private SmartItem _item;

    public bool IsBusy => _item ? _item.IsShow : false;
    public Vector2 Position { get; private set; }

    public Point(Vector2 position)
    {
        Position = position;
    }

    public void SetItem(SmartItem item)
    {
#if UNITY_EDITOR
        if (IsBusy)
            throw new System.Exception("this point is already busy");
#endif
        item.SetPosition(Position);
        _item = item;
    }
    public void ResetPoint()
    {
        _item = null;
    }
}