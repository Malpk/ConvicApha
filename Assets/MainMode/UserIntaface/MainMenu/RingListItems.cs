﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UserIntaface.MainMenu;

public class RingListItems
{
    private RectTransform[] _pointPlaces;
    private RingList<ItemView> _ringList;
    public bool IsRotating = false;
    private int _countMovingItems;
    public ItemView ItemSelected { get => _ringList.Selected.Data; }
    //points 5 при выборе предметов, 3 при выборе персонажей, items-сколько угодно в обоих случаях
    public RingListItems(List<ItemView> itemList, RectTransform[] points)
    {
        if (itemList != null && points.Length > 0)
        {
            _pointPlaces = points;
            _ringList = new RingList<ItemView>(itemList);
            AlignItems();
        }
    }

    public void RotateRight()
    {
        if (IsRotating)
            return;

        IsRotating = true;

        List<ItemView> rotateItems = new List<ItemView>()
        {
           _ringList.Selected.Previous.Data,
           _ringList.Selected.Data,
           _ringList.Selected.Next.Data,
           _ringList.Selected.Next.Next.Data
        };
        _ringList.Selected.Next.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Length - 1]);
        _countMovingItems = rotateItems.Count;

        for (int i = 0; i < rotateItems.Count; i++)
        {
            rotateItems[i].MoveTo(_pointPlaces[i].localPosition, CountMovingItemsCallback);
        }
        _ringList.RotateRight();
    }

    private void CountMovingItemsCallback()
    {
        _countMovingItems--;

        if (_countMovingItems == 0)
            IsRotating = false;
    }

    public void RotateLeft()
    {
        if (IsRotating)
            return;

        IsRotating = true;

        List<ItemView> rotateItems = new List<ItemView>()
        {
           _ringList.Selected.Previous.Previous.Data,
           _ringList.Selected.Previous.Data,
           _ringList.Selected.Data,
           _ringList.Selected.Next.Data
        };

        _countMovingItems = rotateItems.Count;

        _ringList.Selected.Previous.Previous.Data.JumpTo(_pointPlaces[0]);

        for (int i = 0; i < rotateItems.Count; i++)
        {
            rotateItems[i].MoveTo(_pointPlaces[i + 1].localPosition, CountMovingItemsCallback);
        }
        _ringList.RotateLeft();
    }

    private void AlignItems()
    {
        List<ItemView> startItems = new List<ItemView>()
        {
        _ringList.Selected.Previous.Data,
        _ringList.Selected.Data,
        _ringList.Selected.Next.Data
        };

        for (int i = 0; i < startItems.Count; i++)
        {
            startItems[i].JumpTo(_pointPlaces[i + 1]);
        }

        var hideItems = _ringList.Except(startItems).ToList();

        foreach (var item in hideItems)
        {
            item.JumpTo(_pointPlaces[0]);
        }
        _ringList.Selected.Next.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Length - 1]);

    }
}

