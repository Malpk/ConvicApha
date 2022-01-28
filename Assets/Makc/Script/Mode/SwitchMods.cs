using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Underworld;
using Zenject;

namespace SwitchMode
{
    public class SwitchMods : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private List<SettingSequence> _sequences = new List<SettingSequence>();

        [Inject] private GameMap _map;
        [Inject] private TridentSetting _trident;
        [Inject] private CameraAnimation _cameraAnimation;
        [Inject] private Tilemap _tileMap;

        private List<SettingSequence> _curretList = new List<SettingSequence>();

        public GameMap map => _map;
        public Transform playerTransform => _player;
        public TridentSetting trident => _trident;
        public Tilemap tileMap => _tileMap;
        public GameObject curreqSequence { get; private set; }


        private void OnEnable()
        {
            _cameraAnimation.CompliteAction += StartCorotine;
        }
        private void OnDisable()
        {
            _cameraAnimation.CompliteAction -= StartCorotine;
        }
        private void StartCorotine()
        {
            StartCoroutine(RunSwitchMode());
        }

        private IEnumerator RunSwitchMode()
        {
            while (true)
            {
                if (_curretList.Count > 0)
                {
                    var sequence = GetMode().sequence;
                    var instateSequence = Instantiate(sequence, Vector3.zero, Quaternion.identity);
                    instateSequence.transform.parent = transform;
                    curreqSequence = instateSequence;
                    IntializateSequence(instateSequence.GetComponents<ISequence>());
                    yield return new WaitWhile(() => (instateSequence != null));
                }
                else 
                {
                    _curretList = GetList();
                }
            }
        }
        private void IntializateSequence(ISequence[] list)
        {
            foreach (var sequence in list)
            {
                sequence.Constructor(this);
            }
        }
        private SettingSequence GetMode()
        {
            if (_curretList.Count > 0)
            {
                var index = Random.Range(0, _curretList.Count);
                var sequence = _curretList[index];
                _curretList.Remove(sequence);
                return sequence;
            }
            else
            {
                return null;
            }
        }
        private List<SettingSequence> GetList()
        {
            var list = new List<SettingSequence>();
            foreach (var sequence in _sequences)
            {
                list.Add(sequence);
            }
            return list;
        }

    }
}