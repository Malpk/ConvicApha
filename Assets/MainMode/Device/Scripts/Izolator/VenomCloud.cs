using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class VenomCloud : DetectDeviceSet
    {
        [Header("Damage Setting")]
        [SerializeField] private DamageInfo _damageInfo;
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _cloudSprite;

        public DamageInfo DamageInfo => _damageInfo;

        public override void SetMode(bool mode)
        {
            if(_cloudSprite)
                _cloudSprite.enabled = mode;
            base.SetMode(mode);
        }
    }
}