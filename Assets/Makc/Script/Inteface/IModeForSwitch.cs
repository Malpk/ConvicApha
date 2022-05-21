using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public interface IModeForSwitch
    {
        public bool IsActive { get; }
        public void Constructor(SwitchMode swictMode);
        public void SetSetting(string jsonSetting);
    }
}