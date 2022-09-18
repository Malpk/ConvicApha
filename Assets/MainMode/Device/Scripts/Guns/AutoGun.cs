using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public abstract class AutoGun : Gun
    {
        [Header("Auto Mode")]
        [SerializeField] private bool _playOnStart;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_playOnStart)
                CompliteShowAnimation += Activate;
            ActivateAction += Launch;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (_playOnStart)
                CompliteShowAnimation -= Activate;
            ActivateAction -= Launch;
        }
    }
}