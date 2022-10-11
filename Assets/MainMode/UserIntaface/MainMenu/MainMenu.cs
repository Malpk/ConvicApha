using MainMode.LoadScene;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using MainMode.GameInteface;

public sealed class MainMenu : UserInterface
{
    [Header("Reference")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RawImage _backGround;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private BaseLoader _sceneLoader;
    [Header("Player Config")]
    [SerializeField] private ItemScroller _characterScroller;
    [SerializeField] private ItemScroller _artifactItemScroller;
    [SerializeField] private ItemScroller _consumableItemScroller;

    private Animator _animator;

    private bool _isRun = false;
    private PlayerConfig _config = new PlayerConfig();


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

    public void CreateNewConfig()
    {
        if (!_isRun)
        {
            _isRun = true;
            _config.SetArtifactAsync(_artifactItemScroller.GetSelectItem());
            _config.SetConsumableAsync(_consumableItemScroller.GetSelectItem());
            _config.SetPlayerType(GetPlayerType());
            _sceneLoader.Load(_config);
            _sceneLoader.Play();
            _backGround.enabled = false;
            Hide();
        }
    }

    private PlayerType GetPlayerType()
    {
        if (_characterScroller.GetSelectItem() is PlayerInfo player)
            return player.Type;
        return PlayerType.None;
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
}
