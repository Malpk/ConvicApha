using MainMode.LoadScene;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using MainMode.GameInteface;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public sealed class MainMenu : UserInterface
{
    [Header("Reference")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RawImage _backGround;
    [SerializeField] private VideoPlayer _player;
    [SerializeField] private BaseLoader _sceneLoader;
    [Header("Player Config")]
    [SerializeField] private ItemScroller _characterScroller;
    [SerializeField] private ItemScroller _artifactItemScroller;
    [SerializeField] private ItemScroller _consumableItemScroller;

    private Animator _animator;

    private bool _isRun = false;

    public override UserInterfaceType Type => UserInterfaceType.MainMenu;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        ShowEvent();
    }

    public void Intializate(BaseLoader loader)
    {
        _sceneLoader = loader;
    }

    private void OnEnable()
    {
        ShowAction += ShowEvent;
        HideAction += () => _animator.SetBool("ShiftPanels", true);
    }

    private void OnDisable()
    {
        ShowAction -= ShowEvent;
        HideAction -= () => _animator.SetBool("ShiftPanels", true); 
    }

    public async void CreateNewConfig()
    {
        _isRun = true;
        var artifactTask = Addressables.InstantiateAsync(_artifactItemScroller.GetSelectItem().LoadKey).Task;
        var consumableItemTask = Addressables.InstantiateAsync(_consumableItemScroller.GetSelectItem().LoadKey).Task;
        await Task.WhenAll(artifactTask, consumableItemTask);
        var config = new PlayerConfig();
        config.SetConfig(consumableItemTask.Result, artifactTask.Result, GetPlayerType());
        await _sceneLoader.LoadAsync(config);
        _sceneLoader.Play();
        _backGround.enabled = false;
        if (swithchInteface)
            swithchInteface.SetHide(this);
        else
            Hide();
    }

    private PlayerType GetPlayerType()
    {
        if (_characterScroller.GetSelectItem() is PlayerInfo player)
            return player.Type;
        return PlayerType.None;
    }

    private void ShowEvent()
    {
        _player.Play();
        _canvas.enabled = true;
        _backGround.enabled = true;
        _animator.SetBool("ShiftPanels", false);
    }

    private void HideAnimationEvent()
    {
        _canvas.enabled = false;
        _backGround.enabled = false;
        _player.Stop();
    }
}
