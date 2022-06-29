using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    [Serializable]
    public abstract class Artifact : Item
    {
        public int Count;
        public bool IsInfinity;

    }
}
