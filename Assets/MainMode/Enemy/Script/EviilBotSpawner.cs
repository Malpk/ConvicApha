using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

        private bool _isPlay;
        private bool _isPause;
        private string _evilBotKeyLoad = "EvilBot_MainMode";
        private EvilRobot _perfab;
        private EvilRobot _evilBot;
        public bool IsReady { private set; get; }

        private async void OnEnable()
        {
            var task = Addressables.LoadAssetAsync<GameObject>(_evilBotKeyLoad).Task;
            await task;
            if (task.Result.TryGetComponent(out EvilRobot bot))
                _perfab = bot;
            else
                throw new System.NullReferenceException();
            IsReady = true;
        }
        private void OnDisable()
        {
            Addressables.Release<GameObject>(_perfab.gameObject);
        }
        public void Intitlizate(Player target, MapGrid mapGrid)
        {
            _target = target;
            _mapGrid = mapGrid;
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
            _isPlay = false;
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
            var bot = SpawnBot();
            yield return null;
            while (_isPlay)
            {
                ActivateBot();
                yield return new WaitWhile(() => _evilBot.IsActive && _isPlay);
                yield return WaitTime(_spawnDelay);
            }
            Destroy(bot.gameObject);
        }

        private IEnumerator WaitTime(float timeActive)
        {
            var progress = 0f;
            while (progress < 1f)
            {
                yield return new WaitWhile(() => _isPause);
                progress += Time.deltaTime / timeActive;
                yield return null;
            }
        }

        private EvilRobot SpawnBot()
        {
            _evilBot = Instantiate(_perfab.gameObject, Vector3.zero,
                Quaternion.Euler(Vector3.zero)).GetComponent<EvilRobot>();
            _evilBot.SetTarget(_target);
            return _evilBot;
        }
        private void ActivateBot()
        {
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