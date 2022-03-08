using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Underworld;
using Zenject;

namespace SwitchModeComponent
{
    public class SwitchMode : MonoBehaviour
    {
        [Header("Perfab Setting")]
        [SerializeField] private Transform _player;
        [SerializeField] private MapBuilder _builder;
        [SerializeField] private List<SettingSequence> _sequences = new List<SettingSequence>();

        [Inject] private GameMap _map;
        [Inject] private CameraAnimation _cameraAnimation;
        [Inject] private Tilemap _tileMap;

        private List<SettingSequence> _curretList = new List<SettingSequence>();

        public bool isAttackMode => GetSequenceStatus(curreqSequence);
        public GameMap map => _map;
        public Transform playerTransform => _player;
        public Tilemap tileMap => _tileMap;
        public GameObject curreqSequence { get; private set; }
        public MapBuilder builder => _builder;
        
        private void OnEnable()
        {
            _cameraAnimation.CompliteAction += StartCorotine;
        }
        private void OnDisable()
        {
            _cameraAnimation.CompliteAction -= StartCorotine;
        }
        private void Start()
        {
            _builder.Intializate(_map.Map, transform);
        }
        private void StartCorotine()
        {
            StartCoroutine(RunSwitchMode());
        }
        private bool GetSequenceStatus(GameObject curretSecunce)
        {
            if (curretSecunce == null)
                return false;
            if (curretSecunce.TryGetComponent<IModeForSwitch>(out IModeForSwitch sequnce))
                return sequnce.IsAttackMode;
            else
                return false;
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
                    IntializateSequence(instateSequence.GetComponents<IModeForSwitch>());
                    yield return new WaitWhile(() => (instateSequence != null));
                }
                else 
                {
                    _curretList = GetList();
                }
            }
        }
        private void IntializateSequence(IModeForSwitch[] list)
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