using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UserIntaface.MainMenu;

public class RingListCharacter : BaseRingList<CharacterView>
{

    public RingListCharacter(List<CharacterView> characterList, List<RectTransform> points, Transform parentElements) : base(characterList, points, parentElements)
    {
        AlignElements();
    }

    public override void RotateRight()
    {
        if (IsRotating)
            return;

        IsRotating = true;

        List<CharacterView> rotateItems = new List<CharacterView>()
        {
           _ringList.Selected.Data,
           _ringList.Selected.Next.Data
        };

        _countMovingElements = rotateItems.Count;

        _ringList.Selected.Next.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Count - 1]);

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

        List<CharacterView> rotateItems = new List<CharacterView>()
        {
           _ringList.Selected.Previous.Data,
           _ringList.Selected.Data
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
        _ringList.Selected.Data.JumpTo(_pointPlaces[_pointPlaces.Count / 2]);

        var select = new CharacterView[] { _ringList.Selected.Data };

        var hideItems = _ringList.Except(select).ToList();

        foreach (var item in hideItems)
        {
            item.JumpTo(_pointPlaces[0]);
        }
        _ringList.Selected.Next.Data.JumpTo(_pointPlaces[_pointPlaces.Count - 1]);
    }

}

