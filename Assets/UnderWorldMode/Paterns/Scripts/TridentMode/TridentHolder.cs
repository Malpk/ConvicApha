using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;


namespace Underworld
{
    public class TridentHolder : MonoBehaviour
    {
        [Header("Build Setting")]
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private int _countTrident;
        [SerializeField] private float _widthUnit;
        [SerializeField] private float _distanceOnCenter;
        [Header("Reference")]
        [SerializeField] private Trident _trident;
        [SerializeField] private TridentSetting _tridentSetting;
        private Dictionary<Trident, Vector2> _tridentPool = new Dictionary<Trident, Vector2>();

        public bool IsIntializate { get; private set; } = false;

        private async void Awake()
        {
            if(_trident && _tridentSetting && _playOnAwake)
                await IntializateAsync(_trident, _tridentSetting, _countTrident);
        }
        #region Intializate
        public async Task<bool> IntializateAsync(Trident trident, TridentSetting setting, int count)
        {
            if (!IsIntializate)
            {
                _countTrident = count;
                _trident = trident;
                IsIntializate = true;
                await Task.Run(()=>CreateTrident(count % 2 == 0 ? count : count - 1, setting));
            }
            return false;
        }

        private void CreateTrident(int count,TridentSetting setting)
        {
            var position = Vector2.left * (count/2 * _widthUnit - _widthUnit / 2);
            for (int i = 0; i < count; i++)
            {
                var trident = Instantiate(_trident.gameObject, transform).GetComponent<Trident>();
                trident.transform.localPosition = position + Vector2.right * i * _widthUnit;
                trident.Intilizate(setting);
                _tridentPool.Add(trident, trident.transform.localPosition);
            }
        }
        #endregion

        public bool GetFreeTrident(out Trident trident)
        {
            foreach (var pool in _tridentPool)
            {
                if (!pool.Key.IsActive)
                {
                    trident = pool.Key;
                    trident.transform.localPosition = pool.Value;
                    trident.SetDistance(_distanceOnCenter);
                    return true;
                }
            }
            trident = null;
            return false;
        }
        public bool GetGroup( out Trident[] group)
        {

            group = null;
            return false;
        }
    }
}