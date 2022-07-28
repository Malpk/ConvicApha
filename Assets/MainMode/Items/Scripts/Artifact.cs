using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    [Serializable]
    public abstract class Artifact : Item
    {
        [Min(1)]
        public int Count = 1;
        public bool IsInfinity;

    }
}
