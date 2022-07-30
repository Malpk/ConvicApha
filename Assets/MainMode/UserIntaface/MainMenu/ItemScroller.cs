using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScroller : MonoBehaviour
{
    [SerializeField] protected Button _leftBtn;
    [SerializeField] protected Button _rightBtn;
    [SerializeField] private List<ItemView> _items;
    [SerializeField] private RectTransform[] _transforms; 
    private RingListItems _ringList;

    public ItemView SelectedItem { get => _ringList.ItemSelected;}

    private void Awake()
    {   
        _leftBtn.onClick.AddListener(Previous);
        _rightBtn.onClick.AddListener(Next);
        _ringList = new RingListItems(_items, _transforms);
    }

    public void Next()
    {
        Debug.Log("next");                     
        _ringList.RotateRight();
    }

    public void Previous()
    {
        Debug.Log("previ");      
       _ringList.RotateLeft();    
    }

    private void OnDestroy()
    {
        _leftBtn.onClick.RemoveListener(Previous);
        _rightBtn.onClick.RemoveListener(Next);
    }

}
