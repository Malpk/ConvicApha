using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserIntaface.MainMenu
{
    public class BaseScroller<T> : MonoBehaviour where T : BaseScrollElementView
    {
        [SerializeField] protected Button _leftBtn;
        [SerializeField] protected Button _rightBtn;

        [SerializeField] protected List<RectTransform> _transforms;
        [SerializeField] protected List<T> _listPrefabs;
        [SerializeField] protected Transform _parentElements;

        protected BaseRingList<T> _ringList;
        public T SelectedElement { get => _ringList.SelectedElement; }

        protected virtual void Awake()
        {
            _leftBtn.onClick.AddListener(Previous);
            _rightBtn.onClick.AddListener(Next);
        }    

        public virtual void Next()
        {
            _ringList.RotateRight();
        }

        public virtual void Previous()
        {
            _ringList.RotateLeft();
        }

        protected virtual void OnDestroy()
        {
            _leftBtn.onClick.RemoveListener(Previous);
            _rightBtn.onClick.RemoveListener(Next);
        }
    }
}
