using MainMenu.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserIntaface.MainMenu;

public class CharacterScroller : MonoBehaviour
{
    [SerializeField] protected Button _leftBtn;
    [SerializeField] protected Button _rightBtn;
    [SerializeField] private List<CharacterView> _players;
    [SerializeField] private List<RectTransform> _transforms;

    public Action<DescriptionCharacter> ChangeCharacter;

    private RingListCharacter _ringList;
    public CharacterView SelectedPlayer { get => _ringList.ItemSelected; }
    private void Awake()
    {
        _leftBtn.onClick.AddListener(Previous);
        _rightBtn.onClick.AddListener(Next);
        _ringList = new RingListCharacter(_players, _transforms);
    }
    private void Start()
    {
        ChangeCharacter?.Invoke(SelectedPlayer.Description);
    }
    public void Next()
    {
        Debug.Log("next");
        _ringList.RotateRight();
        ChangeCharacter?.Invoke(SelectedPlayer.Description);
    }

    public void Previous()
    {
        Debug.Log("previ");
        _ringList.RotateLeft();
        ChangeCharacter?.Invoke(SelectedPlayer.Description);
    }

    private void OnDestroy()
    {
        _leftBtn.onClick.RemoveListener(Previous);
        _rightBtn.onClick.RemoveListener(Next);
    }
}
