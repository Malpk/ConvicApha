using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public abstract class Device : KI
    {
        public abstract TrapType DeviceType { get; }
    }
}