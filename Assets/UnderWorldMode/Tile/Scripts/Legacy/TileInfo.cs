using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class TileInfo
    {
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _activateTime;

        public float WarningTime => _warningTime;
        public float ActivateTine => _activateTime;
    }
}