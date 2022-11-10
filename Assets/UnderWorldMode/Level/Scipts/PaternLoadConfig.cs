using System.Threading.Tasks;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName ="UnderWorld/PaternLoadConfig")]
    public class PaternLoadConfig : ScriptableObject
    {
        [SerializeField] private ModeType _typeMode;
        [SerializeField]private GeneralMode _perfab;

        private GeneralMode _asset;

        public ModeType TypeMode => _typeMode;

        public GeneralMode Create()
        {
            if (_asset)
                return _asset;
            _asset = Instantiate(_perfab.gameObject).GetComponent<GeneralMode>();
            return _asset;
        }

    }
}
