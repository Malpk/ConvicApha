using MainMenu.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UserIntaface.MainMenu;

public class RingListCharacter
{
    private List<RectTransform> _pointPlaces;
    private RingList<CharacterView> _ringList;
    public bool IsRotating = false;
    private int _countMovingItems;
    public CharacterView ItemSelected { get => _ringList.Selected.Data; }
    public RingListCharacter(List<CharacterView> characterList, List<RectTransform> points)
    {
        if (characterList != null && points.Count > 0)
        {
            _pointPlaces = points;
            _ringList = new RingList<CharacterView>(characterList);
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
           _ringList.Selected.Data,
           _ringList.Selected.Next.Data        
        };

        _countMovingItems = rotateItems.Count;

        _ringList.Selected.Next.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Count- 1]);

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
           _ringList.Selected.Previous.Data,
           _ringList.Selected.Data         
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
        _ringList.Selected.Data.JumpTo(_pointPlaces[_pointPlaces.Count / 2]);

        var select = new ItemView[] {_ringList.Selected.Data };

        var hideItems = _ringList.Except(select).ToList();

        foreach (var item in hideItems)
        {
            item.JumpTo(_pointPlaces[0]);
        }
        _ringList.Selected.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Count - 1]);
    }

}

