using UnityEngine;
using Zenject;

namespace Underworld
{
    [RequireComponent(typeof(AudioSource))]
    public class SourceUnderWorld : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;

        [Inject] private CameraAnimation _cameraAnimation;
        [Inject] private UnderWorldEvent _gameEvent;
        [Inject] private Win _win;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.loop = true;
            _source.playOnAwake = false;
            _source.clip = _audioClip;
        }
        private void OnEnable()
        {
            _cameraAnimation.CompliteAction += OnPlaySound;
            _gameEvent.DeadAction += OnStopSound;
            _win.BlackScreenAction += OnStopSound;
        }
        private void OnDisable()
        {
            _cameraAnimation.CompliteAction -= OnPlaySound;
            _gameEvent.DeadAction -= OnStopSound;
            _win.BlackScreenAction -= OnStopSound;
        }
        private void OnPlaySound()
        {
            _source.Play();
        }
        private void OnStopSound()
        {
            _source.Pause();
        }
    }
}