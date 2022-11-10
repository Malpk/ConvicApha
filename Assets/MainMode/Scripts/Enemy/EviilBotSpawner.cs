using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainMode
{
    public class EviilBotSpawner : MonoBehaviour, IPause
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private Vector2 _spawnDistance;
        [Header("Reference")]
        [SerializeField] private Player _target;
        [SerializeField] private MapGrid _mapGrid;
        [SerializeField] private EvilRobot _evilBot;

        private bool _isPlay;
        private bool _isPause;

        public bool IsReady { private set; get; }
        
        [Inject]
        public void Construct(Player target)
        {
            _target = target;
        }

        private void Start()
        {
            if (_playOnStart)
                Play();
        }

        public void Play()
        {
            if (!_isPlay)
            {
                _isPlay = true;
                StartCoroutine(Work());
            }
        }
        public void Stop()
        {
            if (_isPlay)
            {
                _isPlay = false;
                _evilBot.Deactivate();
                _evilBot.HideItem();
            }
        }
        public void Pause()
        {
            _isPause = true;
        }

        public void UnPause()
        {
            _isPause = false;
        }
        private IEnumerator Work()
        {
            while (_isPlay)
            {
                ActivateBot();
                yield return new WaitWhile(() => _evilBot.IsActive && _isPlay);
                yield return WaitTime(_spawnDelay);
            }
            Stop();
        }

        private IEnumerator WaitTime(float timeActive)
        {
            var progress = 0f;
            while (progress < 1f && _isPlay)
            {
                yield return new WaitWhile(() => _isPause && _isPlay);
                progress += Time.deltaTime / timeActive;
                yield return null;
            }
        }
        private void ActivateBot()
        {
            _evilBot.SetTarget(_target);
            if (_mapGrid.GetFreePoints(out List<Point> points, _spawnDistance.x, _spawnDistance.y))
            {
                _evilBot.transform.position = points[Random.Range(0, points.Count)].Position;
                _evilBot.transform.rotation = Quaternion.Euler(Vector3.zero);
                _evilBot.ShowItem();
                _evilBot.Activate();
            }
        }
    }
}