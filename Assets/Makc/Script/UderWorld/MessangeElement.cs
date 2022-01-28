using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class MessangeElement
    {
        [SerializeField] private ModeType _type;
        [SerializeField] private string _messange;

        public ModeType type => _type;
        public string messange => _messange;
    }
}