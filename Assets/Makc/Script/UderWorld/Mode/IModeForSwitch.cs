using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SwitchModeComponent
{
    public interface IModeForSwitch
    {
        public bool IsAttackMode { get; }
        public void Constructor(SwitchMode swictMode);
    }
}