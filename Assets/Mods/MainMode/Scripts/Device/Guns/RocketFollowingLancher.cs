using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class RocketFollowingLancher : Gun
    {
        [SerializeField] private SmartProjectale _rocket;
        public override TrapType DeviceType => TrapType.RocketFollowingLauncher;

        protected override void ActivateDevice()
        {
            if (!_rocket.IsActivate)
            {
                base.ActivateDevice();
                _rocket.transform.position = transform.position;
                _rocket.transform.rotation = transform.rotation;
                _rocket.Activate(target);
            }
        }
    }
}