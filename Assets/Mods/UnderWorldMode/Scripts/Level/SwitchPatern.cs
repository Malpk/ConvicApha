using System.Collections;
using UnityEngine;
using Zenject;

namespace Underworld
{
    public class SwitchPatern : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private ModeType _chooseMode;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;
        [SerializeField] private PaternLoadConfig[] _configs;

        private GeneralMode _curretMode;

        public bool IsReady { get; private set; } = true;
        public bool IsPause { get; private set; } = false;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
        }

        private void Start()
        {
            if (_playOnStart)
                ActivateMode(_chooseMode);
        }

        public void ActivateMode(ModeType mode)
        {
            IsReady = false;
            _curretMode = GetPatern(mode);
            _curretMode.Intializate(_builder, _player);
            _curretMode.Activate();
            StartCoroutine(WaitComplitePatern(_curretMode));
        }

        public void Deactivate()
        {
            foreach (var term in _builder.Terms)
            {
                term.Deactivate();
                term.HideItem();
            }
            _curretMode.Deactivate();
        }
        private GeneralMode GetPatern(ModeType type)
        {
            foreach (var config in _configs)
            {
                if (config.TypeMode == type)
                {
                    var patern = config.Create();
                    patern.transform.position = transform.position;
                    patern.transform.parent = transform;
                    return patern;
                }
            }
            return null;
        }
        private IEnumerator WaitComplitePatern(GeneralMode patern)
        {
            _curretMode = patern;
            yield return new WaitWhile(() => patern.State == ModeState.Play);
            IsReady = true;
        }
    }
}