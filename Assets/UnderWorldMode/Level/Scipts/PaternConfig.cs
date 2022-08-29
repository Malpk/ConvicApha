using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Underworld
{
    [CreateAssetMenu(menuName ="UnderWorld/PaternConfig")]
    public class PaternConfig : ScriptableObject
    {
        [SerializeField] private ModeType _typeMode;
        [SerializeField] private string _loadKey;

        private GeneralMode _asset;

        private bool _isLoad = false;
        public ModeType TypeMode => _typeMode;

        private void OnDisable()
        {
            _isLoad = false;
            if (_asset)
                UnLoad();
        }

        public async Task<GeneralMode> LoadAsync()
        {
#if UNITY_EDITOR
            if (_isLoad)
                throw new System.Exception("GameObject is already loaded");
#endif
            _isLoad = true;
            var load = Addressables.InstantiateAsync(_loadKey).Task;
            await load;
            _asset = load.Result.GetComponent<GeneralMode>();
            return _asset;
        }

        public void UnLoad()
        {
            _isLoad = false;
            Addressables.ReleaseInstance(_asset.gameObject);
        }

    }
}
