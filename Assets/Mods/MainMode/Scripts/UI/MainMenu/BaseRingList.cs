using System;
using System.Collections.Generic;
using UnityEngine;

namespace UserIntaface.MainMenu
{
    public abstract class BaseRingList<T> where T : BaseScrollElementView
    {
        protected List<RectTransform> _pointPlaces;
        protected RingList<T> _ringList;
        protected List<T> _listPrefabs;
        protected int _countMovingElements;
        protected bool IsRotating = false;
        public T SelectedElement { get => _ringList.Selected.Data; }
        public BaseRingList(List<T> elementList, List<RectTransform> movingPoints, Transform parent)
        {
            if (elementList != null && movingPoints.Count > 0)
            {
                _pointPlaces = movingPoints;
                _listPrefabs = new List<T>(elementList);
                _ringList = new RingList<T>();
                InstantiateElements(parent);              
            }
        }

        public virtual void RotateRight()
        {

        }

        public virtual void RotateLeft()
        {

        }
        protected virtual void AlignElements()
        {

        }
        protected void InstantiateElements(Transform parentElements)
        {
            foreach (var item in _listPrefabs)
            {
                _ringList.Add(GameObject.Instantiate(item, parentElements));
            }
        }
        protected void CountMovingItemsCallback()
        {
            _countMovingElements--;

            if (_countMovingElements == 0)
                IsRotating = false;   

        }

    }
}
