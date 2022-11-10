using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName ="UnderWorld/PaternConfigs/DefoutModeConfig")]
    public class DefoutModeConfig : PaternConfig
    {
        [Min(1)]
        [SerializeField] private float _spawnDistance = 5;
        [Range(0,1)]
        [SerializeField] private float _spawnDelay = 0.1f;

        public override ModeType TypeMode => ModeType.BaseMode;
        public float SpawnDistance => _spawnDistance;
        public float SpawnDelay => _spawnDelay;
    }
}