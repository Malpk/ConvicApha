using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Underworld
{
    public interface IModeSetting
    {
        public abstract ModeTypeNew type { get; }
    }
}