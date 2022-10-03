using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;


namespace Underworld
{
    public class SwitchPatern : MonoBehaviour,IPause
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private ModeType _chooseMode;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;
        [SerializeField] private PaternLoadConfig[] _configs;

        private GeneralMode _curretMode;
        private PaternLoadConfig  _curretConfig;

        public bool IsReady { get; private set; } = true;
        public bool IsPause { get; private set; } = false;

        private async void Start()
        {
            if (_playOnStart)
            {
                 await ActivateModeAsync(_chooseMode);
            }
        }
        public void Intializate( Player player)
        {
            _player = player;
        }
        public async Task ActivateModeAsync(ModeType type, PaternConfig config = null)
        {
            IsReady = false;
            _chooseMode = type;
            var load = Load(type);
            await load;
            if (load.Result != null)
            {
                load.Result.Intializate(_builder, _player);
                if (config)
                    load.Result.Intializate(config);
                load.Result.Activate();
                StartCoroutine(WaitComplitePatern(load.Result));
            }
        }

        public void Deactivate()
        {
            _curretMode.Deactivate();
            _builder.ClearMap();
        }
        public void Pause()
        {
            IsPause = true;
        }

        public void UnPause()
        {
            IsPause = false;
        }
        private async Task<GeneralMode> Load(ModeType type)
        {
            foreach (var config in _configs)
            {
                if (config.TypeMode == type)
                {
                    _curretConfig = config;
                    var load = config.LoadAsync();
                    await load;
                    var patern = load.Result;
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
            _curretConfig.UnLoad();
            IsReady = true;
        }


    }
}