using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MainMode.GameInteface;

namespace MainMode.Mode1921
{
    public class Mode1921 : MonoBehaviour
    {
        [Header("General Seting")]
        [SerializeField] private bool _playOnStart = true;
        [Header("Requred Reference")]
        [SerializeField] private MapGrid _mapGrid;
        [SerializeField] private ShieldQuest _shieldSpawner;
        [SerializeField] private GeneralSpawner[] _spawners;
        [Header("Events")]
        [SerializeField] public UnityEvent Win;

        public bool IsPlay { get; private set; }

        public void Intializate(ChangeTest test)
        {
            if (_shieldSpawner)
                OnDisable();
            foreach (var spawner in _spawners)
            {
                spawner.Intializate(_mapGrid);
                if (spawner is ShieldQuest shieldSpawner)
                    _shieldSpawner = shieldSpawner;
            }
            _shieldSpawner.Intializate(test);
            OnEnable();
        }
        private void OnEnable()
        {
            if(_shieldSpawner)
                _shieldSpawner.ConpliteQuestAction += CompliteShieldQuest;
        }
        private void OnDisable()
        {
            if (_shieldSpawner)
                _shieldSpawner.ConpliteQuestAction -= CompliteShieldQuest;
        }
        private void Start()
        {
            if (_playOnStart)
                Play();
        }

        public void Play()
        {
            if (!IsPlay)
            {
                IsPlay = true;
                StartCoroutine(PlayToReady());
            }
        }
        public void Stop()
        {
            if (IsPlay)
            {
                foreach (var spawner in _spawners)
                {
                    spawner.Stop();
                }
            }
        }
        public void Restart()
        {
            _mapGrid.ClearMap();
            foreach (var spawner in _spawners)
            {
                spawner.Replay();
            }
        }

        private IEnumerator PlayToReady()
        {
            yield return new WaitWhile(() =>
            {
                foreach (var spawner in _spawners)
                {
                    if (!spawner.IsRedy)
                        return true;
                }
                return false;
            });
            foreach (var spawner in _spawners)
            {
                spawner.Play();
            }
        }


        private void CompliteShieldQuest()
        {
            Win.Invoke();
        }
    }
}