using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;


namespace Underworld
{
    public class SwitchPatern : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private ModeType _chooseMode;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;
        [SerializeField] private PaternConfig[] _configs;

        private PaternConfig  _curretConfig;

        public bool IsReady { get; private set; } = true;

        private void Start()
        {
            if (_playOnStart)
            {
               ActivateModeAsync(_chooseMode);
            }
        }
        public void Intializate(MapBuilder builder, Player player)
        {
            _player = player;
            _builder = builder;
        }
        public async void ActivateModeAsync(ModeType type)
        {
            IsReady = false;
            _chooseMode = type;
            var load = Load(type);
            await load;
            if (load.Result != null)
            {
                load.Result.Intializate(_builder, _player);
                load.Result.Activate();
                StartCoroutine(WaitComplitePatern(load.Result));
            }
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
            yield return new WaitWhile(() => patern.State == ModeState.Play);
            _curretConfig.UnLoad();
            IsReady = true;
        }
    }
}