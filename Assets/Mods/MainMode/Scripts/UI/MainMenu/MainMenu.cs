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
using MainMode;

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
    [SerializeField] private GameSwitcher _gameSwitcher;
    [SerializeField] private PlayerUIBinder _binderUI;

    private Player _player;
    private PlayerInventory _playerInventory;
    private Animator _animator;

    private bool _isRun = false;

    public event Action PlayGameAction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        ShowEvent();
    }
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
        _playerInventory = _player.GetComponent<PlayerInventory>();
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
            _backGround.enabled = false;
            var playerConfig = GetPlayerType();
            var player = playerConfig.Create<PlayerBehaviour>();
            var abillity = playerConfig.AddAbillity();
            Bind(player, abillity);
            SetItems(playerConfig);
            if (PlayGameAction != null)
                PlayGameAction();
            Hide();
        }
    }

    private void Bind(PlayerBehaviour player, PlayerBaseAbillitySet abillity)
    {
        _binderUI.UnBindAbillityUI();
        _binderUI.UnBindHealthPlayerUI();
        _binderUI.BindHealthPlayerUI(player);
        _binderUI.BindAbillityUI(abillity);
        _player.SetBehaviour(player);
        _player.SetAbillity(abillity);
        _gameSwitcher.SetPlayerBehaviour(player);
    }
    private void SetItems(PlayerInfo config)
    {
        var artifact = _artifactItemScroller.GetSelectItem().Create<Item>();
        var consumable = _consumableItemScroller.GetSelectItem().Create<ConsumablesItem>();
        artifact.ResetState();
        consumable.ResetState();
        _playerInventory.PickItem(artifact);
        _playerInventory.PickItem(consumable);
        if (_ending)
            _ending.SetUseItems(artifact.Name, consumable.Name, config.Name);
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
