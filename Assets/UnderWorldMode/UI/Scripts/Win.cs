using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
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

    private Animator _animator;
    private string _saveName = "SaveData.json";

    public delegate void BlackScreen();
    public event BlackScreen BlackScreenAction;

    private string Path => Application.streamingAssetsPath + _saveName;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void WinAniamtion()
    {
        _animator.SetTrigger("start");
    }
    private void OnDeactiveCanvas()
    {
        _titleCanvas.enabled = false;
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
