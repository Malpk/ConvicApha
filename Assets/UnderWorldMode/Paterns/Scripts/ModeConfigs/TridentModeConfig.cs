using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/TridentModeConfig")]
    public class TridentModeConfig : PaternConfig
    {
        [SerializeField] private TridentPointConfig _configs;
        public override ModeType TypeMode => ModeType.TrindetMode;
        public TridentPointConfig PointConfig => _configs;
    }
}