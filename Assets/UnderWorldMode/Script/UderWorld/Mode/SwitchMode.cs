using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Underworld;
using Zenject;


namespace Underworld
{
    public class SwitchMode : MonoBehaviour
    {
        [Header("Perfab Setting")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;
        [SerializeField] private List<ModeSet> _mods;
        [SerializeField] private ModeSwitchController _controller;

        [Header("Map Setting")]

        [Inject] private CameraAnimation _cameraAnimation;

        private SettingSerilize _curretMode;
        private List<Seqcunce> _curretList = new List<Seqcunce>();

        //public ModeTypeNew Type => _curretMode !=null ? _curretMode.Type: ModeTypeNew.BaseMode;
        public bool isAttackMode => GetSequenceStatus(curreqSequence);
        public Player Player => _player;
        public GameObject curreqSequence { get; private set; }
        public MapBuilder builder => _builder;
        public Vector2 UnitSize => _builder.UnitSize;

        private void Awake()
        {
            _builder.Intializate(transform);
            var clearList = new List<ModeSet>();
            for (int i = 0; i < _mods.Count; i++)
            {
                if (_mods[i].ModeObject != null)
                {
                    _mods[i].ModeObject = Instantiate(_mods[i].ModeObject, Vector3.zero, Quaternion.identity);
                    _mods[i].ModeObject.transform.parent = transform;
                    _mods[i].ModeObject.SetActive(false);
                    clearList.Add(_mods[i]);
                }
            }
            _mods.Clear();
            _mods = clearList;
        }
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
            Seqcunce sequnce = null;
            UpdateList(sequnce);
            if (_curretList.Count > 0)
            {
                sequnce = _curretList[Random.Range(0, _curretList.Count)];
                for (int i = 0; i < sequnce.SettingSequnce.Count; i++)
                {
                    var mode = DefineMode(sequnce.SettingSequnce[i].Type);
                    if (mode != null)
                    {
                        _curretMode = sequnce.SettingSequnce[i];
                        mode.SetActive(true);
                        if (mode.TryGetComponent<IModeForSwitch>(out IModeForSwitch switchMode))
                        {
                            switchMode.SetSetting(_curretMode.Setting);
                            switchMode.Constructor(this);
                            yield return new WaitWhile(() => switchMode.IsAttackMode);
                        }
                        mode.SetActive(false);
                    }
                    yield return null;
                }
            }
                yield return null;
        }
        private GameObject DefineMode(ModeTypeNew type)
        {
            foreach (var mode in _mods)
            {
                if (mode.TypeMode == type)
                    return mode.ModeObject;
            }
            return null;
        }
        private void UpdateList(Seqcunce delete)
        {
            _curretList.Remove(delete);
            if (_curretList.Count == 0)
            {
                foreach (var sequnce in _controller.Seqcuncs)
                {
                    _curretList.Add(sequnce);
                }
            }
        }
    }
}