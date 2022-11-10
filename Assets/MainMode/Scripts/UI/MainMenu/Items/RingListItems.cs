using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UserIntaface.MainMenu;

public class RingListItems:BaseRingList<ItemView>
{
    public RingListItems(List<ItemView> elementList, List<RectTransform> movingPoints,Transform parentElements) : base(elementList, movingPoints,parentElements)
    {
        AlignElements();        
    }

    public override void RotateRight()
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
        _ringList.Selected.Next.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Count - 1]);
        _countMovingElements = rotateItems.Count;

        for (int i = 0; i < rotateItems.Count; i++)
        {
            rotateItems[i].MoveTo(_pointPlaces[i].localPosition, CountMovingItemsCallback);
        }
        _ringList.RotateRight();
    }
    public override void RotateLeft()
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

        _countMovingElements = rotateItems.Count;

        _ringList.Selected.Previous.Previous.Data.JumpTo(_pointPlaces[0]);

        for (int i = 0; i < rotateItems.Count; i++)
        {
            rotateItems[i].MoveTo(_pointPlaces[i + 1].localPosition, CountMovingItemsCallback);
        }
        _ringList.RotateLeft();
    }

    protected override void AlignElements()
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
        _ringList.Selected.Next.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Count - 1]);

    }
}

