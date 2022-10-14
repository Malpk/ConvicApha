using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (!IsBusy)
        {
            item.SetPosition(Position);
            _item = item;
        }
    }
    public void Delete()
    {
        if(_item)
            MonoBehaviour.Destroy(_item.gameObject);
        _item = null;
    }
}