using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/IslandModeConfig")]
    public abstract class PaternConfig : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private float _workDuration;

        public float WorkDuration => _workDuration;
        public abstract ModeType TypeMode { get; }
    }
}