using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SwitchModeComponent
{
    public interface ISequence
    {
        public bool IsAttackMode { get; }
        public void Constructor(SwitchMods swictMode);
    }
}