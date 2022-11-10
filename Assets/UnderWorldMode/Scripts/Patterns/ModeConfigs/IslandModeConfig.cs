using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/IslandModeConfig")]
    public class IslandModeConfig : PaternConfig
    {
        [SerializeField] private float _warningTime;
        [Header("Island Setting")]
        [SerializeField] private float _minDistanceBothIsland;
        [SerializeField] private Vector2Int _islandSize;

        public override ModeType TypeMode => ModeType.IslandMode;
        public float MinDistanceBothIsland => _minDistanceBothIsland;
        public float WarningTime => _warningTime;
        public Vector2Int IslandSize => _islandSize;
    }
}