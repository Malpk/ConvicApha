using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;

namespace Underworld
{
    public interface IModeForSwitch
    {
        public bool IsActive { get; }
        public void Constructor(SwitchMode swictMode);
    }
}