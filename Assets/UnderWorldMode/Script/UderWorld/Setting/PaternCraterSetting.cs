using UnityEngine;
using Underworld.Editors;

namespace Underworld
{
    [System.Serializable]
    public class PaternCraterSetting : IModeSetting
    {
        [Header("Game setting")]
        [SerializeField] protected float playSpeed;
        [SerializeField] protected bool iversionMode;
        [Header("Frame Setting")]
        [SerializeField] protected float errorColorDefaout;
        [SerializeField] protected Sprite _spriteAtlas;

        public float ChangeSpeed => playSpeed;
        public bool InversionMode => InversionMode;
        public float ErroColorDefout => errorColorDefaout;
        public Sprite SpriteAtlas => _spriteAtlas;

        public ModeTypeNew type => ModeTypeNew.PaternCreater;
    }
}