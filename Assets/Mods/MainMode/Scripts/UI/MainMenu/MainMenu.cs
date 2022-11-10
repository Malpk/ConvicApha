using MainMode.LoadScene;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using MainMode.GameInteface;
using Zenject;
using PlayerComponent;
using MainMode.Items;
using System;
using TMPro;

public sealed class MainMenu : UserInterface
{
    [Header("Reference")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RawImage _backGround;
    [SerializeField] private VideoPlayer _videoPlayer;
    [Header("Player Config")]
    [SerializeField] private Ending _ending;
    [SerializeField] private ItemScroller _characterScroller;
    [SerializeField] private ItemScroller _artifactItemScroller;
    [SerializeField] private ItemScroller _consumableItemScroller;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desctiption;

    private Player _player;
    private Animator _animator;
    private HUDInteface _hud;

    private bool _isRun = false;

    public event Action PlayGameAction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        ShowEvent();
    }
    [Inject]
    public void Construct(Player player, HUDInteface hud)
    {
        _player = player;
        _hud = hud;
    }

    private void OnEnable()
    {
        ShowAction += ShowEvent;
        HideAction += () => _animator.SetBool("ShiftPanels", true);
        _characterScroller.OnSelectItem += SetDescription;
        _artifactItemScroller.OnSelectItem += SetDescription;
        _consumableItemScroller.OnSelectItem += SetDescription;
    }

    private void OnDisable()
    {
        ShowAction -= ShowEvent;
        HideAction -= () => _animator.SetBool("ShiftPanels", true);
        _characterScroller.OnSelectItem -= SetDescription;
        _artifactItemScroller.OnSelectItem -= SetDescription;
        _consumableItemScroller.OnSelectItem -= SetDescription;
    }

    public void CreateNewConfig()
    {
        if (!_isRun)
        {
            _isRun = true;
            var artifact = _artifactItemScroller.GetSelectItem().Create<Item>();
            var consumable = _consumableItemScroller.GetSelectItem().Create<ConsumablesItem>();
            _player.PickItem(artifact);
            _player.PickItem(consumable);
            _player.SetBehaviour(GetPlayerType().Create<PlayerBaseBehaviour>());
            _backGround.enabled = false;
            Hide();
            if (PlayGameAction != null)
                PlayGameAction();
            if(_ending)
                _ending.SetUseItems(artifact.Name , consumable.Name, _player.Name);
        }
    }

    private PlayerInfo GetPlayerType()
    {
        if (_characterScroller.GetSelectItem() is PlayerInfo player)
            return player;
        return null;
    }

    private void ShowEvent()
    {
        _isRun = false;
        _videoPlayer.Play();
        _canvas.enabled = true;
        _backGround.enabled = true;
        _animator.SetBool("ShiftPanels", false);
    }

    private void HideAnimationEvent()
    {
        _canvas.enabled = false;
        _backGround.enabled = false;
        _videoPlayer.Stop();
    }

    private void SetDescription(string description, string name)
    {
        _desctiption.text = description;
        _name.text = name;
    }
}
