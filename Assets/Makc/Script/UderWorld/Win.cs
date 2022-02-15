using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Zenject;
using System.IO;
using UnityEngine.Video;
using Underworld;

[RequireComponent(typeof(Animator))]
public class Win : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private int _idLoadScene = 0;
    [SerializeField] private float _loadDelay = 5f;

    [Header("Perfab Setting")]
    [SerializeField] private VideoPlayer _player;
    [SerializeField] private VideoClip _clip;
    [SerializeField] private Canvas _titleCanvas;

    [Inject] private GameEvent _gameEvent;

    private Animator _animator;
    private string _saveName = "SaveData.json";
    private SaveData _data;

    public delegate void BlackScreen();
    public event BlackScreen BlackScreenAction;

    private string Path => Application.streamingAssetsPath + _saveName;

    private void Awake()
    {
        if (!File.Exists(Path))
        {
            _data = new SaveData();
            _data.IsFirstEnter = true;
        }
        else
        {
            var saveData = File.ReadAllText(Path);
            _data = JsonUtility.FromJson<SaveData>(saveData);
        }
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _gameEvent.WinAction += WinAniamtion;
    }
    private void OnDisable()
    {
        _gameEvent.WinAction -= WinAniamtion;
    }
    private void Start()
    {
        if (_gameEvent.TypeEvent == TypeGameEvent.Win)
        {
            _titleCanvas.enabled = true;
            _animator.SetTrigger("newGame");
        }
    }
    private void OnDestroy()
    {
        File.WriteAllText(Path, JsonUtility.ToJson(_data));
    }
    private void WinAniamtion()
    {
        _animator.SetTrigger("start");
    }
    private void OnDeactiveCanvas()
    {
        _titleCanvas.enabled = false;
    }
    private void OnLoadMainMenu()
    {
        if (BlackScreenAction != null)
            BlackScreenAction();
        if (_data.IsFirstEnter)
            StartCoroutine(ShowTitle(_loadDelay));
        else
            SceneManager.LoadScene(_idLoadScene);
    }
    private IEnumerator ShowTitle(float loadDelay)
    {
        yield return new WaitForSeconds(_loadDelay);
        _player.clip = _clip;
        _player.Play();
        var duration = (float)_clip.length;
        yield return new WaitForSeconds(duration);
        Application.Quit();
    }
}
