using UnityEngine;
using Underworld.Editors;

namespace Underworld
{
    [System.Serializable]
    public class ViseSetting : IModeSetting
    {
        [Header("Mods Setting")]
        [SerializeField] private float _minTimeOffset;
        [SerializeField] private float _maxTimeOffset;
        [Header("Time setting")]
        [SerializeField] private float _warningTime;

        public float MinTimeOffset => _minTimeOffset;
        public float MaxTimeOffset => _maxTimeOffset;
        public float WarningTime => _warningTime;

        public ModeTypeNew type => ModeTypeNew.ViseMode;
    }
}
