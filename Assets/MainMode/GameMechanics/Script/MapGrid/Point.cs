using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

public class Point
{
    private SpawnItem _item;

    public bool IsBusy => _item ? _item.IsShow : false;
    public Vector2 Position { get; private set; }

    public Point(Vector2 position)
    {
        Position = position;
    }

    public void SetItem(SpawnItem item)
    {
        if (_item != null)
            RemoveObject();
        item.SetPosition(Position);
        _item = item;
    }
    public void RemoveObject()
    {
        if (_item)
        {
            _item.SetMode(false);
            _item = null;
        }
    }
}